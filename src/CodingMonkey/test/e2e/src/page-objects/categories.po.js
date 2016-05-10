export class PageObjectCategories {

  constructor() {

  }

  pressSelectCategoryButton(categoryPanelId) {
      element(by.css('#' + categoryPanelId + ' button')).click();
  }

  getCatgeoryTitle(categoryPanelId) {
      return element(by.css('#' + categoryPanelId + ' .panel-title')).getText();
  }

  getCategoriesDisplayedCount() {
      return element.all(by.css('.panel')).count();
  }
}
