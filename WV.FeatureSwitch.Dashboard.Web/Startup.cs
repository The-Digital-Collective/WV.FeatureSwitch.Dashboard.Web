using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WV.FeatureSwitch.Dashboard.DAL.ApiClientFactory.Factory;
using WV.FeatureSwitch.Dashboard.DAL.ApiClientFactory.FactoryInterfaces;
using WV.FeatureSwitch.Dashboard.DAL.ViewModels;

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