namespace MailApp.Models.Accounts
{
    public class AccountsListViewModel : AccountsQuery
    {
        public AccountViewModel[] Accounts { get; set; }
    }
}