using System;

namespace MailApp.Domain
{
    public class GroupAccount
    {
        public int AccountId { get; private set; }
        public Account Account { get; private set; }
        public int GroupId { get; private set; }
        public Group Group { get; private set; }
        public GroupAccountType Type { get; private set; }


        private GroupAccount()
        {
        }

        public GroupAccount(Account account, Group group, GroupAccountType type)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Group = group ?? throw new ArgumentNullException(nameof(group));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            AccountId = account.Id;
            GroupId = group.Id;
        }
    }
}