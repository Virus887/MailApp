using MailApp.Domain;
using System;
using Xunit;

namespace MailApp.Tests
{
    public class GroupAccountTest
    {
        [Fact]
        public void Ctor_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            var ga = new GroupAccount(a, gr, GroupAccountType.Member);
        }

        [Fact]
        public void Ctor_AccountSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            var ga = new GroupAccount(a, gr, GroupAccountType.Member);
            Assert.Equal(a, ga.Account);
        }

        [Fact]
        public void Ctor_AccountIdSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            var ga = new GroupAccount(a, gr, GroupAccountType.Member);
            Assert.Equal(a.Id, ga.AccountId);
        }

        [Fact]
        public void Ctor_GroupSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            var ga = new GroupAccount(a, gr, GroupAccountType.Member);
            Assert.Equal(gr, ga.Group);
        }

        [Fact]
        public void Ctor_GroupIdSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            var ga = new GroupAccount(a, gr, GroupAccountType.Member);
            Assert.Equal(gr.Id, ga.GroupId);
        }

        [Fact]
        public void Ctor_TypeSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            var ga = new GroupAccount(a, gr, GroupAccountType.Member);
            Assert.Equal(GroupAccountType.Member, ga.Type);
        }

        [Fact]
        public void Ctor_TypeIdSet_Ok()
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            var ga = new GroupAccount(a, gr, GroupAccountType.Member);
            Assert.Equal(GroupAccountType.Member.Id, ga.TypeId);
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_AccountEmpty_Throw_ArgumentNullException(Account a1)
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            Assert.Throws<ArgumentNullException>(() => new GroupAccount(a1, gr, GroupAccountType.Member));
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_GroupEmpty_Throw_ArgumentNullException(Group gr)
        {
            var a = new Account("nick1", "a@a.pl");
            Assert.Throws<ArgumentNullException>(() => new GroupAccount(a, gr, GroupAccountType.Member));
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_GroupAccountType_Throw_ArgumentNullException(GroupAccountType type)
        {
            var a = new Account("nick1", "a@a.pl");
            var gr = new Group("group1", a);
            Assert.Throws<ArgumentNullException>(() => new GroupAccount(a, gr, type));
        }
    }
}