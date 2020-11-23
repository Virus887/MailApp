using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailApp.Infrastructure.Mappings
{
    internal class MessageMap : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable(nameof(Message));
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName($"{nameof(Message)}Id");

            builder.Ignore(x => x.Sender);
            builder.Ignore(x => x.Receivers);
            builder.Ignore(x => x.Cc);
            builder.Ignore(x => x.Bcc);
        }
    }
}