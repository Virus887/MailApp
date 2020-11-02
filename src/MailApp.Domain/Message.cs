using System;

namespace MailApp.Domain
{
    public class Message: Entity<int>
    {
        public Account Sender { get; }
        public Account Receiver { get; private set; }
        public Group Group { get; private set; }
        public string Subject { get; set; }

        //nw czy ten format
        public string Text { get; set; }

        public DateTime SentDate { get; set; }

        /// <summary>
        /// Ustawiana na False, jedyny moment gdy sie tym przejmujemy to Odebrane w Box
        /// </summary>
        public bool IsRead { get; private set; }

        public bool Notification { get; private set; }

        public Message()
        {
            //Id automatycznie ustawiany 
            //Sender na this-mysle zeby wiadomosc tworzylo sie przez skrzynke przypisana do Account

            IsRead = false;
            Notification = false;
        }


        public void MarkAsRead() => IsRead = true;
        public void MarkAsUnRead() => IsRead = false;
        public void AddNotification() => Notification = true;
        public void DeleteNotification() => Notification = false;
        
        public void AddReceiver(Account receiver)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }
            this.Receiver = receiver;
        }

        public void AddGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            // trzeba jeszcze sprawdzic czy taka grupa istnieje
            this.Group = group;
        }
        
        public void SendMessage()
        {
            if (this.Group != null)
            {
                foreach (var r in this.Group.Accounts)
                {

                    if (this.Subject == null)
                    {
                        //komunikat czy na pewno chcesz wyslac pusta wiadomosc
                    }
                    if (this.Notification)
                    {
                        //wysylanie notyfikacji
                    }
                }
            }


            if (this.Receiver == null)
            {
                //brak odbiorcy
            }
            if (String.IsNullOrEmpty(this.Subject))
            {
                //komunikat czy na pewno chcesz wyslac pusta wiadomosc
            }
            if (this.Notification)
            {
                //wysylanie notyfikacji
            }
            this.SentDate = DateTime.Now;
        }
    }
}