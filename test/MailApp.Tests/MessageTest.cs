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
        public void Ctor_Notification_False()
        {
            var m = new Message();
            Assert.False(m.Notification);
        }

        [Fact]
        public void DeleteNotification_False()
        {
            var m = new Message();
            m.DeleteNotification();
            Assert.False(m.Notification);
        }

        [Fact]
        public void SetSender_Once()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");
            m.SetSender(a);
            Assert.Equal(a, m.Sender);
        }

        [Fact]
        public void SetSender_Twice()
        {
            var m = new Message();
            var a1 = new Account("nick1", "a@a.pl");
            var a2 = new Account("nick2", "b@a.pl");
            m.SetSender(a1);
            m.SetSender(a2);
            Assert.Equal(a2, m.Sender);
        }

        [Theory]
        [InlineData(null)]
        public void SetSender_Throw_ArgumentNullException(Account a)
        {
            var m = new Message();
            Assert.Throws<ArgumentNullException>(() => m.SetSender(a));
        }


        [Fact]
        public void AddReceiver_Receiver_Not_Null()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");
            m.AddReceiver(a);
            Assert.NotEmpty(m.Receivers);
        }

        [Fact]
        public void AddReceiver_Receiver_Twice()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");
            m.AddReceiver(a);
            m.AddReceiver(a);
            Assert.Single(m.Receivers);
        }

        [Theory]
        [InlineData(null)]
        public void AddReceiver_Throw_ArgumentNullException(Account a)
        {
            var m = new Message();
            Assert.Throws<ArgumentNullException>(() => m.AddReceiver(a));
        }

        [Fact]
        public void AddReceiver__Group_Not_Null()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            m.AddReceiver(gr);
            Assert.NotEmpty(m.Receivers);
        }


        [Theory]
        [InlineData(null)]
        public void AddReceiver_Group_Throw_ArgumentNullException(Group a)
        {
            var m = new Message();
            Assert.Throws<ArgumentNullException>(() => m.AddReceiver(a));
        }

        [Fact]
        public void AddCc_Accout_Not_Null()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");
            m.AddCc(a);
            Assert.NotEmpty(m.Cc);
        }

        [Fact]
        public void AddCc_Accout_Twice()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");
            m.AddCc(a);
            m.AddCc(a);
            Assert.Single(m.Cc);
        }


        [Fact]
        public void AddBcc_Account_Not_Null()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");
            m.AddBcc(a);
            Assert.NotEmpty(m.Bcc);
        }

        [Fact]
        public void AddBcc_Account_Twice()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");
            m.AddBcc(a);
            m.AddBcc(a);
            Assert.Single(m.Bcc);
        }

        [Theory]
        [InlineData(null)]
        public void AddBcc_Account_Throw_ArgumentNullException(Account a)
        {
            var m = new Message();
            Assert.Throws<ArgumentNullException>(() => m.AddBcc(a));
        }

        [Fact]
        public void AddBcc_Group_Not_Null()
        {
            var m = new Message();
            var a = new Account("nick1", "a@a.pl");

            var gr = new Group("group1", a);
            m.AddBcc(gr);
            Assert.NotEmpty(m.Bcc);
        }
    }
}