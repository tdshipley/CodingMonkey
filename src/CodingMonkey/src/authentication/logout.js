import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inlineView("<template></template>")
@inject(HttpClient, Router)
export class logout {
    constructor(http, router) {
        var loc = window.location;

        this.notify = toastr;
        this.notify.options.progressBar = true;

        this.baseUrl = loc.protocol + "//" + loc.host;
        this.appRouter = router;

        http.configure(config => {
            config.useStandardConfiguration()
                .withBaseUrl(this.baseUrl + '/api/Authentication/');
        });

        this.http = http;

        let logoutErrorMessage = "There was a problem logging you out. Try clearing your cache.";

        this.http.fetch('logout', {
            method: 'post'
        })
        .then(response => response.json())
        .then(data => {
            if (data.LogoutSucceeded === true) {
                sessionStorage.removeItem("currentUser");
                location.reload();
                this.appRouter.navigate("/");
            } else {
                this.notify.error(logoutErrorMessage);
            }
        })
        .catch(err => {
            this.notify.error(logoutErrorMessage);
        });
    }
}