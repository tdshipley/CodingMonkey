'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

var _bootstrapUtilityPanelJs = require('./../bootstrap-utility/panel.js');

var PageObjectExercises = (function () {
    function PageObjectExercises() {
        _classCallCheck(this, PageObjectExercises);

        this.panelHelper = new _bootstrapUtilityPanelJs.Panel();
    }

    _createClass(PageObjectExercises, [{
        key: 'pressSelectExerciseButton',
        value: function pressSelectExerciseButton(exercisePanelId) {
            this.panelHelper.clickPanelButton(exercisePanelId);
        }
    }, {
        key: 'getExerciseTitle',
        value: function getExerciseTitle(exercisePanelId) {
            return this.panelHelper.getTitle(exercisePanelId);
        }
    }, {
        key: 'getExercisesDisplayedCount',
        value: function getExercisesDisplayedCount() {
            return this.panelHelper.getPanelCount();
        }
    }]);

    return PageObjectExercises;
})();

exports.PageObjectExercises = PageObjectExercises;