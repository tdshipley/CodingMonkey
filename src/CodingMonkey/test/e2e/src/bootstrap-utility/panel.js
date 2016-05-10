export class Panel {
    constructor() {
        
    }

    getTitle(panelId) {
        return element(by.css('#' + panelId + ' .panel-title')).getText();
    }

    getPanelCount() {
        return element.all(by.css('.panel')).count();
    }

    clickPanelButton(panelId) {
        element(by.css('#' + panelId + ' button')).click();
    }
}