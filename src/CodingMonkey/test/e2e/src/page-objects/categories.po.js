import {Panel} from './../bootstrap-utility/panel.js';

export class PageObjectCategories {

  constructor() {
      this.panelHelper = new Panel();
  }

  pressSelectCategoryButton(categoryPanelId) {
      this.panelHelper.clickPanelButton(categoryPanelId);
  }

  getCatgeoryTitle(categoryPanelId) {
      return this.panelHelper.getTitle(categoryPanelId);
  }

  getCategoriesDisplayedCount() {
      return this.panelHelper.getPanelCount();
  }
}
