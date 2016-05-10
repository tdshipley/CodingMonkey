'use strict';

Object.defineProperty(exports, '__esModule', {
  value: true
});

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

var _bootstrapUtilityPanelJs = require('./../bootstrap-utility/panel.js');

var PageObjectCategories = (function () {
  function PageObjectCategories() {
    _classCallCheck(this, PageObjectCategories);

    this.panelHelper = new _bootstrapUtilityPanelJs.Panel();
  }

  _createClass(PageObjectCategories, [{
    key: 'pressSelectCategoryButton',
    value: function pressSelectCategoryButton(categoryPanelId) {
      this.panelHelper.clickPanelButton(categoryPanelId);
    }
  }, {
    key: 'getCatgeoryTitle',
    value: function getCatgeoryTitle(categoryPanelId) {
      return this.panelHelper.getTitle(categoryPanelId);
    }
  }, {
    key: 'getCategoriesDisplayedCount',
    value: function getCategoriesDisplayedCount() {
      return this.panelHelper.getPanelCount();
    }
  }]);

  return PageObjectCategories;
})();

exports.PageObjectCategories = PageObjectCategories;