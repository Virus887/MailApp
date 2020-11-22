namespace MailApp.Models.Groups
{
    public class GroupDetailsViewModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public AccountViewModel[] Members { get; set; }
    }
}