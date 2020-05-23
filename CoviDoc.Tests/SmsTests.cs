using System;
using System.Threading.Tasks;
using CoviDoc.Services;
using CoviDoc.Services.Models;
using CoviDoc.Tests.Factories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace CoviDoc.Tests
{
    public class SmsTests : IClassFixture<CovidDocWebApplicationFactory>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly IServiceProvider _serviceProvider;

        public SmsTests(CovidDocWebApplicationFactory factory, ITestOutputHelper outputHelper)
        {
            _serviceProvider = factory.Services;
            _outputHelper = outputHelper;
        }

        [Fact]
        public void CanResolveSmsGateWayClient()
        {
            var smsGateWayClient = _serviceProvider.GetRequiredService<AfricasTalkingGateway>();
            smsGateWayClient.Should().NotBeNull();
        }

        [Fact]
        public async Task CanSendSandBoxSms()
        {
            var smsGateWayClient = _serviceProvider.GetRequiredService<AfricasTalkingGateway>();
            var result= await smsGateWayClient.SendSmsMessage(new SmsMessage("+254710773556", message: "Testing..."));
            result.ErrorMessage.Should().BeNullOrWhiteSpace();
            result.AtResponse.Should().NotBeNull();
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}