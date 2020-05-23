using System;

namespace CoviDoc.Controllers.Notifications.Model
{
    public class IncomingMessageNotification
    {
        public DateTimeOffset Date { get; set; }
        public string From { get; set; }
        public string Id { get; set; }
        public string LinkId { get; set; }
        public string Text { get; set; }
        public string To { get; set; }
        public string NetworkCode { get; set; }
    }
}