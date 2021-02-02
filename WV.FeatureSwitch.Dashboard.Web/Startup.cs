using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WV.FeatureSwitch.Dashboard.DAL.DBContext;
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

            //AppConfigValues.ApiBaseUrl = "http://localhost:8622";

            AppConfigValues.ApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("ApiBaseUrl").Value;
            //AppConfigValues.DataCacheApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("DataCacheApiBaseUrl").Value;
            AppConfigValues.ApiToken = Configuration.GetSection("ApiConfig").GetSection("ApiToken").Value;
            AppConfigValues.ApiVersion = Configuration.GetSection("ApiConfig").GetSection("ApiVersion").Value;
            AppConfigValues.LogStorageContainer = Configuration.GetSection("LogStorageDetails").GetSection("LogStorageContainer").Value;
            AppConfigValues.StorageAccountKey = Configuration.GetSection("LogStorageDetails").GetSection("StorageAccountKey").Value;
            AppConfigValues.StorageAccountName = Configuration.GetSection("LogStorageDetails").GetSection("StorageAccountName").Value;
            AppConfigValues.CRMType = Configuration.GetSection("CRMExtractData").GetSection("CRMType").Value;
            AppConfigValues.XSLTStorageContainer = Configuration.GetSection("LogStorageDetails").GetSection("XSLTStorageContainer").Value;
            //AppConfigValues.BaseApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("BaseApiBaseUrl").Value;
            //AppConfigValues.BaseAdyenApiBaseUrl = Configuration.GetSection("ApiConfig").GetSection("BaseAdyenApiBaseUrl").Value;
            AppConfigValues.HostedCountry = Configuration.GetSection("CRMExtractData").GetSection("HostedCountry").Value;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<FeatureSwitchDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FeatureSwitchConnection")));


            // Use same instance within a scope and create new instance for different http request and out of scope.
            services.AddScoped<IFeatureSwitchFactory, FeatureSwitchFactory>();

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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
