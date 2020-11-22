using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailApp.Infrastructure.Mappings
{
    internal class GroupAccountTypeMap : IEntityTypeConfiguration<GroupAccountType>
    {
        public void Configure(EntityTypeBuilder<GroupAccountType> builder)
        {

            builder.ToTable(nameof(GroupAccountType));
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName($"{nameof(GroupAccountType)}Id");

            builder.HasData(GroupAccountType.Member, GroupAccountType.Owner);
        }
    }
}