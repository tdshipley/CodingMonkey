export class StringHelpers {
    constructor() {

    }

    convertTitleToPageId(title) {
        return title.replace(/\s+/g, '_').toLowerCase();
    }
}