'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

var Panel = (function () {
    function Panel() {
        _classCallCheck(this, Panel);
    }

    _createClass(Panel, [{
        key: 'getTitle',
        value: function getTitle(panelId) {
            return element(by.css('#' + panelId + ' .panel-title')).getText();
        }
    }, {
        key: 'getPanelCount',
        value: function getPanelCount() {
            return element.all(by.css('.panel')).count();
        }
    }, {
        key: 'clickPanelButton',
        value: function clickPanelButton(panelId) {
            element(by.css('#' + panelId + ' button')).click();
        }
    }]);

    return Panel;
})();

exports.Panel = Panel;