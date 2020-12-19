﻿using System;
using Azure.Storage.Blobs;
using MailApp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddSingleton((sp) =>
            {
                var blobServiceClient = new BlobServiceClient(AzureStorageConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient("attachments");
                return containerClient;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            });
        }
    }
}