using System;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using Hangfire;
using MailApp.Infrastructure;
using MailApp.Infrastructure.Notifications;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RestEase;

namespace MailApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public String MailAppConnectionString => Configuration.GetConnectionString("MailApp");
        public String AzureStorageConnectionString => Configuration.GetConnectionString("AzureStorage");

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(x =>
            {
                x.Filters.Add(new AuthorizeFilter());
                x.Filters.Add<EnsureEntitiesAreAttached>();
                x.Filters.Add<EnsureAccountActionFilter>();
            });
            services.AddDbContext<MailAppDbContext>(x => x.UseSqlServer(MailAppConnectionString));
            services.AddRazorPages();
            services.AddAuthentication(AzureADB2CDefaults.AuthenticationScheme)
                .AddAzureADB2C(options => Configuration.Bind("AzureAdB2C", options));

            services.AddHttpContextAccessor();
            services.AddScoped<IAccountProvider>(sp => new CacheAccountProvider(ActivatorUtilities.CreateInstance<AccountProvider>(sp)));
            services.AddSingleton(sp =>
            {
                var blobServiceClient = new BlobServiceClient(AzureStorageConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient("attachments");
                return containerClient;
            });

            services.Configure<NotificationClientOption>(x => Configuration.GetSection(nameof(NotificationClientOption)).Bind(x));
            
            services
                .AddHttpClient(nameof(INotificationClient), (serviceProvider, httpClient) =>
                {
                    var option = serviceProvider.GetService<IOptions<NotificationClientOption>>();
                    httpClient.DefaultRequestHeaders.Add("x-api-key", option.Value.ApiKey);
                    httpClient.BaseAddress = new Uri(option.Value.Url);
                })
                .AddTypedClient(RestClient.For<INotificationClient>);

            services.AddHangfire(x => x.UseSqlServerStorage(MailAppConnectionString));
            services.AddHangfireServer();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp, IRecurringJobManager jobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapGet($"/jobs/{nameof(NotificationSenderJob)}", context =>
                    ActivatorUtilities.CreateInstance<NotificationSenderJob>(context.RequestServices).SendNotifications());
            });

            //Hangfire
            jobManager.AddOrUpdate("",
                () => ActivatorUtilities.CreateInstance<NotificationSenderJob>(sp).SendNotifications()
                , Cron.Daily);
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}