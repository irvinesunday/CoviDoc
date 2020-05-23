namespace CoviDoc.Controllers.Notifications.Model
{
    public class SubscriptionNotification
    {
        public string PhoneNumber { get; set; }
        public string ShortCode { get; set; }
        public string Keyword { get; set; }
        public string UpdateType { get; set; }
    }
}