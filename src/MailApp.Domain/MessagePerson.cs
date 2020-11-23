using System;

namespace MailApp.Domain
{
    public class MessagePerson
    {
        public int AccountId { get; private set; }
        public Account Account { get; private set; }
        public int MessageId { get; private set; }
        public Message Message { get; private set; }
        public MessagePersonType Type { get; private set; }
        public int TypeId { get; private set; }

        private MessagePerson()
        {
        }

        public MessagePerson(Account account, Message message, MessagePersonType type)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            AccountId = account.Id;
            MessageId = message.Id;
            TypeId = type.Id;
        }
    }
}