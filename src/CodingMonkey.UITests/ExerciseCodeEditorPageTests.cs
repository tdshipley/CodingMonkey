namespace CodingMonkey.UITests
{
    using System;
    using PageObjects;
    using PageObjects.PageObjects;
    using Xunit;

    public class ExerciseCodeEditorPageTests : BaseTest
    {
        [Fact]
        public void ExerciseCodeEditorPageHasExerciseDetailsAndEditorDisplayed()
        {
            bool exerciseTitleDisplayed = _basePageObject.Get<HomePageObject>()
                .ClickPickCategoryButton()
                .Get<ExerciseCategoryListPageObject>()
                .ClickSelectCategoryButtonForFirstFoundCategory()
                .Get<ExerciseListPageObject>()
                .ClickSelectExerciseButtonForFirstFoundExercise()
                .Get<ExerciseCodeEditorPageObject>()
                .IsExerciseTitleDisplayed();

            bool exerciseGuidanceDisplayed = _basePageObject.Get<ExerciseCodeEditorPageObject>()
                .IsExerciseGuidanceDisplayed();

            bool exerciseCodeEditorDisplayed = _basePageObject.Get<ExerciseCodeEditorPageObject>()
                .IsExerciseCodeEditorDisplayed();

            bool submitExerciseCodeButtonDisplayed = _basePageObject.Get<ExerciseCodeEditorPageObject>()
                .IsSubmitExerciseCodeButtonDisplayed();

            Assert.True(exerciseTitleDisplayed, "Exercise title is not displayed");
            Assert.True(exerciseGuidanceDisplayed, "Exercise guidance is not displayed");
            Assert.True(exerciseCodeEditorDisplayed, "Exercise code editor is not displayed");
            Assert.True(submitExerciseCodeButtonDisplayed, "Submit exercise code button is not displayed");
        }
    }
}
