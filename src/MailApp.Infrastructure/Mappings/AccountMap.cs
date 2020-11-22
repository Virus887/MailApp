using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailApp.Infrastructure.Mappings
{
    internal class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(nameof(Account));
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName($"{nameof(Account)}Id");

            builder.HasIndex(x => x.Nick).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}