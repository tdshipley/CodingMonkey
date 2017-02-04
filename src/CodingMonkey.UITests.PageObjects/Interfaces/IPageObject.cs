namespace CodingMonkey.UITests.PageObjects.Interfaces
{
    using OpenQA.Selenium;

    public interface IPageObject
    {
        T Get<T>() where T : IPageObject;

        void WaitForPageLoad(int pageLoadSecondsTimeout);

        IWebElement FindVisibleElement(By by, int timeoutInSeconds = 10);
    }
}
