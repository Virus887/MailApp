using System;
using System.Collections.Generic;
using System.Linq;
using MailApp.Domain;
using MailApp.Models.Accounts;

namespace MailApp.Models.Messages
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public AccountViewModel Sender { get; set; }
        public AccountViewModel[] Receivers { get; set; }
        public AccountViewModel[] Cc { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public Dictionary<string, string> Attachments { get; set; }

        public MessageViewModel()
        {
        }

        public MessageViewModel(Message message)
        {
            MessageId = message.Id;
            Sender = new AccountViewModel(message.Sender);
            Receivers = message.Receivers.Select(x => new AccountViewModel(x)).ToArray();
            Cc = message.Cc.Select(x => new AccountViewModel(x)).ToArray();
            SentDate = message.SentDate;
            IsRead = message.IsRead;
            Subject = message.Subject;
            Text = message.Text;
            Attachments = message.MessageAttachments.ToDictionary(x => x.ExternalId, x => x.Name);
        }
    }
}