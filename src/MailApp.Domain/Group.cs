using System;
using System.Collections.Generic;
using System.Linq;

namespace MailApp.Domain
{
    public class Group : Entity<int>
    {
        public string Name { get; set; }


        //Tu w sumie mozemy zrobic Dictionary i nadawac szutcznie id 
        public List<Account> Accounts { get; set; }


        //Methods
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

        //Przeszukiwanie
        public Account FindMemberByNick(string nick)
        {
            if (String.IsNullOrEmpty(nick))
            {
                throw new ArgumentNullException(nameof(nick));
            }

            foreach (Account a in Accounts)
            {
                if (a.Nick == nick) return a;
            }

            //tu nie pamietam co sie zwracalo
            return null;
        }
    }
}