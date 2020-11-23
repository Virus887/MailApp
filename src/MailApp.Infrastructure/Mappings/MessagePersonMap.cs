using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailApp.Infrastructure.Mappings
{
    internal class MessagePersonMap : IEntityTypeConfiguration<MessagePerson>
    {
        public void Configure(EntityTypeBuilder<MessagePerson> builder)
        {
            builder.ToTable(nameof(MessagePerson));
            builder.HasKey(x => new { x.AccountId, x.MessageId, x.TypeId});

            builder.HasIndex(x => x.AccountId);
            builder.HasIndex(x => x.MessageId);
            builder.HasOne(x => x.Message).WithMany(x => x.MessagePersons).HasForeignKey(x => x.MessageId);
            builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
        }
    }
}