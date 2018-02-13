namespace CodingMonkey.UITests.PageObjects
{
    using System;
    using CodingMonkey.UITests.PageObjects.Interfaces;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Chrome;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    public class BasePageObject : IBasePageObject
    {
        private IWebDriver driverInstance = null;
        protected IWebDriver Driver
        {
            get
            {
                return driverInstance;
            }
        }

        public string BaseUrl { get; set; }

        public BasePageObject(string baseUrl = "http://localhost:49850", IWebDriver driver = null)
        {
            this.BaseUrl = baseUrl;

            if (driver != null)
            {
                this.driverInstance = driver;
            }
            else
            {
                string driverPath = Path.Combine(Directory.GetCurrentDirectory(), "Drivers");
                this.driverInstance = new ChromeDriver(driverPath);
                this.Driver.Navigate().GoToUrl(this.BaseUrl);
            }

            this.WaitForPageLoad();
        }

        public T Get<T>() where T : IPageObject
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { this.BaseUrl, this.Driver });
        }

        public virtual void WaitForPageLoad(int pageLoadSecondsTimeout = 10)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(pageLoadSecondsTimeout));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("page-host")));
        }

        public IWebElement FindVisibleElement(By by, int timeoutInSeconds = 10)
        {
            return this.FindVisibleElements(by, timeoutInSeconds).FirstOrDefault();
        }

        public IReadOnlyCollection<IWebElement> FindVisibleElements(By by, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(by));

            return this.Driver.FindElements(by);
        }

        public void QuitDriver()
        {
            this.Driver.Quit();
            this.driverInstance = null;
        }

        public bool IsElementDisplayed(By by)
        {
            bool displayed = false;

            try
            {
                this.FindVisibleElement(by);
                displayed = true;
            }
            catch { }

            return displayed;
        }

    }
}
