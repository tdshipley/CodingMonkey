import {PageObjectCategories} from './page-objects/welcome.po.js';
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

  it('should display some categories'), () => {
      expect(poSkeleton.getCategoriesDisplayedCount()).toBeGreaterThan(0);
  });
});
