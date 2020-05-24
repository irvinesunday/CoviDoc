using System;
using System.Collections.Generic;
using System.Net.Http;
using CoviDoc.Services;
using CoviDoc.Services.Models;

namespace CoviDoc.Extensions
{
    public static class ObjectExtensions
    {
        public static FormUrlEncodedContent ToFormUrlEncodedContent(this SmsMessage message, SmsMessageOptions options)
        {
            var mergedDictionary = new Dictionary<string, string>();
            message.ToKeyValue(ref mergedDictionary);
            options.ToKeyValue(ref mergedDictionary);
            return new FormUrlEncodedContent(mergedDictionary);
        }

        public static void ToKeyValue(this SmsMessage message, ref Dictionary<string, string> contentDict)
        {
            if (contentDict == null)
            {
                throw new ArgumentNullException(nameof(contentDict));
            }
            if (string.IsNullOrWhiteSpace(message.Message))
            {
                throw new ArgumentNullException(message.Message);
            }
            if (string.IsNullOrWhiteSpace(message.To))
            {
                throw new ArgumentNullException(message.To);
            }
            contentDict.Add("message", message.Message);
            contentDict.Add("to", message.To);
        }
        public static void ToKeyValue(this SmsMessageOptions message, ref Dictionary<string, string> contentDict)
        {
            if (contentDict == null)
            {
                throw new ArgumentNullException(nameof(contentDict));
            }
            contentDict.Add("bulkSmsMode", message.BulkSmsMode.ToString());
            if (string.IsNullOrWhiteSpace(message.UserName))
            {
                throw new ArgumentNullException(message.UserName);
            }
            else
            {
                contentDict.Add("username", message.UserName);
            }
            if (!string.IsNullOrWhiteSpace(message.Keyword))
            {
                contentDict.Add("keyword", message.Keyword);
            }
            if (!string.IsNullOrWhiteSpace(message.LinkId))
            {
                contentDict.Add("linkId", message.LinkId);
            }
            contentDict.Add("enqueue", message.Enqueue.ToString());
            contentDict.Add("retryDurationInHours", message.RetryDurationInHours.ToString());
        }
    }
}
