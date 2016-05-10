'use strict';

var _pageObjectsWelcomePoJs = require('./page-objects/welcome.po.js');

var _pageObjectsSkeletonPoJs = require('./page-objects/skeleton.po.js');

describe('Coding Monkey', function () {
  var poWelcome = undefined;
  var poSkeleton = undefined;

  beforeEach(function () {
    poSkeleton = new _pageObjectsSkeletonPoJs.PageObjectSkeleton();
    poWelcome = new _pageObjectsWelcomePoJs.PageObjectWelcome();

    browser.loadAndWaitForAureliaPage('http://localhost:5000');
  });

  it('should load the page and display the initial page title', function () {
    expect(poSkeleton.getCurrentPageTitle()).toBe('Welcome | Coding Monkey');
  });

  it('should display greeting', function () {
    expect(poWelcome.getGreeting()).toBe('Coding Monkey');
  });

  it('should navigate to exercise categories page', function () {
    poSkeleton.navigateTo('#/categories');
    expect(poSkeleton.getCurrentPageTitle()).toBe('Exercise Categories | Coding Monkey');
  });
});