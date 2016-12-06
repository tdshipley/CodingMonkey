namespace CodingMonkey.UITests.PageObjects
{
    using System;
    using CodingMonkey.UITests.PageObjects.Interfaces;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Chrome;

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

        public BasePageObject(string baseUrl = "http://localhost:5000", IWebDriver driver = null)
        {
            this.BaseUrl = baseUrl;

            if (driver != null)
            {
                this.driverInstance = driver;
            }
            else
            {
                this.driverInstance = new ChromeDriver();
                this.Driver.Navigate().GoToUrl(this.BaseUrl);
            }

            this.WaitForPageLoad();
        }

        public T Get<T>() where T : IPageObject
        {
            throw new NotImplementedException();
        }

        public virtual void WaitForPageLoad(int pageLoadSecondsTimeout = 10)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(pageLoadSecondsTimeout));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("page-host")));
        }

        public IWebElement FindVisibleElement(By by, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(by));
            var element = this.Driver.FindElement(by);

            return element;
        }

    }
}
