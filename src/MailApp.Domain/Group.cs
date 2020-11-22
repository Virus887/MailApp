using System;
using System.Collections.Generic;
using System.Linq;

namespace MailApp.Domain
{
    public class Group : Entity<int>
    {
        public string Name { get; private set; }
        public ICollection<GroupAccount> GroupAccounts { get; private set; } = new List<GroupAccount>();

        private Group()
        {
        }

        public Group(string name, Account owner)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            AddAccount(owner, GroupAccountType.Owner);
        }

        private void AddAccount(Account account, GroupAccountType type)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));

            if (GroupAccounts.All(x => x.Account != account))
            {
                var groupAccount = new GroupAccount(account, this, type);
                GroupAccounts.Add(groupAccount);
                account.GroupAccounts.Add(groupAccount);
            }
        }

        public void AddAccount(Account account) => AddAccount(account, GroupAccountType.Member);

        public void RemoveAccount(Account account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));

            var groupAccount = GroupAccounts.SingleOrDefault(x => x.Account == account && x.Type == GroupAccountType.Member);
            if (groupAccount != null)
            {
                GroupAccounts.Remove(groupAccount);
                account.GroupAccounts.Remove(groupAccount);
            }
        }

        public Account FindMemberByNick(string nick)
        {
            if (String.IsNullOrEmpty(nick))
            {
                throw new ArgumentNullException(nameof(nick));
            }

            return GroupAccounts.FirstOrDefault(a => a.Account.Nick == nick)?.Account;
        }

        public bool IsMember(Account account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));
            return GroupAccounts.Any(x => x.Account == account);
        }

        public bool IsOwner(Account account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));
            return GroupAccounts.Any(x => x.Account == account && x.Type == GroupAccountType.Owner);
        }
    }
}