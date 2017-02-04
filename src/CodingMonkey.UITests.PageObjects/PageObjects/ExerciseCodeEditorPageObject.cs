namespace CodingMonkey.UITests.PageObjects.PageObjects
{
    using System;
    using Interfaces;
    using OpenQA.Selenium;

    public class ExerciseCodeEditorPageObject : BasePageObject, IPageObject
    {
        public ExerciseCodeEditorPageObject(string baseUrl, IWebDriver driver) : base (baseUrl, driver)
        {
        }

        public bool IsExerciseTitleDisplayed()
        {
            return this.IsElementDisplayed(By.Id("exerciseHeading"));
        }

        public bool IsExerciseGuidanceDisplayed()
        {
            return this.IsElementDisplayed(By.ClassName("exercise-guidance"));
        }

        public bool IsExerciseCodeEditorDisplayed()
        {
            return this.IsElementDisplayed(By.Id("aceEditor"));
        }

        public bool IsSubmitExerciseCodeButtonDisplayed()
        {
            return this.IsElementDisplayed(By.ClassName("submit-code-btn"));
        }
    }
}
