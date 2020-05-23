namespace CoviDoc.Controllers.Notifications.Model
{
    public class SmsDeliveryReportNotification
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string PhoneNumber { get; set; }
        public string NetworkCode { get; set; }
        public string FailureReason { get; set; }
        public string RetryCount { get; set; }
    }
}