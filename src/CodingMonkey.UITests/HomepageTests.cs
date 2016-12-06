namespace CodingMonkey.UITests
{
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class HomepageTests
    {
        public HomepageTests()
        {
        }

        [Fact]
        public void HomepageHasJumbotronDisplayed()
        {
            var basePageObject = new BasePageObject();

            bool jumbotronIsDisplayed =  basePageObject.Get<HomePageObject>()
                                                       .IsHomepageJumbotronDisplayed();

            Assert.True(jumbotronIsDisplayed);
        }
    }
}
