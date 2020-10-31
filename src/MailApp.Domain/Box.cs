using System;
using System.Collections.Generic;

namespace MailApp.Domain
{
    public class Box
    {
        public List<Message> Messages { get; set; }

        //Methods
        public Box()
        {
            Messages = new List<Message>();
        }
        public Message CreateMessage() => new Message();

        public void SendMessage(Message message)
        {
            if (message.Group != null)
            {
                foreach (var r in message.Group.Accounts)
                {

                    if (message.Subject == null)
                    {
                        //komunikat czy na pewno chcesz wyslac pusta wiadomosc
                    }
                    if (message.Notification)
                    {
                        //wysylanie notyfikacji
                    }
                }
            }


            if (message.Receiver == null)
            {
                //brak odbiorcy
            }
            if (message.Subject == null)
            {
                //komunikat czy na pewno chcesz wyslac pusta wiadomosc
            }
            if (message.Notification)
            {
                //wysylanie notyfikacji
            }
            message.Date = DateTime.Now;
        }

        public void ReadMessage(Message message)
        {
            message.IsRead = true;
            //openMessage
        }


    }
}
