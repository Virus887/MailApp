using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailApp.Domain;
using MailApp.Infrastructure;
using MailApp.Models;
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
        public async Task<IActionResult> Index(MessagesQueryViewModel requestModel, CancellationToken cancellationToken)
        {
            var senders = await MailAppDbContext.Accounts
                .ToArrayAsync(cancellationToken);

            //TODO: filtrowanie wiadomości po aktualnym użytkowniku
            var messages = await MailAppDbContext.Messages
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x => requestModel.SenderId == null || x.Sender.Id == requestModel.SenderId)
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            var viewModel = new MessagesViewModel
            {
                SenderId = requestModel.SenderId,
                MessageId = requestModel.MessageId,
                Senders = senders
                    .Select(x => new AccountViewModel(x))
                    .ToArray(),
                MessageList = new MessageListViewModel
                {
                    Messages = messages.Values
                        .Select(x => new MessageViewModel(x))
                        .ToArray(),
                }
            };

            if (viewModel.MessageId.HasValue)
            {
                var message = messages[viewModel.MessageId.Value];
                viewModel.Details = new MessageViewModel
                {
                    MessageId = message.Id,
                    Sender = new AccountViewModel
                    {
                        AccountId = message.Sender.Id,
                        Nick = message.Sender.Nick,
                        Email = message.Sender.Email,
                    },
                    Subject = message.Subject,
                    Text = message.Text,
                    SentDate = message.SentDate
                };
            }

            return View(viewModel);
        }

        [HttpGet("/Message/Details/{messageId}")]
        public async Task<IActionResult> Details(int messageId, CancellationToken cancellationToken)
        {
            var message = await MailAppDbContext.Messages
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .SingleOrDefaultAsync(x => x.Id == messageId, cancellationToken);

            if (message == null)
            {
                return NotFound();
            }

            message.MarkAsRead();
            await MailAppDbContext.SaveChangesAsync(cancellationToken);

            var viewModel = new MessageViewModel
            {
                MessageId = message.Id,
                Sender = new AccountViewModel
                {
                    AccountId = message.Sender.Id,
                    Nick = message.Sender.Nick,
                    Email = message.Sender.Email,
                },
                Subject = message.Subject,
                Text = message.Text,
                SentDate = message.SentDate
            };

            return PartialView(viewModel);
        }

        [HttpGet]
        public IActionResult NewMessage() => View();

        [HttpPost]
        public async Task<IActionResult> NewMessage(NewMessageViewModel m, CancellationToken cancellationToken)
        {
            if (m.Subject == null)
            {
                //TODO: INFO ZE PUSTA WIADOMOSC
            }

            var sender = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var message = new Message
            {
                Subject = m.Subject,
                Text = m.Text,
                Sender = sender,
                SentDate = DateTime.Now
            };
            if (m.Notification)
            {
                message.AddNotification();
            }

            if (m.Email == null)
            {
                return BadRequest();
            }

            var receiver = await MailAppDbContext.Accounts.SingleOrDefaultAsync(a => a.Email == m.Email, cancellationToken);
            if (receiver == null)
            {
                return BadRequest();
            }

            MailAppDbContext.Messages.Add(message);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }
    }
}