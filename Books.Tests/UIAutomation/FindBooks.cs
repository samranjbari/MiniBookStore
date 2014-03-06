using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace Books.Tests.UIAutomation
{
    [TestClass]
    public class FindBooks
    {
        private IWebDriver driver;
        private string hubURL = "http://10380d-deegan:4444/wd/hub/";
        private string baseURL;

        [TestInitialize]
        public void Init()
        {
            baseURL = "http://10380d-deegan:801/";

            DesiredCapabilities capability = DesiredCapabilities.InternetExplorer();
            
            driver = new RemoteWebDriver(new Uri(hubURL), capability);
        }

        [TestMethod]
        public void GoToPage()
        {
            driver.Navigate().GoToUrl(baseURL + "/Home/FindBook");

            Assert.AreEqual(driver.Title, "Find Books");
        }

        [TestCleanup]
        public void CleanUp()
        {
            driver.Quit();
        }
    }
}
