namespace CodingMonkey.UITests
{
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class ExerciseCategoryListPageTests
    {
        [Fact]
        public void ExerciseCategoryListPageHasCategoryListDisplayed()
        {
            var basePageObject = new BasePageObject();

            bool categoryListDisplayed = basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListPageObject>()
                .IsCategoryListDisplayed();

            Assert.True(categoryListDisplayed, "Category list is not displayed");

            // TODO: Setup per test class clean up to clean up driver per
            // test class instead of this. 
            basePageObject.QuitDriver();
        }

        [Fact]
        public void ExerciseCategoryListPageHasCategoriesDisplayedInCategoryList()
        {
            var basePageObject = new BasePageObject();

            int numberOfCategoriesDisplayed = basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListPageObject>()
                .GetCountOfCategoriesDisplayed();

            int numberOfSelectCategoryButtonsDisplayed = basePageObject.Get<ExerciseCategoryListPageObject>()
                .GetCountOfSelectCategoryButtons();

            Assert.NotEqual(numberOfCategoriesDisplayed, 0);
            Assert.Equal(numberOfCategoriesDisplayed, numberOfSelectCategoryButtonsDisplayed);

            // TODO: Setup per test class clean up to clean up driver per
            // test class instead of this. 
            basePageObject.QuitDriver();
        }
    }
}
