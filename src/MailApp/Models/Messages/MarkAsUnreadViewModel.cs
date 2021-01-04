using System.ComponentModel.DataAnnotations;

namespace MailApp.Models.Messages
{
    public class MarkAsUnreadViewModel
    {
        [Required]
        public int MessageId { get; set; }
    }
}