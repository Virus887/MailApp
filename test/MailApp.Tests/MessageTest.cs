using MailApp.Domain;
using MailApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MailApp.Tests
{
    public class MessageTest
    {
        [Fact]
        public void Ctor_Ok()
        {
            Message m = new Message();
        }

        [Fact]
        public void Ctor_IsRead_False()
        {
            Message m = new Message();
            Assert.False(m.IsRead);
        }

        [Fact]
        public void Ctor_Notification_False()
        {
            Message m = new Message();
            Assert.False(m.Notification);
        }
        [Fact]
        public void MarkAsUnRead_False()
        {
            Message m = new Message();
            m.IsRead = true;
            m.MarkAsUnRead();
            Assert.False(m.IsRead);

        }

        [Fact]
        public void AddNotification_True()
        {
            Message m = new Message();
            m.AddNotification();
            Assert.True(m.Notification);

        }
        [Fact]
        public void DeleteNotification_False()
        {
            Message m = new Message();
            m.DeleteNotification();
            Assert.False(m.Notification);

        }
        [Fact]
        public void AddReceiver_Receiver_Not_NULL()
        {
            Message m = new Message();
            Account a = new Account("nick1");
            m.AddReceiver(a);
            Assert.NotNull(m.Receiver);
        }
        [Theory]
        [InlineData(null)]
        public void AddReceiver_Throw_ArgumentNullException(Account a)
        {
            Message m = new Message();
            Assert.Throws<ArgumentNullException>(() => m.AddReceiver(a));
        }
        [Fact]
        public void AddGroup_Group_Not_NULL()
        {
            Message m = new Message();
            Group gr = new Group("group1");
            m.AddGroup(gr);
            Assert.NotNull(m.Group);
        }
        [Theory]
        [InlineData(null)]
        public void AddGroup_Throw_ArgumentNullException(Group a)
        {
            Message m = new Message();
            Assert.Throws<ArgumentNullException>(() => m.AddGroup(a));
        }
        [Fact]
        public void SendMessage_Ok()
        {
            Message message = new Message();
            message.SendMessage();

        }
        [Fact]
        //przedzial 2 sekund
        public void SendMessage_DateTime_Equals_Now()
        {
            Message message = new Message();
            message.SendMessage();
            Assert.InRange(message.Date, DateTime.Now - TimeSpan.FromSeconds(2), DateTime.Now);


        }
        [Fact(Skip = "Not implemented")]
        public void SendMessage_No_Receiver_Notyfication()
        {

        }

        [Fact(Skip = "Not implemented")]
        public void SendMessage_No_Subject_Notyfication()
        {

        }


    }
}
