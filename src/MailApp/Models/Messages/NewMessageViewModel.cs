using MailApp.Models.Accounts;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MailApp.Models.Messages
{
    public class NewMessageViewModel //: IValidatableObject
    {
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public bool Notification { get; set; }
        public string Text { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public List<IFormFile> FileForm { get; set; }
        public AccountViewModel[] LastReceivers { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
            //if (string.IsNullOrEmpty(Receiver) && string.IsNullOrEmpty(Cc) && string.IsNullOrEmpty(Bcc))
            //{
            //    yield return new ValidationResult("Message Receiver is required.", new List<string> { "Receiver", "Bcc", "Cc"});
            //}
            //if (string.IsNullOrEmpty(Subject) && string.IsNullOrEmpty(Text))
            //{
            //    yield return new ValidationResult("Message has to have subject or text.", new List<string> { "Subject", "Text" });
            //}
        //}

    }
}
