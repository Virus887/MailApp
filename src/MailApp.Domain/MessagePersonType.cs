namespace MailApp.Domain
{
    public class MessagePersonType : Entity<int>
    {
        public string Name { get; private set; }

        private MessagePersonType()
        {
        }

        private MessagePersonType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static MessagePersonType Sender = new MessagePersonType(1, nameof(Sender));
        public static MessagePersonType Receiver = new MessagePersonType(2, nameof(Receiver));
        public static MessagePersonType Cc = new MessagePersonType(3, nameof(Cc));
        public static MessagePersonType Bcc = new MessagePersonType(4, nameof(Bcc));
    }
}