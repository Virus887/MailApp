using System.ComponentModel.DataAnnotations;
namespace MailApp.Models.Groups
{
    public class RemoveMemberViewModel
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string AccountNick { get; set; }
    }
}