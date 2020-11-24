using System.ComponentModel.DataAnnotations;

namespace MailApp.Models.Groups
{
    public class AddMemberViewModel
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string AccountNick { get; set; }
    }
}