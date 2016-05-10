import {PageObjectCategories} from './page-objects/categories.po.js';
import {PageObjectSkeleton} from './page-objects/skeleton.po.js';

describe('Coding Monkey', function() {
  let poCategories;
  let poSkeleton;

  beforeEach(() => {
    poSkeleton = new PageObjectSkeleton();
    poCategories = new PageObjectCategories();

    browser.loadAndWaitForAureliaPage('http://localhost:5000/#/categories');
  });

  it('should load the page and display the initial page title', () => {
    expect(poSkeleton.getCurrentPageTitle()).toBe('Exercise Categories | Coding Monkey');
  });

  it('should display some categories', () => {
      expect(poCategories.getCategoriesDisplayedCount()).toBeGreaterThan(0);
  });

  it('should go to the exercise selection page when category has been selected', () => {
      let categoryIdToTest = 'string_manipulation';
      let categoryTitle = 'String Manipulation';

      expect(poCategories.getCatgeoryTitle(categoryIdToTest)).toEqual(categoryTitle);
      poCategories.pressSelectCategoryButton(categoryIdToTest);

      browser.waitForRouterComplete();

      expect(poSkeleton.getCurrentPageTitle()).toBe('Exercises in Category | Coding Monkey');
  });
});
