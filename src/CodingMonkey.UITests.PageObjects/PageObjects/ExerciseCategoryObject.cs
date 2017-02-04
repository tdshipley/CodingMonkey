namespace CodingMonkey.UITests.PageObjects.PageObjects
{
    using System;
    using Interfaces;
    using OpenQA.Selenium;

    public class ExerciseCategoryObject : BasePageObject, IPageObject
    {
        public ExerciseCategoryObject(string baseUrl, IWebDriver driver) : base (baseUrl, driver)
        {
        }

        public bool IsCategoryListDisplayed()
        {
            return this.IsElementDisplayed(By.Id("category-list"));
        }

        public int GetCountOfCategoriesDisplayed()
        {
            return this.FindVisibleElements(By.ClassName("category-list-item")).Count;
        }

        public int GetCountOfSelectCategoryButtons()
        {
            return this.FindVisibleElements(By.ClassName("select-category-btn")).Count;
        }
    }
}
