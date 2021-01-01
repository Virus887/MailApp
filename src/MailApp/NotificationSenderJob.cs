using MailApp.Infrastructure;
using MailApp.Infrastructure.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailApp.Domain;

namespace MailApp
{
    public class NotificationSenderJob
    {
        private MailAppDbContext MailAppDbContext { get; }
        private INotificationClient NotificationClient { get; }

        public NotificationSenderJob(MailAppDbContext mailAppDbContext, INotificationClient notificationClient)
        {
            MailAppDbContext = mailAppDbContext;
            NotificationClient = notificationClient;
        }

        public async Task SendNotifications()
        {
            var messages = await MailAppDbContext.Messages
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Account)
                .Include(x => x.MessagePersons)
                .ThenInclude(x => x.Type)
                .Where(x => x.Notification && x.MessagePersons.Any(y => !y.IsRead))
                .ToArrayAsync();

            var sendNotificationRequests = messages
                .SelectMany(x => x.MessagePersons.Where(y => !y.IsRead && y.Type != MessagePersonType.Sender), (message, messagePerson) =>
                    new SendNotificationRequest
                    {
                        Content = $"You have not read message from {message.Sender.Nick}.",
                        ContentType = "text/html",
                        RecipientsList = new[] {messagePerson.Account.Email},
                        WithAttachments = false
                    })
                .ToArray();

            await Task.WhenAll(sendNotificationRequests.Select(NotificationClient.SendNotification));
        }
    }
}