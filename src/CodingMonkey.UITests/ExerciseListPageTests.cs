namespace CodingMonkey.UITests
{
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class ExerciseListPageTests : BaseTest
    {
        [Fact]
        public void ExerciseListPageHasExerciseListDisplayed()
        {
            bool exerciseListDisplayed = _basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListPageObject>()
                .ClickSelectCategoryButtonForFirstFoundCategory()
                .Get<ExerciseListPageObject>()
                .IsExerciseListDisplayed();

            Assert.True(exerciseListDisplayed, "Exercise list is not displayed");
        }

        [Fact]
        public void ExerciseCategoryPageHasCategoriesDisplayedInCategoryList()
        {
            int countOfExercisesDisplayed = _basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListPageObject>()
                .ClickSelectCategoryButtonForFirstFoundCategory()
                .Get<ExerciseListPageObject>()
                .GetCountOfExercisesDisplayed();

            int numberOfSelectExerciseButtonsDisplayed = _basePageObject.Get<ExerciseListPageObject>()
                .GetCountOfSelectExerciseButtons();

            Assert.NotEqual(0, countOfExercisesDisplayed);
            Assert.Equal(countOfExercisesDisplayed, numberOfSelectExerciseButtonsDisplayed);
        }
    }
}
