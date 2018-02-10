namespace CodingMonkey.UITests
{
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class ExerciseCodeEditorPageTests
    {
        [Fact]
        public void ExerciseCodeEditorPageHasExerciseDetailsAndEditorDisplayed()
        {
            var basePageObject = new BasePageObject();

            bool exerciseTitleDisplayed = basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListPageObject>()
                .ClickSelectCategoryButtonForFirstFoundCategory()
                .Get<ExerciseListPageObject>()
                .ClickSelectExerciseButtonForFirstFoundExercise()
                .Get<ExerciseCodeEditorPageObject>()
                .IsExerciseTitleDisplayed();

            bool exerciseGuidanceDisplayed = basePageObject.Get<ExerciseCodeEditorPageObject>()
                .IsExerciseGuidanceDisplayed();

            bool exerciseCodeEditorDisplayed = basePageObject.Get<ExerciseCodeEditorPageObject>()
                .IsExerciseCodeEditorDisplayed();

            bool submitExerciseCodeButtonDisplayed = basePageObject.Get<ExerciseCodeEditorPageObject>()
                .IsSubmitExerciseCodeButtonDisplayed();

            Assert.True(exerciseTitleDisplayed, "Exercise title is not displayed");
            Assert.True(exerciseGuidanceDisplayed, "Exercise guidance is not displayed");
            Assert.True(exerciseCodeEditorDisplayed, "Exercise code editor is not displayed");
            Assert.True(submitExerciseCodeButtonDisplayed, "Submit exercise code button is not displayed");

            // TODO: Setup per test class clean up to clean up driver per
            // test class instead of this. 
            basePageObject.QuitDriver();
        }
    }
}
