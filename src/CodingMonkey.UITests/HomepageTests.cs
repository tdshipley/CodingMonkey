﻿namespace CodingMonkey.UITests
{
    using PageObjects;
    using PageObjects.PageObjects;
    using System;
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

            bool jumbotronIsDisplayed = basePageObject.Get<HomePageObject>()
                                                      .IsHomepageJumbotronDisplayed();

            Assert.True(jumbotronIsDisplayed);

            // TODO: Setup per test class clean up to clean up driver per
            // test class instead of this. 
            basePageObject.QuitDriver();
        }
    }
}
