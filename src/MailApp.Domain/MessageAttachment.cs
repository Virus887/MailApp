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
            ExternalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}