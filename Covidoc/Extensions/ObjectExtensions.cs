using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using CoviDoc.Services.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;

namespace CoviDoc.Services
{
    public static class ObjectExtensions
    {
        public static FormUrlEncodedContent ToFormUrlEncodedContent(this SmsMessage message, SmsMessageOptions options)
        {
            var smsMessageDictionary = message.ToKeyValue();
            var smsMessageOptionsDictionary = options.ToKeyValue();
            smsMessageDictionary.ToList().ForEach(x => smsMessageOptionsDictionary[x.Key] = x.Value);
            return new FormUrlEncodedContent(smsMessageOptionsDictionary);
        }

        public static IDictionary<string, string> ToKeyValue(this SmsMessage message)
        {
            var dictionary = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(message.Message))
            {
                throw new ArgumentNullException(message.Message);
            }
            if (string.IsNullOrWhiteSpace(message.To))
            {
                throw new ArgumentNullException(message.To);
            }
            dictionary.Add("message", message.Message);
            dictionary.Add("to", message.To);
            return dictionary;
        }
        public static IDictionary<string, string> ToKeyValue(this SmsMessageOptions message)
        {
            var dictionary = new Dictionary<string, string> { { "bulkSmsMode", message.BulkSmsMode.ToString() } };
            if (string.IsNullOrWhiteSpace(message.UserName))
            {
                throw new ArgumentNullException(message.UserName);
            }
            else
            {
                dictionary.Add("username", message.UserName);
            }
            if (!string.IsNullOrWhiteSpace(message.Keyword))
            {
                dictionary.Add("keyword", message.Keyword);
            }
            if (!string.IsNullOrWhiteSpace(message.LinkId))
            {
                dictionary.Add("linkId", message.LinkId);
            }
            dictionary.Add("enqueue", message.Enqueue.ToString());
            dictionary.Add("retryDurationInHours", message.RetryDurationInHours.ToString());
            return dictionary;
        }
    }
}
