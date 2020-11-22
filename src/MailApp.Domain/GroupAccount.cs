using System;

namespace MailApp.Domain
{
    public class GroupAccount
    {
        public Int32 AccountId { get; private set; }
        public Account Account { get; private set; }
        public Int32 GroupId { get; private set; }
        public Group Group { get; private set; }

        private GroupAccount()
        {
        }

        public GroupAccount(Account account, Group group)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Group = group ?? throw new ArgumentNullException(nameof(group));
            AccountId = account.Id;
            GroupId = group.Id;
        }
    }
}