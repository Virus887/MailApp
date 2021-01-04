using System;
using MailApp.Domain;
using Xunit;

namespace MailApp.Tests
{
    public class GroupTests
    {
        private readonly Account owner = new Account("owner", "owner@owner.pl");

        [Fact]
        public void Ctor_Ok()
        {
            var group = new Group("A", owner);
            Assert.Equal("A", group.Name);
        }


        [Fact]
        public void Ctor_List_Not_Null()
        {
            var group = new Group("A", owner);
            Assert.NotNull(group.GroupAccounts);
        }

        [Theory]
        [InlineData("Name")]
        public void Ctor_Name_Equals_Given_String(string name)
        {
            var group = new Group(name, owner);
            Assert.Equal(name, group.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_EmptyName_Throw_ArgumentException(String name)
        {
            Assert.Throws<ArgumentNullException>(() => new Group(name, owner));
        }

        [Fact]
        public void AddAccount_Throw_ArgumentNullException()
        {
            var gr = new Group("A", owner);
            Assert.Throws<ArgumentNullException>(() => gr.AddAccount(default));
        }

        [Fact]
        public void AddAccount_Count2_Ok()
        {
            var gr = new Group("A", owner);

            var a1 = new Account("nick1", "a@a.pl");
            var a2 = new Account("nick2", "b@b.pl");

            gr.AddAccount(a1);
            gr.AddAccount(a1);
            gr.AddAccount(a2);

            Assert.Equal(3, gr.GroupAccounts.Count);
        }

        [Fact]
        public void RemoveAccount_Throw_ArgumentNullException()
        {
            var gr = new Group("A", owner);
            Assert.Throws<ArgumentNullException>(() => gr.RemoveAccount(default));
        }

        [Fact]
        public void RemoveAccount_Count2_Ok()
        {
            var gr = new Group("A", owner);
            var a1 = new Account("nick1", "a@a.pl");
            var a2 = new Account("nick2", "b@b.pl");
            var a3 = new Account("nick3", "c@c.pl");

            gr.AddAccount(a1);
            gr.AddAccount(a2);
            gr.AddAccount(a3);
            gr.RemoveAccount(a3);

            Assert.Equal(3, gr.GroupAccounts.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FindMemberByNick_EmptyNickThrow_ArgumentException(String nick)
        {
            var gr = new Group("A", owner);
            Assert.Throws<ArgumentNullException>(() => gr.FindMemberByNick(nick));
        }

        [Fact]
        public void FindMemberByNick_Exists_NotNull()
        {
            var gr = new Group("A", owner);
            var a1 = new Account("nick1", "a@a.pl");
            var a2 = new Account("nick2", "b@b.pl");
            var a3 = new Account("nick3", "c@c.pl");
            gr.AddAccount(a1);
            gr.AddAccount(a2);
            gr.AddAccount(a3);
            Assert.NotNull(gr.FindMemberByNick("nick1"));
        }

        [Fact]
        public void FindMemberByNick_NotExists_Null()
        {
            var gr = new Group("A", owner);
            var a1 = new Account("nick1", "a@a.pl");
            var a2 = new Account("nick2", "b@b.pl");
            var a3 = new Account("nick3", "c@c.pl");
            gr.AddAccount(a1);
            gr.AddAccount(a2);
            gr.AddAccount(a3);
            Assert.Null(gr.FindMemberByNick("nick4"));
        }

        [Fact]
        public void IsMember_True()
        {
            var gr = new Group("A", owner);
            var a1 = new Account("nick1", "a@a.pl");
            gr.AddAccount(a1);
            Assert.True(gr.IsMember(a1));
        }

        [Fact]
        public void IsMember_False()
        {
            var gr = new Group("A", owner);
            var a1 = new Account("nick1", "a@a.pl");
            Assert.False(gr.IsMember(a1));
        }

        [Fact]
        public void IsMember_Throws_ArgumentNullException()
        {
            var gr = new Group("A", owner);
            Assert.Throws<ArgumentNullException>(() => gr.IsMember(default));
        }

        [Fact]
        public void IsOwner_True()
        {
            var gr = new Group("A", owner);
            Assert.True(gr.IsOwner(owner));
        }

        [Fact]
        public void IsOwner_False()
        {
            var gr = new Group("A", owner);
            var a1 = new Account("nick1", "a@a.pl");
            Assert.False(gr.IsOwner(a1));
        }

        [Fact]
        public void IsOwner_Throws_ArgumentNullException()
        {
            var gr = new Group("A", owner);
            Assert.Throws<ArgumentNullException>(() => gr.IsOwner(default));
        }
    }
}