using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailApp.Domain;
using MailApp.Infrastructure;
using MailApp.Models.Groups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace MailApp.Controllers
{
    [Route("/Groups")]
    public class GroupsController : Controller
    {
        private MailAppDbContext MailAppDbContext { get; }
        private IAccountProvider AccountProvider { get; }

        public GroupsController(MailAppDbContext mailAppDbContext, IAccountProvider accountProvider)
        {
            MailAppDbContext = mailAppDbContext;
            AccountProvider = accountProvider;
        }

        private Task<Group> GetGroup(Int32 groupId, CancellationToken cancellationToken) =>
            MailAppDbContext.Groups
                .Include(x => x.GroupAccounts)
                .ThenInclude(x => x.Account)
                .SingleOrDefaultAsync(x => x.Id == groupId, cancellationToken);

        [HttpGet]
        [SwaggerOperation(Summary = "List all of the groups",
            Description = "Returns view with global list of existing groups.")]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var account = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var groups = await MailAppDbContext.Groups
                .Include(x => x.GroupAccounts).ThenInclude(x => x.Type)
                .Include(x => x.GroupAccounts).ThenInclude(x => x.Account)
                .ToArrayAsync(cancellationToken);

            var viewModel = new GroupsListViewModel
            {
                Groups = groups
                    .Where(x => x.IsMember(account))
                    .Select(x => new GroupViewModel
                    {
                        GroupId = x.Id,
                        Name = x.Name
                    })
                    .ToArray(),
            };
            return View(viewModel);
        }

        [HttpGet("{groupId}")]
        [SwaggerOperation(Summary = "Get information about group",
            Description = "Returns view with information about group with specified Id.")]
        public async Task<IActionResult> Details(int groupId, CancellationToken cancellationToken)
        {
            var account = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var group = await GetGroup(groupId, cancellationToken);
            if (group == null)
            {
                return NotFound();
            }

            if (!group.IsMember(account))
            {
                return Forbid();
            }

            var viewModel = new GroupDetailsViewModel
            {
                GroupId = group.Id,
                Name = group.Name,
                Owner = new AccountViewModel(group.Owner),
                Members = group.GroupAccounts
                    .Select(x => new AccountViewModel(x.Account))
                    .ToArray(),
            };
            return View(viewModel);
        }

        [HttpGet("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult AddGroup() => View();

        [HttpPost("[action]")]
        [SwaggerOperation(Summary = "Create new group", Description = "Creates new group with given name and returns default group list view.")]
        public async Task<IActionResult> AddGroup(AddGroupViewModel viewModel, CancellationToken cancellationToken)
        {
            var account = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var group = new Group(viewModel.Name, account);
            MailAppDbContext.Groups.Add(group);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Details), new {groupId = group.Id});
        }

        [HttpGet("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RemoveGroup(int groupId, CancellationToken cancellationToken)
        {
            var group = await GetGroup(groupId, cancellationToken);
            if (group == null)
            {
                return NotFound();
            }

            return View(new RemoveGroupViewModel
            {
                GroupId = group.Id,
                Name = group.Name
            });
        }

        [HttpPost("[action]")]
        [SwaggerOperation(Summary = "Remove group", Description = "Removes group with specified Id from database and returns default group list view.")]
        public async Task<IActionResult> RemoveGroup(RemoveGroupViewModel viewModel, CancellationToken cancellationToken)
        {
            var account = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var group = await GetGroup(viewModel.GroupId, cancellationToken);
            if (group == null)
            {
                return BadRequest();
            }

            if (!group.IsOwner(account))
            {
                return Forbid();
            }

            MailAppDbContext.Remove(group);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(List));
        }

        [HttpGet("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult AddMember(int groupId) =>
            View(new AddMemberViewModel
            {
                GroupId = groupId,
            });

        [HttpPost("[action]")]
        [SwaggerOperation(Summary = "Add new member to group", Description = "Adds account with specified Id to already existing group and returns default groups view.")]
        public async Task<IActionResult> AddMember(AddMemberViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var account = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var group = await GetGroup(viewModel.GroupId, cancellationToken);

            if (group == null)
            {
                ModelState.AddModelError(nameof(viewModel.GroupId), "Group does not exist");
                return View(viewModel);
            }

            if (!group.IsOwner(account))
            {
                ModelState.AddModelError(String.Empty, "Only group admin can delete it's members.");
                return View(viewModel);
            }


            var newMember = await MailAppDbContext.Accounts.SingleOrDefaultAsync(x => x.Nick == viewModel.AccountNick, cancellationToken);

            if (newMember == null)
            {
                ModelState.AddModelError(nameof(viewModel.AccountNick), "There is no Account with such nick.");
                return View(viewModel);
            }

            group.AddAccount(newMember);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Details), new {groupId = viewModel.GroupId});
        }

        [HttpGet("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult RemoveMember(int groupId) =>
            View(new RemoveMemberViewModel
            {
                GroupId = groupId,
            });

        [HttpPost("[action]")]
        [SwaggerOperation(Summary = "Remove group member", Description = "Removes account with specified Id from already existing group and returns default groups view.")]
        public async Task<IActionResult> RemoveMember(RemoveMemberViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var account = await AccountProvider.GetAccountForCurrentUser(cancellationToken);
            var group = await GetGroup(viewModel.GroupId, cancellationToken);

            if (group == null)
            {
                ModelState.AddModelError(nameof(viewModel.GroupId), "Group does not exist");
                return View(viewModel);
            }

            if (!group.IsOwner(account))
            {
                ModelState.AddModelError(String.Empty, "Only admin can add members.");
                return View(viewModel);
            }

            var member = await MailAppDbContext.Accounts.SingleOrDefaultAsync(x => x.Nick == viewModel.AccountNick, cancellationToken);

            if (member == null)
            {
                ModelState.AddModelError(nameof(viewModel.AccountNick), "There is no Account with such nick.");
                return View(viewModel);
            }

            group.RemoveAccount(member);
            await MailAppDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Details), new {groupId = viewModel.GroupId});
        }
    }
}