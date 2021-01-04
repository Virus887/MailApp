using MailApp.Infrastructure;
using MailApp.Infrastructure.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailApp.Domain;
using Azure.Storage.Blobs;

namespace MailApp
{
    public class RetentionJob
    {
        private BlobContainerClient ContainerClient { get; }
        private MailAppDbContext MailAppDbContext { get; }

        public RetentionJob(MailAppDbContext mailAppDbContext, BlobContainerClient containerClient)
        {
            ContainerClient = containerClient;
            MailAppDbContext = mailAppDbContext;
        }

        public async Task RemoveOldAttachments()
        {
            var blobs = ContainerClient.GetBlobs()
                .Where(x => x.Properties.CreatedOn.HasValue && (DateTimeOffset.Now - x.Properties.CreatedOn.Value).Days >= 100)
                .ToArray();

            foreach (var blob in blobs)
            {
                var bl = ContainerClient.GetBlobClient(blob.Name);
                await bl.DeleteIfExistsAsync();

                var message = MailAppDbContext.Messages
                    .Include(x => x.MessageAttachments)
                    .FirstOrDefault(x => x.MessageAttachments.Any(y => y.ExternalId == blob.Name));

                if (message == null)
                {
                    return;
                }

                var attachment = message.MessageAttachments.SingleOrDefault(x => x.ExternalId == blob.Name);
                if (attachment == null)
                {
                    return;
                }

                message.MessageAttachments.Remove(attachment);
                await MailAppDbContext.SaveChangesAsync();
            }
        }
    }
}