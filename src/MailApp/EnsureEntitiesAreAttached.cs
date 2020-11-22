using MailApp.Domain;
using MailApp.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MailApp
{
    /// <summary>
    /// Ten filtr pozwala oznaczyć encje jako dodane, żeby EF nie dodawał ich wielokrotnie do tabeli
    /// </summary>
    public class EnsureEntitiesAreAttached : IActionFilter
    {
        private MailAppDbContext MailAppDbContext { get; }

        public EnsureEntitiesAreAttached(MailAppDbContext mailAppDbContext)
        {
            MailAppDbContext = mailAppDbContext;
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            MailAppDbContext.Attach(GroupAccountType.Owner);
            MailAppDbContext.Attach(GroupAccountType.Member);
        }
    }
}