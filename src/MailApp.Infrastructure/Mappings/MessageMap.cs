using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailApp.Infrastructure.Mappings
{
    public class MessageMap : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable(nameof(Message));
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName($"{nameof(Message)}Id");

            builder.HasOne(x => x.Sender).WithMany();
            builder.HasOne(x => x.Receiver).WithMany();
            builder.HasOne(x => x.Group);
        }
    }
}