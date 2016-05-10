'use strict';

var _pageObjectsCategoriesPoJs = require('./page-objects/categories.po.js');

var _pageObjectsExercisesPoJs = require('./page-objects/exercises.po.js');

var _pageObjectsSkeletonPoJs = require('./page-objects/skeleton.po.js');

describe('Coding Monkey', function () {
  var poCategories = undefined;
  var poExercises = undefined;
  var poSkeleton = undefined;

  beforeEach(function () {
    poSkeleton = new _pageObjectsSkeletonPoJs.PageObjectSkeleton();
    poCategories = new _pageObjectsCategoriesPoJs.PageObjectCategories();
    poExercises = new _pageObjectsExercisesPoJs.PageObjectExercises();

    browser.loadAndWaitForAureliaPage('http://localhost:5000/#/categories');
  });

  it('should load the page and display the initial page title', function () {
    expect(poSkeleton.getCurrentPageTitle()).toBe('Exercise Categories | Coding Monkey');
  });

  it('should display some categories', function () {
    expect(poCategories.getCategoriesDisplayedCount()).toBeGreaterThan(0);
  });

  it('should go to the exercise selection page when category has been selected', function () {
    var categoryIdToTest = 'string_manipulation';
    var categoryTitle = 'String Manipulation';

    expect(poCategories.getCatgeoryTitle(categoryIdToTest)).toEqual(categoryTitle);
    poCategories.pressSelectCategoryButton(categoryIdToTest);

    browser.waitForRouterComplete();

    expect(poSkeleton.getCurrentPageTitle()).toBe('Exercises in Category | Coding Monkey');
    expect(poExercises.getExercisesDisplayedCount()).toBeGreaterThan(0);

    var exerciseIdToTest = "get_first_letter_of_a_string";
    var exerciseTitle = "Get First Letter of a String";

    expect(poExercises.getExerciseTitle(exerciseIdToTest)).toEqual(exerciseTitle);
  });
});