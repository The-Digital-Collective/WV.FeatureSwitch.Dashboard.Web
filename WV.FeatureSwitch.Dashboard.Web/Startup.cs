using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using WV.FeatureSwitch.Dashboard.Web.ApiClientFactory.Factory;
using WV.FeatureSwitch.Dashboard.Web.ApiClientFactory.FactoryInterfaces;
using WV.FeatureSwitch.Dashboard.Web.ViewModels;

namespace WV.FeatureSwitch.Dashboard.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AppConfigValues.ApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("ApiBaseUrl").Value;            
            AppConfigValues.ApiToken = Configuration.GetSection("ApiConfig").GetSection("ApiToken").Value;
            AppConfigValues.ApiVersion = Configuration.GetSection("ApiConfig").GetSection("ApiVersion").Value;      
            AppConfigValues.LogStorageContainer = Configuration.GetSection("LogStorageDetails").GetSection("LogStorageContainer").Value;
            AppConfigValues.StorageAccountKey = Configuration.GetSection("LogStorageDetails").GetSection("StorageAccountKey").Value;
            AppConfigValues.StorageAccountName = Configuration.GetSection("LogStorageDetails").GetSection("StorageAccountName").Value;
            AppConfigValues.XSLTStorageContainer = Configuration.GetSection("LogStorageDetails").GetSection("XSLTStorageContainer").Value;
            AppConfigValues.ApiCountry = Configuration.GetSection("ApiConfig").GetSection("ApiCountry").Value;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();         

            // Use same instance within a scope and create new instance for different http request and out of scope.
            services.AddScoped<IFeatureSwitchFactory, FeatureSwitchFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Serilog.Formatting.Json.JsonFormatter jsonFormatter = new Serilog.Formatting.Json.JsonFormatter();
            string connectionString = GetAzureConnectionString(AppConfigValues.StorageAccountName, AppConfigValues.StorageAccountKey);

            Log.Logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(Configuration)
                       .WriteTo.AzureBlobStorage(jsonFormatter, connectionString, Serilog.Events.LogEventLevel.Information, AppConfigValues.LogStorageContainer, "{yyyy}/{MM}/FeatureSwitchDashboard/log-{dd}.json", false, TimeSpan.FromDays(1))
                       .CreateLogger();
            // logging
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=FeatureSwitch}/{action=Index}/{id?}");
            });
        }
        private static string GetAzureConnectionString(string accountName, string accountKey)
        {
            string azureConnection = "DefaultEndpointsProtocol=https;AccountName=" + accountName + ";AccountKey=" + accountKey + ";EndpointSuffix=core.windows.net";
            return azureConnection;
        }
    }
}