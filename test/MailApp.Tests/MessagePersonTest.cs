using MailApp.Domain;
using System;
using Xunit;

namespace MailApp.Tests
{
    public class MessagePersonTest
    {
        [Fact]
        public void Ctor_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var m = new Message();
            var mp = new MessagePerson(a, m, MessagePersonType.Bcc);
        }

        [Fact]
        public void Ctor_AccountSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var m = new Message();
            var mp = new MessagePerson(a, m, MessagePersonType.Bcc);
            Assert.Equal(a, mp.Account);
        }

        [Fact]
        public void Ctor_AccountIdSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var m = new Message();
            var mp = new MessagePerson(a, m, MessagePersonType.Bcc);
            Assert.Equal(a.Id, mp.AccountId);
        }

        [Fact]
        public void Ctor_MessageSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var m = new Message();
            var mp = new MessagePerson(a, m, MessagePersonType.Bcc);
            Assert.Equal(m, mp.Message);
        }

        [Fact]
        public void Ctor_MessageIdSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var m = new Message();
            var mp = new MessagePerson(a, m, MessagePersonType.Bcc);
            Assert.Equal(m.Id, mp.MessageId);
        }

        [Fact]
        public void Ctor_TypeSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var m = new Message();
            var mp = new MessagePerson(a, m, MessagePersonType.Bcc);
            Assert.Equal(MessagePersonType.Bcc, mp.Type);
        }

        [Fact]
        public void Ctor_TypeIdSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var m = new Message();
            var mp = new MessagePerson(a, m, MessagePersonType.Bcc);
            Assert.Equal(MessagePersonType.Bcc.Id, mp.TypeId);
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_AccountEmpty_Throw_ArgumentNullException(Account a)
        {
            var m = new Message();
            Assert.Throws<ArgumentNullException>(() => new MessagePerson(a, m, MessagePersonType.Bcc));
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_MessageEmpty_Throw_ArgumentNullException(Message m)
        {
            var a = new Account("nick1", "a@a.pl");
            Assert.Throws<ArgumentNullException>(() => new MessagePerson(a, m, MessagePersonType.Bcc));
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_MessagePersonType_Throw_ArgumentNullException(MessagePersonType type)
        {
            var a = new Account("nick1", "a@a.pl");
            var m = new Message();
            Assert.Throws<ArgumentNullException>(() => new MessagePerson(a, m, type));
        }
    }
}