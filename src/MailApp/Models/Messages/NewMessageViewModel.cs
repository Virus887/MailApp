using MailApp.Models.Accounts;

namespace MailApp.Models.Messages
{
    public class NewMessageViewModel
    {
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public bool Notification { get; set; }
        public string Text { get; set; }

        public string Cc { get; set; }
        public string Bcc { get; set; }

        public AccountViewModel[] LastReceivers { get; set; }
    }
}
