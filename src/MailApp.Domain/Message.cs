﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MailApp.Domain
{
    public class Message : Entity<int>
    {
        public Account Sender => MessagePersons.First(x => x.Type == MessagePersonType.Sender).Account;
        public Account[] Receivers => MessagePersons.Where(x => x.Type == MessagePersonType.Receiver).Select(x => x.Account).ToArray();
        public Account[] Cc => MessagePersons.Where(x => x.Type == MessagePersonType.Cc).Select(x => x.Account).ToArray();
        public Account[] Bcc => MessagePersons.Where(x => x.Type == MessagePersonType.Bcc).Select(x => x.Account).ToArray();
        public ICollection<MessagePerson> MessagePersons { get; set; } = new List<MessagePerson>();
        public ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();

        public string Subject { get; set; }
        public string Text { get; set; }

        public DateTime SentDate { get; set; }

        public bool IsRead(Account account) =>
            MessagePersons.Where(x => x.Account == account).Any(x => x.IsRead);

        public void MarkAsRead(Account account)
        {
            foreach (var messagePerson in MessagePersons.Where(x => x.Account == account))
            {
                messagePerson.IsRead = true;
            }
        }

        public void MarkAsUnread(Account account)
        {
            foreach (var messagePerson in MessagePersons.Where(x => x.Account == account))
            {
                messagePerson.IsRead = false;
            }
        }

        public bool Notification { get; set; }

        public Message()
        {
            Notification = false;
        }

        public void DeleteNotification() => Notification = false;

        public void SetSender(Account sender)
        {
            _ = sender ?? throw new ArgumentNullException(nameof(sender));

            var actualReceiver = MessagePersons.FirstOrDefault(x => x.Type == MessagePersonType.Sender);
            if (actualReceiver != null)
            {
                MessagePersons.Remove(actualReceiver);
            }
            MessagePersons.Add(new MessagePerson(sender, this, MessagePersonType.Sender));
        }

        public void AddReceiver(Account account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));

            if (!MessagePersons.Any(x => x.Type == MessagePersonType.Receiver && x.Account == account))
            {
                MessagePersons.Add(new MessagePerson(account, this, MessagePersonType.Receiver));
            }
        }

        public void AddCc(Account account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));

            if (!MessagePersons.Any(x => x.Type == MessagePersonType.Cc && x.Account == account))
            {
                MessagePersons.Add(new MessagePerson(account, this, MessagePersonType.Cc));
            }
        }

        public void AddBcc(Account account)
        {
            _ = account ?? throw new ArgumentNullException(nameof(account));

            if (!MessagePersons.Any(x => x.Type == MessagePersonType.Bcc && x.Account == account))
            {
                MessagePersons.Add(new MessagePerson(account, this, MessagePersonType.Bcc));
            }
        }

        public void AddReceiver(Group group)
        {
            _ = group ?? throw new ArgumentNullException(nameof(group));

            foreach (var member in group.Members)
            {
                AddReceiver(member);
            }
        }

        public void AddCc(Group group)
        {
            _ = group ?? throw new ArgumentNullException(nameof(group));

            foreach (var member in group.Members)
            {
                AddCc(member);
            }
        }

        public void AddBcc(Group group)
        {
            _ = group ?? throw new ArgumentNullException(nameof(group));

            foreach (var member in group.Members)
            {
                AddBcc(member);
            }
        }
        


        public void AddAttachments(params MessageAttachment[] messageAttachments)
        {
            foreach (var attachment in messageAttachments)
            {
                if (MessageAttachments.All(x => x.ExternalId != attachment.ExternalId))
                {
                    MessageAttachments.Add(attachment);
                }
            }
        }

        

    }
}