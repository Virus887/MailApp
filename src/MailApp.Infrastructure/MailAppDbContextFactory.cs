using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MailApp.Infrastructure
{
    public class MailAppDbContextFactory : IDesignTimeDbContextFactory<MailAppDbContext>
    {
        public MailAppDbContext CreateDbContext(String[] args)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<MailAppDbContext>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MailApp;Integrated Security=True;");
            return new MailAppDbContext(dbContextOptionsBuilder.Options);
        }
    }
}