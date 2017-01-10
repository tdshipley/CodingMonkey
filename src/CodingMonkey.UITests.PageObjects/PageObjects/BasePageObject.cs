namespace CodingMonkey.UITests.PageObjects
{
    using System;
    using CodingMonkey.UITests.PageObjects.Interfaces;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Chrome;
    using System.IO;

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
                string srcDir = this.GetSolutionPath();
                string driverPath = Path.Combine(srcDir, "src\\CodingMonkey.UITests.PageObjects\\bin\\Debug\\net452\\Drivers");
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
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(by));
            var element = this.Driver.FindElement(by);

            return element;
        }

        public void QuitDriver()
        {
            this.Driver.Quit();
            this.driverInstance = null;
        }

        private string GetSolutionPath()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string soultionPath = currentDir.Substring(0, currentDir.IndexOf("\\src"));

            return soultionPath;
        }

    }
}
