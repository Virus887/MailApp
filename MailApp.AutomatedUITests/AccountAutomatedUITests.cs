using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;
using MailApp;
using System.Net.Http;
using OpenQA.Selenium.Remote;

namespace MailApp.AutomatedUITests
{
    public class AccountAutomatedUITests : IClassFixture<LocalServerFactory<MailApp.Startup>>, IDisposable
    {
        private const string _ResourceEndpoint = "Accounts";
        private const string _ChromeDriverEnvironmentVariableName = "ChromeWebDriver";
        private readonly HttpClient _httpClient;
        private readonly RemoteWebDriver _webDriver;
        private readonly LocalServerFactory<Startup> _server;

        public AccountAutomatedUITests(LocalServerFactory<Startup> server)
        {
            _server = server;
            _httpClient = server.CreateClient();
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--ignore-certificate-errors");
            var chromeDriverLocation = string.IsNullOrEmpty(Environment.GetEnvironmentVariable(_ChromeDriverEnvironmentVariableName))
                ? System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                : Environment.GetEnvironmentVariable(_ChromeDriverEnvironmentVariableName);
            _webDriver = new ChromeDriver(chromeDriverLocation, chromeOptions);
        }


        public IWebElement FindElement(By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }

            return _webDriver.FindElement(by);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _webDriver?.Dispose();
            }
        }
    }
}