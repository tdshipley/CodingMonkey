namespace CodingMonkey.UITests.PageObjects.PageObjects
{
    using Interfaces;
    using OpenQA.Selenium;

    public class HomePageObject : BasePageObject, IPageObject
    {
        public HomePageObject(string baseUrl, IWebDriver driver) : base (baseUrl, driver)
        {
        }

        public bool IsHomepageJumbotronDisplayed()
        {
            bool displayed = false;

            try
            {
                this.FindVisibleElement(By.Id("jumbotron"));
                displayed = true;
            }
            catch { }
            
            return displayed;
        }
    }
}
