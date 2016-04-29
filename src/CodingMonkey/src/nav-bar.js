import {inject} from 'aurelia-framework';
import {bindable} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router'
import 'fetch';

@inject(HttpClient, Router)
export class NavBar {
    @bindable router = null;

    constructor(http, router) {
        var loc = window.location;
        this.baseUrl = loc.protocol + "//" + loc.host;

        http.configure(config => {
            config.useStandardConfiguration();
        });
        
        this.http = http;
        this.router = router;

        this.hasCategories = false;
        this.vm = {
            user: {
                isLoggedIn: false,
                username: "",
                roles: []
            }
        };
    }

    attached() {
        this.getCurrentUser();
        sessionStorage.setItem("currentUser", this.vm.user);
    }

    detached() {
        this.getCurrentUser();
        sessionStorage.removeItem("currentUser");
    }

    getCurrentUser() {
        this.http.baseUrl = this.baseUrl + '/api/Account/';

        this.http.fetch('CurrentUser')
            .then(response => response.json())
            .then(data => {
                if (data.GetUserSucceeded) {
                    this.vm.user.isLoggedIn = true;
                    this.vm.user.username = data.username;
                    this.vm.user.roles = data.roles;
                }
            })
            .catch(err => {
                this.vm.user.isLoggedIn = false;
                this.vm.user.username = "";
                this.vm.user.roles = [];
            });
    }

    logout() {
        this.router.navigate("logout");
    }
}