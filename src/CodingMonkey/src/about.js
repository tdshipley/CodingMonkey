import {Router} from 'aurelia-router';
import {inject} from 'aurelia-framework';

@inject(Router)
export class About {
    constructor(router) {
        this.appRouter = router;
        this.heading = 'About Coding Monkey';
    }
}