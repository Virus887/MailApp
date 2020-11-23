using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailApp.Infrastructure.Mappings
{
    internal class GroupAccountMap : IEntityTypeConfiguration<GroupAccount>
    {
        public void Configure(EntityTypeBuilder<GroupAccount> builder)
        {
            builder.ToTable(nameof(GroupAccount));
            builder.HasKey(x => new {x.AccountId, x.GroupId, x.TypeId});

            builder.HasIndex(x => x.AccountId);
            builder.HasIndex(x => x.GroupId);
            builder.HasOne(x => x.Group).WithMany(x => x.GroupAccounts).HasForeignKey(x => x.GroupId);
            builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
        }
    }
}