namespace CodingMonkey.UITests
{
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class ExerciseListPageTests
    {
        [Fact]
        public void ExerciseListPageHasExerciseListDisplayed()
        {
            var basePageObject = new BasePageObject();

            bool exerciseListDisplayed = basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListObject>()
                .ClickSelectCategoryButtonForFirstFoundCategory()
                .Get<ExerciseListObject>()
                .IsExerciseListDisplayed();

            Assert.True(exerciseListDisplayed, "Exercise list is not displayed");

            // TODO: Setup per test class clean up to clean up driver per
            // test class instead of this. 
            basePageObject.QuitDriver();
        }

        [Fact]
        public void ExerciseCategoryPageHasCategoriesDisplayedInCategoryList()
        {
            var basePageObject = new BasePageObject();

            int countOfExercisesDisplayed = basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListObject>()
                .ClickSelectCategoryButtonForFirstFoundCategory()
                .Get<ExerciseListObject>()
                .GetCountOfExercisesDisplayed();

            int numberOfSelectExerciseButtonsDisplayed = basePageObject.Get<ExerciseListObject>()
                .GetCountOfSelectExerciseButtons();

            Assert.NotEqual(countOfExercisesDisplayed, 0);
            Assert.Equal(countOfExercisesDisplayed, numberOfSelectExerciseButtonsDisplayed);

            // TODO: Setup per test class clean up to clean up driver per
            // test class instead of this. 
            basePageObject.QuitDriver();
        }
    }
}
