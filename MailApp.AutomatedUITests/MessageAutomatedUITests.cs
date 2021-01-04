using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace MailApp.AutomatedUITests
{
    public class MessageAutomatedUITests : IClassFixture<LocalServerFactory<MailApp.Startup>>, IDisposable
    {
        private const string _ResourceEndpoint = "Message";
        private const string _ChromeDriverEnvironmentVariableName = "ChromeWebDriver";
        private readonly HttpClient _httpClient;
        private readonly RemoteWebDriver _webDriver;
        private readonly LocalServerFactory<Startup> _server;


        public MessageAutomatedUITests(LocalServerFactory<Startup> server)
        {
            _server = server;
            _httpClient = server.CreateClient();
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--ignore-certificate-errors");
            chromeOptions.AddArguments("--allow-insecure-localhost");

            var chromeDriverLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _webDriver = new ChromeDriver(chromeDriverLocation, chromeOptions);
        }

        [Fact]
        public void Send_Message_No_Attachment()
        {
            var url = $"{_server.RootUri}/{_ResourceEndpoint}";
            _webDriver.Navigate().GoToUrl(url);
            FindElement(By.Id("NewMessage"), 10).Click();

            Assert.Equal("https://localhost:5001/Message/NewMessage", _webDriver.Url);
            Dispose();
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

        public IWebElement FindElement(By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }

            return _webDriver.FindElement(by);
        }
    }
}