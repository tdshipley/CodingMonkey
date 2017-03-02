import {Router} from 'aurelia-router';
import {HttpClient, json} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';
import 'fetch';

@inject(HttpClient, Router)
export class About {

    constructor(http, router) {
        this.appRouter = router;
        this.heading = 'About Coding Monkey';
        this.contributorUserProfiles = [];
        this.contributorUserNames = ["captainkirk854", "bretcolloff", "chreden", "jonparish", "no1melman"];

        http.configure(config => {
            config
                .useStandardConfiguration();
        });

        this.http = http;
    }

    activate() {
        this.getContributorProfiles();
    }

    getContributorProfiles() {
        this.http.fetch("../../json/contributors.json")
            .then(response => response.json())
            .then(contributors => {
                for (let contributor of contributors) {
                    contributor.displayName = "";
                    if (contributor.name === undefined) {
                        contributor.displayName = contributor.login;
                    } else {
                        contributor.displayName = contributor.name + " (" + contributor.login + ")";
                    }

                    this.contributorUserProfiles.push(contributor);
                }
            });
    }
}