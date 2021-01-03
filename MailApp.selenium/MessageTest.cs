using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace MailApp.selenium
{
    [Trait("Environment", "Localhost")]
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
            var chromeDriverLocation = string.IsNullOrEmpty(Environment.GetEnvironmentVariable(_ChromeDriverEnvironmentVariableName)) ?
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) :
                Environment.GetEnvironmentVariable(_ChromeDriverEnvironmentVariableName);
            _webDriver = new ChromeDriver(chromeDriverLocation, chromeOptions);

        }
        [Fact]
        public void Send_Message_No_Attachment()
        {
            
            _webDriver.Navigate().GoToUrl($"{_server.RootUri}/{_ResourceEndpoint}");
            ////_webDriver.Navigate().GoToUrl(_httpClient.BaseAddress);
            //Thread.Sleep(5000);
            //FindElement(By.Id("NewMessage"), 10).Click();
            ////FindElement(By.Id("Receiver"), 10).SendKeys("testowytestowy844@gmail.com");
            //// _webDriver.FindElement(By.Id("Subject")).SendKeys("subject");
            //// _webDriver.FindElement(By.Id("exampleFormControlTextarea1")).SendKeys("text");
            //// _webDriver.FindElement(By.Id("Send")).Click();
            //Assert.Equal("https://localhost:5001/Message/NewMessage", _webDriver.Url);
            Dispose();
        }
        //[Fact]
        //public void Open_Message()
        //{
        //    FindElement(By.Id("mess"), 5).Click();
        //}
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
