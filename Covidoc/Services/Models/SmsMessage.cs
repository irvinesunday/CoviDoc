using System;

namespace CoviDoc.Services.Models
{
    public class SmsMessage
    {
        public SmsMessage(string to, string message)
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentNullException(nameof(to));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            this.To = to;
            this.Message = message;
        }
        public string To { get; }
        public string Message { get; }
    }
}