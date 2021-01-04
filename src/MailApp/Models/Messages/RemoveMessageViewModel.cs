using System.ComponentModel.DataAnnotations;

namespace MailApp.Models.Messages
{
    public class RemoveMessageViewModel
    {
        [Required]
        public int MessageId { get; set; }
    }
}