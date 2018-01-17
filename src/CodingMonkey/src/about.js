import {Router} from 'aurelia-router';
import {HttpClient, json} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';
import 'fetch';

@inject(HttpClient, Router)
export class About {

    constructor(http, router) {
        this.appRouter = router;
        this.heading = 'About Coding Monkey';

        http.configure(config => {
            config
                .useStandardConfiguration();
        });

        this.http = http;
    }

    activate() {
    }
}