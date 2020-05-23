using System.Net.Http;
using System.Threading.Tasks;
using CoviDoc.Services.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoviDoc.Services
{
    public class AfricasTalkingGateway
    {
        private readonly ILogger<AfricasTalkingGateway> _logger;
        private readonly HttpClient _client;
        private readonly SmsMessageOptions _smsMessageOptions;

        public AfricasTalkingGateway(ILogger<AfricasTalkingGateway> logger, HttpClient client, SmsMessageOptions smsMessageOptions)
        {
            _logger = logger;
            _client = client;
            _smsMessageOptions = smsMessageOptions;
        }

        public async Task<ResponseWrapper> SendSmsMessage(SmsMessage message)
        {
            var messageFormData = message.ToFormUrlEncodedContent(this._smsMessageOptions);
            var result = await this._client.PostAsync(this._smsMessageOptions.ApiHost, messageFormData);
            var responseString = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                var atResponse = JsonConvert.DeserializeObject<AtResponse>(responseString);
                var responseWrapper = new ResponseWrapper(atResponse);
                return responseWrapper;
            }
            else
            {
                var responseWrapper = new ResponseWrapper(responseString, result.StatusCode, result.ReasonPhrase);
                return responseWrapper;
            }
        }
    }
}