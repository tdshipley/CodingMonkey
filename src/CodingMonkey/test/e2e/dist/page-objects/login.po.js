'use strict';

Object.defineProperty(exports, '__esModule', {
  value: true
});

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

var PageObjectLogin = (function () {
  function PageObjectLogin() {
    _classCallCheck(this, PageObjectLogin);
  }

  _createClass(PageObjectLogin, [{
    key: 'setUsername',
    value: function setUsername(username) {
      element(by.id('username')).sendKeys(username);
    }
  }, {
    key: 'setPassword',
    value: function setPassword(password) {
      element(by.id('password')).sendKeys(password);
    }
  }, {
    key: 'pressLoginButton',
    value: function pressLoginButton() {
      element(by.id('login')).click();
    }
  }]);

  return PageObjectLogin;
})();

exports.PageObjectLogin = PageObjectLogin;