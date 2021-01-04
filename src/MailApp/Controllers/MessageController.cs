using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using MailApp.Domain;
using MailApp.Infrastructure;
using MailApp.Models.Accounts;
using MailApp.Models.Messages;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace MailApp.Controllers
{
    public class MessageController : Controller
    {
        private MailAppDbContext MailAppDbContext { get; }
        private IAccountProvider AccountProvider { get; }
        private BlobContainerClient ContainerClient { get; }
        private TelemetryClient TelemetryClient { get; }

        public MessageController(MailAppDbContext mailAppDbContext, IAccountProvider accountProvider, BlobContainerClient containerClient, TelemetryClient telemetryClient)
        {
            MailAppDbContext = mailAppDbContext;
            AccountProvider = accountProvider;
            ContainerClient = containerClient;
            TelemetryClient = telemetryClient;
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
                        .Select(x => new MessageViewModel(x, owner))
                        .ToArray(),
                }
            };
            return View(viewModel);
        }

        [HttpGet("/Message/Details/{messageId}")]
        [SwaggerOperation(Summary = "Get information about message",
            Description = "Returns partial view showing content and all information about message with specified Id.")]
        public async Task<IActionResult> Details(int messageId, CancellationToken cancellationToken)
        {
            var message = await MailAppDbContext.Messages
                .Include(x => x.MessageAttachments)
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Account)
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Type)
                .SingleOrDefaultAsync(x => x.Id == messageId, cancellationToken);

            if (message == null)
            {
                return NotFound();
            }

            var currentAccount = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            message.MarkAsRead(currentAccount);

            await MailAppDbContext.SaveChangesAsync(cancellationToken);

            var viewModel = new MessageViewModel(message, currentAccount);
            return PartialView(viewModel);
        }

        [HttpGet("/Message/Details/{messageId}/Attachments/{attachmentId}")]
        [SwaggerOperation(Summary = "Download attachments from message",
            Description = "Downloads attachment with given Id from message with given Id.")]
        public async Task<IActionResult> DownloadAttachments(int messageId, string attachmentId, CancellationToken cancellationToken)
        {
            var message = await MailAppDbContext.Messages
                .Include(x => x.MessageAttachments)
                .SingleOrDefaultAsync(x => x.Id == messageId, cancellationToken);

            if (message == null)
            {
                return NotFound();
            }

            var attachment = message.MessageAttachments.SingleOrDefault(x => x.ExternalId == attachmentId);
            if (attachment == null)
            {
                return NotFound();
            }

            var blobClient = ContainerClient.GetBlobClient(attachmentId);
            var blobInfo = await blobClient.DownloadAsync(cancellationToken);
            return File(blobInfo.Value.Content, attachment.Type, attachment.Name);
        }

        [HttpGet("/Message/NewMessage")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetNewMessage(NewMessageViewModel viewModel, CancellationToken cancellationToken)
        {
            await ReadLatestReceivers(viewModel, cancellationToken);
            return View("NewMessage", viewModel);
        }

        private async Task ReadLatestReceivers(NewMessageViewModel viewModel, CancellationToken cancellationToken)
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
        }

        [HttpPost("/Message/NewMessage")]
        [SwaggerOperation(Summary = "Create new message",
            Description = "Creates a new message and sends it to appropriate receivers. Returns default mail app view." +
                          "There must be at least one receiver(casual, Bcc or CC) and subject must be not empty to send a message.")]
        public async Task<IActionResult> NewMessage(NewMessageViewModel request, CancellationToken cancellationToken)
        {
            await ReadLatestReceivers(request, cancellationToken);

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            if (string.IsNullOrEmpty(request.Receiver) && string.IsNullOrEmpty(request.Cc) && string.IsNullOrEmpty(request.Bcc))
            {
                ModelState.AddModelError(nameof(request.Receiver), "There is no receiver.");
                return View(request);
            }

            if (string.IsNullOrEmpty(request.Subject) && string.IsNullOrEmpty(request.Text))
            {
                ModelState.AddModelError(nameof(request.Subject), "Message should have subject or text");
                return View(request);
            }

            var message = new Message
            {
                Subject = request.Subject,
                Text = request.Text,
                SentDate = DateTime.Now,
                Notification = request.Notification,
            };

            var sender = await AccountProvider.GetAccountForCurrentUser(cancellationToken);

            foreach (var i in request.FileForm ?? new IFormFile[0])
            {
                var fileName = i.FileName;
                var contentType = i.ContentType;
                var blobId = Guid.NewGuid().ToString();
                message.AddAttachments(new MessageAttachment(blobId, fileName, contentType));

                var blobClient = ContainerClient.GetBlobClient(blobId);
                await blobClient.UploadAsync(i.OpenReadStream(), true, cancellationToken);
            }

            if (sender == null)
            {
                return BadRequest();
            }

            message.SetSender(sender);

            var flag = true;

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
                        else
                        {
                            flag = false;
                        }
                    }
                }
            }

            await SetPersons(request.Receiver, account => message.AddReceiver(account), group => message.AddReceiver(group));
            if (flag == false)
            {
                ModelState.AddModelError(nameof(request.Receiver), "Invadlid mail or group name");
                return View(request);
            }

            await SetPersons(request.Cc, account => message.AddCc(account), group => message.AddCc(group));
            if (flag == false)
            {
                ModelState.AddModelError(nameof(request.Cc), "Invadlid mail or group name");
                return View(request);
            }

            await SetPersons(request.Bcc, account => message.AddReceiver(account), group => message.AddBcc(group));
            if (flag == false)
            {
                ModelState.AddModelError(nameof(request.Bcc), "Invadlid mail or group name");
                return View(request);
            }

            MailAppDbContext.Messages.Add(message);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            TelemetryClient.TrackEvent("Message sent");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("[action]")]
        [SwaggerOperation(Summary = "Remove message", Description = "Removes message with given Id from database. Returns default mail app view.")]
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

        [HttpPost("[action]")]
        [SwaggerOperation(Summary = "Mark message as unread", Description = "Changes message with given Id IsRead property to false. Returns default mail app view.")]
        public async Task<IActionResult> MarkAsUnread(MarkAsUnreadViewModel m, CancellationToken cancellationToken)
        {
            var message = await MailAppDbContext.Messages.SingleOrDefaultAsync(x => x.Id == m.MessageId, cancellationToken);

            if (message == null)
            {
                return BadRequest();
            }

            var currentAccount = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            message.MarkAsUnread(currentAccount);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}