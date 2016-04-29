import {Router} from 'aurelia-router';
import {inject} from 'aurelia-framework';

@inject(Router)
export class Welcome {
    constructor(router) {
        this.appRouter = router;
        this.heading = 'Coding Monkey';
    }

    navigateToCategories() {
        this.appRouter.navigate("categories");
    }
}