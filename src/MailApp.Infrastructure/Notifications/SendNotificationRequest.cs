using Newtonsoft.Json;


namespace MailApp.Infrastructure.Notifications
{
    public class SendNotificationRequest
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("recipientsList")]
        public string[] RecipientsList { get; set; }

        [JsonProperty("withAttachments")]
        public bool WithAttachments { get; set; }
    }
}

