using System.ComponentModel.DataAnnotations;
namespace MailApp.Models.Groups
{
    public class RemoveGroupViewModel
    {
        [Required]
        public int GroupId { get; set; }
        public string Name { get; set; }
    }
}