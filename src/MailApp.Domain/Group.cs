using System;
using System.Collections.Generic;
using System.Linq;

namespace MailApp.Domain
{
    public class Group : Entity<int>
    {
        public string Name { get; set; }
        public List<Account> Accounts { get; set; }

        private Group()
        {
        }

        public Group(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Accounts = new List<Account>();
        }

        public void AddAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (!Accounts.Any(x => x == account))
            {
                Accounts.Add(account);
            }
        }

        public void RemoveAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (Accounts.Any(x => x == account))
            {
                Accounts.Remove(account);
            }
        }

        public Account FindMemberByNick(string nick)
        {
            if (String.IsNullOrEmpty(nick))
            {
                throw new ArgumentNullException(nameof(nick));
            }

            return Accounts.FirstOrDefault(a => a.Nick == nick);
        }
    }
}