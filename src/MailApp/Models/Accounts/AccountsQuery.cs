using System;

namespace MailApp.Models.Accounts
{
    public class AccountsQuery
    {
        
        public String Nick { get; set; }
        public String Email { get; set; }
        public SortingOptions? Sort { get; set; }

        public enum SortingOptions
        {
            Nick,
            Email,
        }
    }
}