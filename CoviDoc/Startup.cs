using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using CoviDoc.Models.Mocks;
using CoviDoc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FileService;
using Microsoft.Extensions.Logging;
using Ngrok.AspNetCore;


namespace CoviDoc
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var covidDocConfig = new CoviDocConfig();
            Configuration.GetSection(nameof(CoviDocConfig)).Bind(covidDocConfig);
            services.AddSingleton(covidDocConfig);


            services.AddControllersWithViews();
            services.AddSingleton<IPatientRepository, MockPatientRepository>();
            services.AddSingleton<ITestCentreRepository, MockTestCentreRepository>();
            services.AddSingleton<ILocationRepository, MockLocationRepository>();
            services.AddSingleton<IFileUtility, DiskFileUtility>();
            services.AddSingleton<IDiagnosisReportRepository, MockDiagnosisReportRepository>();

            services.AddSingleton<SmsMessageOptions>(provider =>
            {
                if (_environment.IsDevelopment())
                {
                    var smsMessageOptions = new SmsMessageOptions("sandbox", Constants.SandboxApiHostUri, "AFRICASTKNG");
                    return smsMessageOptions;
                }
                else
                {
                    var config = provider.GetRequiredService<CoviDocConfig>();
                    var smsMessageOptions = new SmsMessageOptions(config.ATUserName, Constants.LiveApiHostUri, config.KeyWord);
                    return smsMessageOptions;
                }
            });

            services.AddHttpClient<AfricasTalkingGateway>()
                .ConfigureHttpClient((provider, client) =>
                {
                    var productInfoHeader = new ProductInfoHeaderValue("CovidDoc", "v1.0");
                    var acceptHeader = new MediaTypeWithQualityHeaderValue("application/json", 1);
                    var acceptCharset = new StringWithQualityHeaderValue(Encoding.UTF8.HeaderName);

                    client.DefaultRequestHeaders.Accept.Add(acceptHeader);
                    client.DefaultRequestHeaders.UserAgent.Add(productInfoHeader);
                    client.DefaultRequestHeaders.AcceptCharset.Add(acceptCharset);

                    var config = provider.GetRequiredService<CoviDocConfig>();
                    client.DefaultRequestHeaders.Add("apiKey", config.ATApiKey);
                }).SetHandlerLifetime(TimeSpan.FromMinutes(2))
                .ConfigurePrimaryHttpMessageHandler(provider =>
                {
                    var logger = provider.GetRequiredService<ILogger<HttpClientHandler>>();
                    return new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        AutomaticDecompression = DecompressionMethods.All,
                        ServerCertificateCustomValidationCallback =
                            (httpRequestMessage, sslCertificate, certChain, policyErrors) =>
                            {
                                logger.LogInformation(
                                    $"Uri: {httpRequestMessage.RequestUri}:{Environment.NewLine}" +
                                    $"Subject:{sslCertificate.Subject}:{Environment.NewLine}" +
                                    $"Issuer:{sslCertificate.Issuer}:{Environment.NewLine}" +
                                    $"NotBefore:{sslCertificate.NotBefore}:{Environment.NewLine}" +
                                    $"FriendlyName:{sslCertificate.FriendlyName}:{Environment.NewLine}" +
                                    $"Errors:{policyErrors}");
                                return true;
                            }
                    };
                });

            #region Ngrok

            //services.AddNgrok(options =>
            //{
            //    options.DetectUrl = true;
            //    options.DownloadNgrok = true;
            //    options.ManageNgrokProcess = true;
            //    options.RedirectLogs = true;
            //    options.Disable = !_environment.IsDevelopment();
            //});

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Patients}/{action=Index}/{id?}");
            });
        }
    }
}
