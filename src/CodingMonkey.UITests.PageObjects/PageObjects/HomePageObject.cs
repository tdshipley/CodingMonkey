namespace CodingMonkey.UITests.PageObjects.PageObjects
{
    using System;
    using Interfaces;
    using OpenQA.Selenium;

    public class HomePageObject : BasePageObject, IPageObject
    {
        public HomePageObject(string baseUrl, IWebDriver driver) : base (baseUrl, driver)
        {
        }

        public bool IsHomepageJumbotronDisplayed()
        {
            return this.IsElementDisplayed(By.ClassName("jumbotron"));
        }

        public bool IsNavigationBarDisplayed()
        {
            return this.IsElementDisplayed(By.ClassName("navbar"));
        }

        public bool IsGetStartedButtonDisplayedInJumbotron()
        {
            string cssSelector = ".jumbotron .btn";
            return this.IsElementDisplayed(By.CssSelector(cssSelector));
        }
    }
}
