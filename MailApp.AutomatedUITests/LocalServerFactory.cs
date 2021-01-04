using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace MailApp.AutomatedUITests
{
    public class LocalServerFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private IWebHost _host;

        public string RootUri { get; set; } = "http://localhost";

        public LocalServerFactory()
        {
            ClientOptions.BaseAddress = new Uri(RootUri);
            CreateServer(CreateWebHostBuilder());
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            _host = builder.Build();
            _host.Start();
            RootUri = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();
            return new TestServer(new WebHostBuilder().UseStartup<TStartup>());
        }

        protected override IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder(new string[0])
                .UseStartup<TStartup>()
                .UseSetting("ENVIRONMENT", "Development");

        [ExcludeFromCodeCoverage]
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _host?.Dispose();
            }
        }
    }
}