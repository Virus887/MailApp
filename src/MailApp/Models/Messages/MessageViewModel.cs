using System;
using MailApp.Domain;
using MailApp.Models.Accounts;

namespace MailApp.Models.Messages
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public AccountViewModel Sender { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get;  set; }
        public string Subject { get; set; }
        public string Text { get; set; }

        public MessageViewModel()
        {
        }

        public MessageViewModel(Message message)
        {
            MessageId = message.Id;
            Sender = new AccountViewModel(message.Sender);
            SentDate = message.SentDate;
            IsRead = message.IsRead;
            Subject = message.Subject;
            Text = message.Text;
        }
    }  
}
