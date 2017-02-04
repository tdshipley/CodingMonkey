namespace CodingMonkey.UITests
{
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class HomepageTests
    {
        [Fact]
        public void HomepageHasJumbotronDisplayed()
        {
            var basePageObject = new BasePageObject();

            bool jumbotronIsDisplayed = basePageObject.Get<HomePageObject>()
                                                      .IsHomepageJumbotronDisplayed();

            bool getStartedButtonDisplayedInJumbotron = basePageObject.Get<HomePageObject>()
                                                                      .IsGetStartedButtonDisplayedInJumbotron();

            Assert.True(jumbotronIsDisplayed, "Homepage jumbotron is not displayed");
            Assert.True(getStartedButtonDisplayedInJumbotron, "Get started button in the homepage jumbotron is not displayed");

            // TODO: Setup per test class clean up to clean up driver per
            // test class instead of this. 
            basePageObject.QuitDriver();
        }

        [Fact]
        public void HomepageHasNavigationBarDisplayed()
        {
            var basePageObject = new BasePageObject();

            bool navigationBarIsDisplayed = basePageObject.Get<HomePageObject>()
                                                          .IsNavigationBarDisplayed();

            Assert.True(navigationBarIsDisplayed, "Navigation bar is not displayed");

            basePageObject.QuitDriver();
        }
    }
}
