using System;
using MailApp.Models;
using Xunit;

namespace MailApp.Tests
{
    public class GroupTests
    {
        [Fact]
        public void Ctor_Ok()
        {
            var group = new Group("A");
            Assert.Equal("A", group.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_EmptyName_Throw_ArgumentException(String name)
        {
            Assert.Throws<ArgumentNullException>(() => new Group(name));
        }
    }
}
