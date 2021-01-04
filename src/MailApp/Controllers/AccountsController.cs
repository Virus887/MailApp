using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailApp.Infrastructure;
using MailApp.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MailApp.Controllers
{
    public class AccountsController : Controller
    {
        const int AdminId = 1;
        private MailAppDbContext MailAppDbContext { get; }
        private IAccountProvider AccountProvider { get; }
        public AccountsController(MailAppDbContext mailAppDbContext, IAccountProvider accountProvider)
        {
            MailAppDbContext = mailAppDbContext;
            AccountProvider = accountProvider;
        }

        public async Task<IActionResult> Index(AccountsQuery query, CancellationToken cancellationToken)
        {
            var currentAccount = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            if(currentAccount.Id != AdminId)
            {
                return Forbid();
            }

            var queryable = MailAppDbContext.Accounts
                .Where(x => String.IsNullOrEmpty(query.Nick) || x.Nick.Contains(query.Nick))
                .Where(x => String.IsNullOrEmpty(query.Email) || x.Email.Contains(query.Email));

            queryable = query.Sort switch
            {
                AccountsQuery.SortingOptions.Email => queryable.OrderBy(x => x.Email),
                AccountsQuery.SortingOptions.Nick => queryable.OrderBy(x => x.Nick),
                _ => queryable
            };

            var accounts = await queryable
                .Select(x => new AccountViewModel(x))
                .ToArrayAsync(cancellationToken);

            var viewModel = new AccountsListViewModel
            {
                Email = query.Email,
                Nick = query.Nick,
                Accounts = accounts
            };
            return View("List", viewModel);
        }

    }
}