import {Panel} from './../bootstrap-utility/panel.js';

export class PageObjectExercises {

    constructor() {
        this.panelHelper = new Panel();
    }

    pressSelectExerciseButton(exercisePanelId) {
        this.panelHelper.clickPanelButton(exercisePanelId);
    }

    getExerciseTitle(exercisePanelId) {
        return this.panelHelper.getTitle(exercisePanelId);
    }

    getExercisesDisplayedCount() {
        return this.panelHelper.getPanelCount();
    }
}