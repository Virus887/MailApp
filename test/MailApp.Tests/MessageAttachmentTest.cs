using MailApp.Domain;
using System;
using Xunit;

namespace MailApp.Tests
{
    public class MessageAttachmentTest
    {
        private const string ExternalId = "externalId";
        private const string Name = "name";
        private const string Type = "type";

        [Fact]
        public void Ctor_Ok()
        {
            var ma = new MessageAttachment(ExternalId, Name, Type);
        }

        [Fact]
        public void Ctor_ExternalIdSet_Ok()
        {
            var ma = new MessageAttachment(ExternalId, Name, Type);
            Assert.Equal(ExternalId, ma.ExternalId);
        }

        [Fact]
        public void Ctor_NameSet_Ok()
        {
            var ma = new MessageAttachment(ExternalId, Name, Type);
            Assert.Equal(Name, ma.Name);
        }

        [Fact]
        public void Ctor_TypeSet_Ok()
        {
            var ma = new MessageAttachment(ExternalId, Name, Type);
            Assert.Equal(Type, ma.Type);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_ExternalIdEmpty_Throw_ArgumentNullException(String s)
        {
            Assert.Throws<ArgumentNullException>(() => new MessageAttachment(s, Name, Type));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_NameEmpty_Throw_ArgumentNullException(String s)
        {
            Assert.Throws<ArgumentNullException>(() => new MessageAttachment(ExternalId, s, Type));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_TypeEmpty_Throw_ArgumentNullException(String s)
        {
            Assert.Throws<ArgumentNullException>(() => new MessageAttachment(ExternalId, Name, s));
        }
    }
}