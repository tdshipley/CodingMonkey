import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class login {
    constructor(http, router) {
        var loc = window.location;

        this.notify = toastr;
        this.notify.options.progressBar = true;

        this.baseUrl = loc.protocol + "//" + loc.host;
        this.appRouter = router;

        http.configure(config => {
            config.useStandardConfiguration();
        });

        this.http = http;

        this.vm = {
            username: "",
            password: ""
        }
    }

    login() {
        this.http.baseUrl = this.baseUrl + '/api/Authentication/';

        this.http.fetch('login', {
            method: 'post',
            body: json({
                Username: this.vm.username,
                Password: this.vm.password
            })
        })
        .then(response => response.json())
        .then(data => {
            if (data.LoginSucceeded === true) {
                this.notify.remove();
                location.reload();
                this.appRouter.navigate("admin/exercises");
            } else {
                this.notify.error("Your Username or Password was incorrect.");
            }
        })
        .catch(err => {
            this.notify.error("There was a problem logging you in.");
        });
    }
}