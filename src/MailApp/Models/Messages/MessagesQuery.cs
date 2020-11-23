namespace MailApp.Models.Messages
{
    public class MessagesQuery
    {
        public int? SenderId { get; set; }
        public string Search { get; set; }
        public SortingOptions Sort { get; set; }

        public enum SortingOptions
        {
            Date,
            Nick,
            Subject
        }
    }
}