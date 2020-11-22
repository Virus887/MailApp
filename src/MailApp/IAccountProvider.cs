using System.Threading;
using System.Threading.Tasks;
using MailApp.Domain;

namespace MailApp
{
    public interface IAccountProvider
    {
        Task<Account> GetAccountForCurrentUser(CancellationToken cancellationToken);
    }
}