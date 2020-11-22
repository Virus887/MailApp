using System.Threading;
using System.Threading.Tasks;
using MailApp.Domain;
using MailApp.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MailApp
{
    internal class AccountProvider : IAccountProvider
    {
        private MailAppDbContext MailAppDbContext { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public AccountProvider(MailAppDbContext mailAppDbContext, IHttpContextAccessor httpContextAccessor)
        {
            MailAppDbContext = mailAppDbContext;
            HttpContextAccessor = httpContextAccessor;
        }

        public Task<Account> GetAccountForCurrentUser(CancellationToken cancellationToken)
        {
            var currentUserEmail = HttpContextAccessor.HttpContext.User.FindFirst("emails");
            return MailAppDbContext.Accounts.SingleAsync(x => x.Email == currentUserEmail.Value, cancellationToken);
        }
    }
}