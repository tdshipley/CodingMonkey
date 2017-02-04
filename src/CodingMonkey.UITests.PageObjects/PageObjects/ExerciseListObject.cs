namespace CodingMonkey.UITests.PageObjects.PageObjects
{
    using System;
    using Interfaces;
    using OpenQA.Selenium;

    public class ExerciseListObject : BasePageObject, IPageObject
    {
        public ExerciseListObject(string baseUrl, IWebDriver driver) : base(baseUrl, driver)
        {
        }

        public bool IsExerciseListDisplayed()
        {
            return this.IsElementDisplayed(By.Id("exercise-list"));
        }

        public int GetCountOfExercisesDisplayed()
        {
            return this.FindVisibleElements(By.ClassName("exercise-list-item")).Count;
        }

        public int GetCountOfSelectExerciseButtons()
        {
            return this.FindVisibleElements(By.ClassName("select-exercise-btn")).Count;
        }

        public ExerciseListObject ClickSelectExerciseButtonForFirstFoundExercise()
        {
            this.FindVisibleElement(By.ClassName("select-exercise-btn")).Click();
            return this;
        }
    }
}
