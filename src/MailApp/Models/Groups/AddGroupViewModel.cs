using System.ComponentModel.DataAnnotations;

namespace MailApp.Models.Groups
{
    public class AddGroupViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}