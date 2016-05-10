'use strict';

Object.defineProperty(exports, '__esModule', {
  value: true
});

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

var PageObjectWelcome = (function () {
  function PageObjectWelcome() {
    _classCallCheck(this, PageObjectWelcome);
  }

  _createClass(PageObjectWelcome, [{
    key: 'getGreeting',
    value: function getGreeting() {
      return element(by.tagName('h1')).getText();
    }
  }, {
    key: 'openAlertDialog',
    value: function openAlertDialog() {
      var _this = this;

      return browser.wait(function () {
        _this.pressSubmitButton();

        return browser.switchTo().alert().then(
        // use alert.accept instead of alert.dismiss which results in a browser crash
        function (alert) {
          alert.accept();return true;
        }, function () {
          return false;
        });
      });
    }
  }]);

  return PageObjectWelcome;
})();

exports.PageObjectWelcome = PageObjectWelcome;