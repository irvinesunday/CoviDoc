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
        }

        public SmsMessageOptions(string userName, Uri apiHost, string keyWord) : this()
        {
            this.UserName = !string.IsNullOrWhiteSpace(userName) ? userName : throw new ArgumentNullException(nameof(userName));
            this.Keyword = !string.IsNullOrWhiteSpace(keyWord) ? keyWord : throw new ArgumentNullException(nameof(keyWord));
            this.ApiHost = apiHost ?? throw new ArgumentNullException(nameof(apiHost));
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