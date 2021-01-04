using MailApp.Domain;
using System;
using Xunit;

namespace MailApp.Tests
{
    public class MessagePersonTypeTest
    {
        [Fact]
        public void Sender_Ok()
        {
            Assert.Equal(1, MessagePersonType.Sender.Id);
        }
        [Fact]
        public void Receiver_Ok()
        {
            Assert.Equal(2, MessagePersonType.Receiver.Id);
        }
        [Fact]
        public void Cc_Ok()
        {
            Assert.Equal(3, MessagePersonType.Cc.Id);
        }
        [Fact]
        public void Bcc_Ok()
        {
            Assert.Equal(4, MessagePersonType.Bcc.Id);
        }
    }
}
