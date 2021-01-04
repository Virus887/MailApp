using System;
using System.Collections.Generic;
using System.Text;

namespace MailApp.Domain
{
    public class MessageAttachment
    {
        public string ExternalId { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }

        private MessageAttachment()
        {
        }

        public MessageAttachment(string externalId, string name, string type)
        {
            if (String.IsNullOrEmpty(externalId)) throw new ArgumentNullException(nameof(externalId));
            else ExternalId = externalId;
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            else Name = name;
            if (String.IsNullOrEmpty(type)) throw new ArgumentNullException(nameof(type));
            else Type = type;
        }
    }
}