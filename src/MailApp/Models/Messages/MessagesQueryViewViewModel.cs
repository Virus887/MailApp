using MailApp.Models.Accounts;

namespace MailApp.Models.Messages
{
    public class MessagesViewModel : MessagesQueryViewModel
    {
        public AccountViewModel[] Senders { get; set; } = new AccountViewModel[0];
        public MessageListViewModel MessageList { get; set; }
    }
}