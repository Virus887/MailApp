using System;
using System.Globalization;
using Azure.Storage.Blobs;
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
using Microsoft.OpenApi.Models;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.ApplicationInsights;

namespace MailApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        private String MailAppConnectionString => Configuration.GetConnectionString("MailApp");
        private String AzureStorageConnectionString => Configuration.GetConnectionString("AzureStorage");

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
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

            //hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(MailAppConnectionString));
            services.AddHangfireServer();

            //swagger
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("docs", new OpenApiInfo
                {
                    Title = "API Documentation",
                    Version = "v1"
                });
                x.IgnoreObsoleteProperties();
                x.EnableAnnotations();
            });
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

            app.UseMiddleware<ApplicationInsightsMiddleware>();
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

                endpoints.MapGet($"/jobs/{nameof(RetentionJob)}", context =>
                    ActivatorUtilities.CreateInstance<RetentionJob>(context.RequestServices).RemoveOldAttachments());
            });

            //Hangfire
            jobManager.AddOrUpdate("",
                () => ActivatorUtilities.CreateInstance<NotificationSenderJob>(sp).SendNotifications()
                , Cron.Daily);

            jobManager.AddOrUpdate("",
                () => ActivatorUtilities.CreateInstance<RetentionJob>(sp).RemoveOldAttachments()
                , Cron.Daily);

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/docs/swagger.json", "API Documentation");
                x.DefaultModelsExpandDepth(-1);
            });
        }
    }
}