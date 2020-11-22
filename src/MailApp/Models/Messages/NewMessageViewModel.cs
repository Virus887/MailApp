namespace MailApp.Models.Messages
{
    public class NewMessageViewModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public bool Notification { get; set; }
        public string Text { get; set; }

        public AccountViewModel[] LastReceivers { get; set; }
    }
}
