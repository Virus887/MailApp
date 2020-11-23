using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailApp.Infrastructure.Mappings
{
    internal class MessagePersonTypeMap : IEntityTypeConfiguration<MessagePersonType>
    {
        public void Configure(EntityTypeBuilder<MessagePersonType> builder)
        {
            builder.ToTable(nameof(MessagePersonType));
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName($"{nameof(MessagePersonType)}Id");

            builder.HasData(MessagePersonType.Sender, MessagePersonType.Receiver, MessagePersonType.Cc, MessagePersonType.Bcc);
        }
    }
}