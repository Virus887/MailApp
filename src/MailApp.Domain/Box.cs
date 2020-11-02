using System;
using System.Collections.Generic;

namespace MailApp.Domain
{
    public class Box
    {
        public List<Message> Messages { get; private set; } = new List<Message>();

        public Message CreateMessage() => new Message();

        public void ReadMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            message.MarkAsRead();
        }
    }
}