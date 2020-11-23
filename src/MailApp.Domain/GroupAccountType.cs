namespace MailApp.Domain
{
    public class GroupAccountType : Entity<int>
    {
        public string Name { get; private set; }

        private GroupAccountType()
        {
        }

        private GroupAccountType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static GroupAccountType Owner = new GroupAccountType(1, nameof(Owner));
        public static GroupAccountType Member = new GroupAccountType(2, nameof(Member));
    }
}