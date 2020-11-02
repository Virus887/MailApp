using System;
using MailApp.Domain;
using System.Linq;
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

        [Fact]
        public void Ctor_List_Not_NULL()
        {
            var group = new Group("A");
            Assert.NotNull(group.Accounts);
        }

        [Theory]
        [InlineData("Name")]
        public void Ctor_Name_Equals_Given_String(string name)
        {
            var group = new Group(name);
            Assert.Equal(name, group.Name);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_EmptyName_Throw_ArgumentException(String name)
        {
            Assert.Throws<ArgumentNullException>(() => new Group(name));
        }

        [Theory]
        [InlineData(null)]
        public void AddAccount_Throw_ArgumentNullException(Account account)
        {
            var gr = new Group("A");
            Assert.Throws<ArgumentNullException>(() => gr.AddAccount(account));
        }

        [Fact]
        public void AddAcount_Count2_Ok()
        {
            var a1 = new Account("nick1");
            var a2 = new Account("nick2");
            var gr = new Group("A");
            gr.AddAccount(a1);
            gr.AddAccount(a1);
            gr.AddAccount(a2);
            Assert.Equal(2, gr.Accounts.Count());
        }

        [Theory]
        [InlineData(null)]
        public void removeAccount_Throw_ArgumentNullException(Account account)
        {
            var gr = new Group("A");
            Assert.Throws<ArgumentNullException>(() => gr.RemoveAccount(account));
        }

        [Fact]
        public void RemoveAcount_Count2_Ok()
        {
            var gr = new Group("A");
            var a1 = new Account("nick1");
            var a2 = new Account("nick2");
            var a3 = new Account("nick3");
            //dodaje 3 i usuwam 1
            gr.AddAccount(a1);
            gr.AddAccount(a2);
            gr.AddAccount(a3);
            gr.RemoveAccount(a2);
            Assert.Equal(2, gr.Accounts.Count());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FindMemberByNick_EmptyNickThrow_ArgumentException(String nick)
        {
            var gr = new Group("A");
            Assert.Throws<ArgumentNullException>(() => gr.FindMemberByNick(nick));
        }

        [Fact]
        public void FindMemberByNick_Exists_NotNull()
        {
            var gr = new Group("A");
            var a1 = new Account("nick1");
            var a2 = new Account("nick2");
            var a3 = new Account("nick3");
            gr.AddAccount(a1);
            gr.AddAccount(a2);
            gr.AddAccount(a3);
            Assert.NotNull(gr.FindMemberByNick("nick1"));
        }
        [Fact]
        public void FindMemberByNick_NotExists_Null()
        {
            var gr = new Group("A");
            var a1 = new Account("nick1");
            var a2 = new Account("nick2");
            var a3 = new Account("nick3");
            gr.AddAccount(a1);
            gr.AddAccount(a2);
            gr.AddAccount(a3);
            Assert.Null(gr.FindMemberByNick("nick4"));
        }
    }

}
