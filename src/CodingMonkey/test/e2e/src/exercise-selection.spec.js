import {PageObjectCategories} from './page-objects/categories.po.js';
import {PageObjectExercises} from './page-objects/exercises.po.js';
import {PageObjectEditor} from './page-objects/editor.po.js';
import {PageObjectSkeleton} from './page-objects/skeleton.po.js';

describe('Coding Monkey', function() {
  let poCategories;
  let poExercises;
  let poSkeleton;
  let poEditor;

  beforeEach(() => {
    poSkeleton = new PageObjectSkeleton();
    poCategories = new PageObjectCategories();
    poExercises = new PageObjectExercises();
    poEditor = new PageObjectEditor();

    browser.loadAndWaitForAureliaPage('http://localhost:5000/#/categories');
  });

  it('should load the page and display the initial page title', () => {
    expect(poSkeleton.getCurrentPageTitle()).toBe('Exercise Categories | Coding Monkey');
  });

  it('should display some categories', () => {
      expect(poCategories.getCategoriesDisplayedCount()).toBeGreaterThan(0);
  });

  it('should show the code editor when a category and exercise have been selected', () => {
      let categoryIdToTest = 'string_manipulation';
      let categoryTitle = 'String Manipulation';

      expect(poCategories.getCatgeoryTitle(categoryIdToTest)).toEqual(categoryTitle);
      poCategories.pressSelectCategoryButton(categoryIdToTest);

      browser.waitForRouterComplete();

      expect(poSkeleton.getCurrentPageTitle()).toBe('Exercises in Category | Coding Monkey');
      expect(poExercises.getExercisesDisplayedCount()).toBeGreaterThan(0);

      let exerciseIdToTest = "get_first_letter_of_a_string";
      let exerciseTitle = "Get First Letter of a String";

      expect(poExercises.getExerciseTitle(exerciseIdToTest)).toEqual(exerciseTitle);
      poExercises.pressSelectExerciseButton(exerciseIdToTest);

      browser.waitForRouterComplete();

      expect(poSkeleton.getCurrentPageTitle()).toBe('Code Editor | Coding Monkey');

  });
});
