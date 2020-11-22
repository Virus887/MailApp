using System.Threading.Tasks;
using MailApp.Domain;
using MailApp.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MailApp
{
    /// <summary>
    /// Zadaniem tego filtra jest upewnienie się, że Account dla danego użytkownika wykonujacego żądanie jest już stworzony
    /// </summary>
    public class EnsureAccountActionFilter : IAsyncActionFilter
    {
        private MailAppDbContext MailAppDbContext { get; }

        public EnsureAccountActionFilter(MailAppDbContext mailAppDbContext)
        {
            MailAppDbContext = mailAppDbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cancellationToken = context.HttpContext.RequestAborted;
            var email = context.HttpContext.User.FindFirst("emails");
            var account = await MailAppDbContext.Accounts.SingleOrDefaultAsync(x => x.Email == email.Value, cancellationToken);
            if (account == null)
            {
                var nick = context.HttpContext.User.FindFirst("name");
                account = new Account(nick.Value, email.Value);
                MailAppDbContext.Add(account);
                await MailAppDbContext.SaveChangesAsync(cancellationToken);
            }

            await next();
        }
    }
}