using System.Threading;
using System.Threading.Tasks;
using MailApp.Domain;

namespace MailApp
{
    internal class CacheAccountProvider : IAccountProvider
    {
        private IAccountProvider Inner { get; }
        private Account Account { get; set; }
        private SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1);

        public CacheAccountProvider(IAccountProvider inner)
        {
            Inner = inner;
        }

        public async Task<Account> GetAccountForCurrentUser(CancellationToken cancellationToken)
        {
            await Semaphore.WaitAsync(cancellationToken);
            if (Account == null)
            {
                Account = await Inner.GetAccountForCurrentUser(cancellationToken);
            }

            Semaphore.Release();
            return Account;
        }
    }
}