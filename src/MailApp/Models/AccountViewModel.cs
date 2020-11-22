using MailApp.Domain;

namespace MailApp.Models
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public string Nick { get; set; }
        public string Email { get; set; }

        public AccountViewModel()
        {
        }

        public AccountViewModel(Account account)
        {
            AccountId = account.Id;
            Nick = account.Nick;
            Email = account.Email;
        }
    }
}