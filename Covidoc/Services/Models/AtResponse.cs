using Newtonsoft.Json;

namespace CoviDoc.Services.Models
{
    public partial class AtResponse
    {
        [JsonProperty("SMSMessageData")]
        public SmsMessageData SmsMessageData { get; set; }
    }
    public partial class Recipient
    {
        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("cost")]
        public string Cost { get; set; }

        [JsonProperty("messageId")]
        public string MessageId { get; set; }
    }
    public partial class SmsMessageData
    {
        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Recipients")]
        public Recipient[] Recipients { get; set; }
    }
}