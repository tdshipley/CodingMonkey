'use strict';

Object.defineProperty(exports, '__esModule', {
  value: true
});

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

var PageObjectCategories = (function () {
  function PageObjectCategories() {
    _classCallCheck(this, PageObjectCategories);
  }

  _createClass(PageObjectCategories, [{
    key: 'pressSelectCategoryButton',
    value: function pressSelectCategoryButton(categoryPanelId) {
      element(by.id(categoryPanelId)).click();
    }
  }, {
    key: 'getCatgeoryTitle',
    value: function getCatgeoryTitle(categoryPanelId) {
      return element(by.css('#' + categoryPanelId + ' > panel-title')).getText();
    }
  }, {
    key: 'getCategoriesDisplayedCount',
    value: function getCategoriesDisplayedCount() {
      return element.all(by.css('panel')).count();
    }
  }]);

  return PageObjectCategories;
})();

exports.PageObjectCategories = PageObjectCategories;