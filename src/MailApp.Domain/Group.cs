using System;
using System.Collections.Generic;
using System.Linq;

namespace MailApp.Domain
{
    public class Group
    {
        public string Name { get; set; }

        public int Id { get; }

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
            if (Accounts.Any(x => x.Id == account.Id))
            {
                Accounts.Add(account);
            }
        }

        public void RemoveAccount(Account account)
        {
            if (Accounts.Any(x => x.Id == account.Id))
            {
                Accounts.Remove(account);
            }
        }

        //Przeszukiwanie
        public Account FindMemberByNick(string nick)
        {
            foreach (Account a in Accounts)
            {
                if (a.Nick == nick) return a;
            }

            //tu nie pamietam co sie zwracalo
            return null;
        }
    }
}