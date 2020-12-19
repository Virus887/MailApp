using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailApp.Infrastructure.Mappings
{
    internal class MessageAttachmentMap : IEntityTypeConfiguration<MessageAttachment>
    {
        public void Configure(EntityTypeBuilder<MessageAttachment> builder)
        {
            builder.ToTable(nameof(MessageAttachment));

            builder.Property<int>("MessageId").IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Type).IsRequired();

            builder.HasKey("MessageId", nameof(MessageAttachment.ExternalId));
        }
    }
}