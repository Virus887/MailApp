using MailApp.Domain;
using MailApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MailApp.Tests
{
    public class AccountTests
    {
        [Fact]
        public void Ctor_Ok()
        {
            var account = new Account("nick1");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_EmptyName_Throw_ArgumentException(String name)
        {
            Assert.Throws<ArgumentNullException>(() => new Account(name));
        }
        [Fact]
        public void Ctor_EmptyName_Throw_Too_Long_Exception()
        {
            var s = string.Join("", Enumerable.Repeat("a", Account.MaxNickLength + 1));
            Assert.Throws<ArgumentException>(() => new Account(s));
        }
    }
}
