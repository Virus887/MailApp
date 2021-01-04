namespace MailApp.Models.Accounts
{
    public class AccountsListViewModel : AccountsQuery
    {
        public int AccountId { get; set; }
        public AccountViewModel[] Accounts { get; set; }
    }
}