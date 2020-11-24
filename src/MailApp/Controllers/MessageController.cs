using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailApp.Domain;
using MailApp.Infrastructure;
using MailApp.Models.Accounts;
using MailApp.Models.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MailApp.Controllers
{
    public class MessageController : Controller
    {
        private MailAppDbContext MailAppDbContext { get; }
        private IAccountProvider AccountProvider { get; }

        public MessageController(MailAppDbContext mailAppDbContext, IAccountProvider accountProvider)
        {
            MailAppDbContext = mailAppDbContext;
            AccountProvider = accountProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index(MessagesQuery query, CancellationToken cancellationToken)
        {
            var senders = await MailAppDbContext.Accounts
                .ToArrayAsync(cancellationToken);

            var owner = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var messages = await MailAppDbContext.Messages
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Account)
                .ToArrayAsync(cancellationToken);

            messages = messages
                .Where(x => query.SenderId == null || x.Sender.Id == query.SenderId)
                .Where(x => x.MessagePersons.Any(y => y.Type != MessagePersonType.Sender && y.Account == owner))
                .ToArray();

            if (!String.IsNullOrEmpty(query.Search))
            {
                foreach (var part in query.Search.Split(" "))
                {
                    messages = messages.Where(x => (x.Subject + x.Text).Contains(part)).ToArray();
                }
            }

            messages = query.Sort switch
            {
                MessagesQuery.SortingOptions.Subject => messages.OrderBy(x => x.Subject).ToArray(),
                MessagesQuery.SortingOptions.Date => messages.OrderBy(x => x.SentDate).ToArray(),
                MessagesQuery.SortingOptions.Nick => messages.OrderBy(x => x.Sender.Nick).ToArray(),
                _ => messages.OrderBy(x => x.SentDate).ToArray()
            };

            var viewModel = new MessagesListViewModel
            {
                SenderId = query.SenderId,
                Senders = senders
                    .Select(x => new AccountViewModel(x))
                    .ToArray(),
                MessageList = new MessageListViewModel
                {
                    Messages = messages
                        .Select(x => new MessageViewModel(x))
                        .ToArray(),
                }
            };

            return View(viewModel);
        }

        [HttpGet("/Message/Details/{messageId}")]
        public async Task<IActionResult> Details(int messageId, CancellationToken cancellationToken)
        {
            var message = await MailAppDbContext.Messages
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Account)
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Type)
                .SingleOrDefaultAsync(x => x.Id == messageId, cancellationToken);

            if (message == null)
            {
                return NotFound();
            }

            message.MarkAsRead();
            await MailAppDbContext.SaveChangesAsync(cancellationToken);

            var viewModel = new MessageViewModel(message);
            return PartialView(viewModel);
        }

        [HttpGet("/Message/NewMessage")]
        public async Task<IActionResult> GetNewMessage(NewMessageViewModel viewModel, CancellationToken cancellationToken)
        {
            var sender = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var lastReceivers = await MailAppDbContext.Messages
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Account)
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Type)
                .Where(x => x.MessagePersons.Any(y => y.Type != MessagePersonType.Sender && y.Account == sender))
                .OrderByDescending(x => x.SentDate)
                .Distinct()
                .Take(5)
                .ToArrayAsync(cancellationToken);

            viewModel.LastReceivers = lastReceivers
                .SelectMany(x => x.Receivers)
                .Distinct()
                .Select(x => new AccountViewModel(x))
                .ToArray();

            return View("NewMessage", viewModel);
        }

        [HttpPost("/Message/NewMessage")]
        public async Task<IActionResult> NewMessage(NewMessageViewModel request, CancellationToken cancellationToken)
        {
            if (request.Subject == null)
            {
                //TODO: INFO ZE PUSTA WIADOMOSC
            }

            var message = new Message
            {
                Subject = request.Subject,
                Text = request.Text,
                SentDate = DateTime.Now,
                Notification = request.Notification
            };

            if (request.Receiver == null)
            {
                return BadRequest();
            }

            var sender = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            message.SetSender(sender);

            async Task SetPersons(String addresses, Action<Account> f1, Action<Group> f2)
            {
                foreach (var address in (addresses ?? String.Empty).Split(";", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray())
                {
                    var group = await MailAppDbContext.Groups.Include(x => x.GroupAccounts).ThenInclude(x => x.Account).SingleOrDefaultAsync(x => x.Name == address, cancellationToken);

                    if (group != null)
                    {
                        f2(group);
                    }
                    else
                    {
                        var account = await MailAppDbContext.Accounts.SingleOrDefaultAsync(x => x.Email == address, cancellationToken);
                        if (account != null)
                        {
                            f1(account);
                        }
                    }
                }
            }

            await SetPersons(request.Receiver, account => message.AddReceiver(account), group => message.AddReceiver(group));
            await SetPersons(request.Cc, account => message.AddCc(account), group => message.AddCc(group));
            await SetPersons(request.Bcc, account => message.AddReceiver(account), group => message.AddBcc(group));

            MailAppDbContext.Messages.Add(message);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMessage(RemoveMessageViewModel m, CancellationToken cancellationToken)
        {
            var message = await MailAppDbContext.Messages.SingleOrDefaultAsync(x => x.Id == m.MessageId, cancellationToken);

            if (message == null)
            {
                return BadRequest();
            }

            MailAppDbContext.Messages.Remove(message);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsUnread(MarkAsUnreadViewModel m, CancellationToken cancellationToken)
        {
            var message = await MailAppDbContext.Messages.SingleOrDefaultAsync(x => x.Id == m.MessageId, cancellationToken);

            if (message == null)
            {
                return BadRequest();
            }

            message.MarkAsUnRead();
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}