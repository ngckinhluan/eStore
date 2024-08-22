using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace AutomatedUITests
{
    public class AutomatedUiTests : IDisposable
    {
        private readonly IWebDriver _driver;

        public AutomatedUiTests()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--ignore-certificate-errors");
            _driver = new ChromeDriver(chromeOptions);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
        
        private void NavigateAndMaximize(string url)
        {
            _driver.Navigate().GoToUrl(url);
            _driver.Manage().Window.Maximize();
        }

        // Open localhost:5173 in Chrome browser, maximize the window,
        // and check if the page title is available,
        // wait for 5 seconds and close the browser.
        [Fact]
        public void TestSwaggerUi()
        {
            NavigateAndMaximize("http://localhost:5173/swagger/index.html");
            var swaggerHeader = _driver.FindElement(By.XPath("//h2[@class='title']"));
            var url = _driver.FindElement(By.XPath("//span[@class='url']"));
            Assert.NotNull(swaggerHeader);
            Assert.NotNull(url);
            Thread.Sleep(5000);
        }
        
        //
        // [Fact]
        // public void TestGetCategory()
        // {
        //     NavigateAndMaximize("http://localhost:5173/api/category");
        //     var categoryHeader = _driver.FindElement(By.XPath("//h1[contains(text(), 'Category')]"));
        //     Assert.NotNull(categoryHeader);
        //     Thread.Sleep(5000);
        // }
    }
}