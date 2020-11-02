using MailApp.Domain;
using MailApp.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MailApp.Infrastructure
{
    public class MailAppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }

        public MailAppDbContext(DbContextOptions<MailAppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MailAppDbContext).Assembly);
        }
    }
}