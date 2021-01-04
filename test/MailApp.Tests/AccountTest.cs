using MailApp.Domain;
using System;
using System.Linq;
using Xunit;

namespace MailApp.Tests
{
    public class AccountTests
    {
        private const string Nick = "nick1";
        private const string Email = "email@email.pl";

        [Fact]
        public void Ctor_Ok()
        {
            var account = new Account(Nick, Email);
        }

        [Fact]
        public void Email_Ok()
        {
            var account = new Account(Nick, Email);
            Assert.Equal("email@email.pl", Email);
        }

        [Fact]
        public void Nick_Ok()
        {
            var account = new Account(Nick, Email);
            Assert.Equal("nick1", Nick);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_EmptyNick_Throw_ArgumentException(String nick)
        {
            Assert.Throws<ArgumentNullException>(() => new Account(nick, Email));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_EmptyEmail_Throw_ArgumentException(String email)
        {
            Assert.Throws<ArgumentNullException>(() => new Account(Nick, email));
        }

        [Fact]
        public void Ctor_Throw_Too_Long_Exception()
        {
            var s = String.Join("", Enumerable.Repeat("a", Account.MaxNickLength + 1));
            Assert.Throws<ArgumentException>(() => new Account(s, Email));
        }
    }
}