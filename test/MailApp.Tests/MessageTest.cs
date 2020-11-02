using MailApp.Domain;
using System;
using Xunit;

namespace MailApp.Tests
{
    public class MessageTest
    {
        [Fact]
        public void Ctor_Ok()
        {
            var m = new Message();
        }

        [Fact]
        public void Ctor_IsRead_False()
        {
            var m = new Message();
            Assert.False(m.IsRead);
        }

        [Fact]
        public void Ctor_Notification_False()
        {
            var m = new Message();
            Assert.False(m.Notification);
        }

        [Fact]
        public void MarkAsUnRead_False()
        {
            var m = new Message();
            m.MarkAsUnRead();
            Assert.False(m.IsRead);
        }

        [Fact]
        public void AddNotification_True()
        {
            var m = new Message();
            m.AddNotification();
            Assert.True(m.Notification);
        }

        [Fact]
        public void DeleteNotification_False()
        {
            var m = new Message();
            m.DeleteNotification();
            Assert.False(m.Notification);
        }

        [Fact]
        public void AddReceiver_Receiver_Not_NULL()
        {
            var m = new Message();
            var a = new Account("nick1");
            m.AddReceiver(a);
            Assert.NotNull(m.Receiver);
        }

        [Theory]
        [InlineData(null)]
        public void AddReceiver_Throw_ArgumentNullException(Account a)
        {
            var m = new Message();
            Assert.Throws<ArgumentNullException>(() => m.AddReceiver(a));
        }

        [Fact]
        public void AddGroup_Group_Not_NULL()
        {
            var m = new Message();
            var gr = new Group("group1");
            m.AddGroup(gr);
            Assert.NotNull(m.Group);
        }

        [Theory]
        [InlineData(null)]
        public void AddGroup_Throw_ArgumentNullException(Group a)
        {
            var m = new Message();
            Assert.Throws<ArgumentNullException>(() => m.AddGroup(a));
        }

        [Fact]
        public void SendMessage_Ok()
        {
            var message = new Message();
            message.SendMessage();
        }

        [Fact]
        //przedzial 2 sekund
        public void SendMessage_DateTime_Equals_Now()
        {
            var message = new Message();
            message.SendMessage();
            Assert.InRange(message.SentDate, DateTime.Now - TimeSpan.FromSeconds(2), DateTime.Now);
        }

        [Fact(Skip = "Not implemented")]
        public void SendMessage_No_Receiver_Notification()
        {
        }

        [Fact(Skip = "Not implemented")]
        public void SendMessage_No_Subject_Notification()
        {
        }
    }
}