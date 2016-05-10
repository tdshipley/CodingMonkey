import {PageObjectWelcome} from './page-objects/welcome.po.js';
import {PageObjectSkeleton} from './page-objects/skeleton.po.js';

describe('Coding Monkey', function() {
  let poWelcome;
  let poSkeleton;

  beforeEach(() => {
    poSkeleton = new PageObjectSkeleton();
    poWelcome = new PageObjectWelcome();

    browser.loadAndWaitForAureliaPage('http://localhost:5000');
  });

  it('should load the page and display the initial page title', () => {
    expect(poSkeleton.getCurrentPageTitle()).toBe('Welcome | Coding Monkey');
  });

  it('should display greeting', () => {
      expect(poWelcome.getGreeting()).toBe('Coding Monkey');
  });

  it('should navigate to exercise categories page', () => {
    poSkeleton.navigateTo('#/categories');
    expect(poSkeleton.getCurrentPageTitle()).toBe('Exercise Categories | Coding Monkey');
  });
});
