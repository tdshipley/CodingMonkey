namespace CodingMonkey.UITests
{
    using System;
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class ExerciseCategoryListPageTests : BaseTest
    {
        [Fact]
        public void ExerciseCategoryListPageHasCategoryListDisplayed()
        {
            bool categoryListDisplayed = _basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListPageObject>()
                .IsCategoryListDisplayed();

            Assert.True(categoryListDisplayed, "Category list is not displayed");
        }

        [Fact]
        public void ExerciseCategoryListPageHasCategoriesDisplayedInCategoryList()
        {
            int numberOfCategoriesDisplayed = _basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListPageObject>()
                .GetCountOfCategoriesDisplayed();

            int numberOfSelectCategoryButtonsDisplayed = _basePageObject.Get<ExerciseCategoryListPageObject>()
                .GetCountOfSelectCategoryButtons();

            Assert.NotEqual(0, numberOfCategoriesDisplayed);
            Assert.Equal(numberOfCategoriesDisplayed, numberOfSelectCategoryButtonsDisplayed);
        }
    }
}
