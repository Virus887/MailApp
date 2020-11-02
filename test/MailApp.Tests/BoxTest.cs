using MailApp.Domain;
using System;
using Xunit;

namespace MailApp.Tests
{
    public class BoxTest
    {
        [Fact]
        public void Ctor_Ok()
        {
            var box = new Box();
        }

        [Fact]
        public void Ctor_List_Not_NULL()
        {
            var box = new Box();
            Assert.NotNull(box.Messages);
        }

        [Fact]
        public void ReadMessage_OK()
        {
            var box = new Box();
            var message = new Message();
            box.ReadMessage(message);
            Assert.True(message.IsRead);
        }

        [Theory]
        [InlineData(null)]
        public void ReadMessage_EmptyName_Throw_ArgumentException(Message message)
        {
            var box = new Box();
            Assert.Throws<ArgumentNullException>(() => box.ReadMessage(message));
        }

        [Fact]
        public void CreateMessage_OK()
        {
            var box = new Box();
            Assert.IsType<Message>(box.CreateMessage());
        }
    }
}