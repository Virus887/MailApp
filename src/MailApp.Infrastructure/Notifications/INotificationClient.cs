using RestEase;
using System.Threading.Tasks;

namespace MailApp.Infrastructure.Notifications
{
    public interface INotificationClient
    {
        [Post("/notifications")]
        public Task SendNotification([Body] SendNotificationRequest request);
    }
}