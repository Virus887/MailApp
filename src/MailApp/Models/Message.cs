using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailApp.Models
{
    public class Message
    {
        public int Id { get; }
        public Account Sender { get; set; }
        public Account Receiver { get; set; }
        public Group Group { get; set; }
        public string Subject { get; set; }
        //nw czy ten format
        public string Text { get; set; }
        public DateTime Date { get; set; }
        //ustawiana na False, jedyny moment gdy sie tym przejmujemy to Odebrane w Box
        public bool IsRead { get; set; }
        public bool Notification { get; set; }





        //METHODS
        public Message()
        {
            //Id automatycznie ustawiany 
            //Sender na this-mysle zeby wiadomosc tworzylo sie przez skrzynke przypisana do Account

            IsRead = false;
            Notification = false;
        }

       
        public void MarkAsUnRead() => IsRead = false;
        public void AddNotification() => Notification = true;
        public void DeleteNotification() => Notification = false;


    }
}
