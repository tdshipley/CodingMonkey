namespace CodingMonkey.UITests
{
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class HomepageTests : BaseTest
    {
        [Fact]
        public void HomepageHasJumbotronDisplayed()
        {
            bool jumbotronIsDisplayed = _basePageObject.Get<HomePageObject>()
                                                      .IsHomepageJumbotronDisplayed();

            bool getStartedButtonDisplayedInJumbotron = _basePageObject.Get<HomePageObject>()
                                                                      .IsGetStartedButtonDisplayedInJumbotron();

            Assert.True(jumbotronIsDisplayed, "Homepage jumbotron is not displayed");
            Assert.True(getStartedButtonDisplayedInJumbotron, "Get started button in the homepage jumbotron is not displayed");
        }

        [Fact]
        public void HomepageHasNavigationBarDisplayed()
        {
            bool navigationBarIsDisplayed = _basePageObject.Get<HomePageObject>()
                                                          .IsNavigationBarDisplayed();

            Assert.True(navigationBarIsDisplayed, "Navigation bar is not displayed");
        }
    }
}
