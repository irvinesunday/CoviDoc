using System;

namespace CoviDoc.Services
{
    public class SmsMessageOptions
    {
        public SmsMessageOptions()
        {
            BulkSmsMode = 1;
            Enqueue = 1;
            RetryDurationInHours = 1;
            LinkId = string.Empty;
            Keyword = string.Empty;
        }

        public SmsMessageOptions(string userName, Uri apiHost) : this()
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            if (apiHost == null)
            {
                throw new ArgumentNullException(nameof(apiHost));
            }
            this.UserName = userName;
            this.ApiHost = apiHost;
        }

        public int BulkSmsMode { get; }
        public string Keyword { get; }
        public string LinkId { get; }
        public int Enqueue { get; }
        public int RetryDurationInHours { get; }
        public string UserName { get; }
        public Uri ApiHost { get; set; }
    }
}