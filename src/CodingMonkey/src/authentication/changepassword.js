import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class changePassword {
    constructor(http, router) {
        this.appRouter = router;
        // Check there is a logged in user or kick them to homepage
        var currentUser = this.getCurrentUserInSessionStorage();

        if (!currentUser.isLoggedIn) {
            this.appRouter.navigate("/");
        }

        var loc = window.location;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        this.heading = "Change Password";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
        http.configure(config => {
            config.useStandardConfiguration();
        });
        
        this.http = http;

        this.vm = {
            currentPassword: "",
            newPassword: "",
            newPasswordConfirmation: "",
            failureReasons: []
        }
    }

    changePassword() {
        this.http.baseUrl = this.baseUrl + '/api/Account/';

        this.http.fetch('changepassword', {
            method: 'post',
            body: json({
                CurrentPassword: this.vm.currentPassword,
                NewPassword: this.vm.newPassword,
                NewPasswordConfirmation: this.vm.newPasswordConfirmation
            })
        })
        .then(response => response.json())
        .then(data => {
            this.vm.failureReasons = [];

            if (data.PasswordChangeSuccessful) {
                this.appRouter.navigate("admin/exercises");
                this.notify.success("Password changed.");
                return 0;
            } else if (data.ChangeFailureReason === 100) {
                this.vm.failureReasons.push("New Password and New Password Confirmation do not match");
            } else {
                for (let error of data.ChangePasswordErrors) {
                    this.vm.failureReasons.push(error.Description);
                }
            }

            this.notify.error("Failed to change password. Please review errors and try again.");
            })
        .catch(err => {
            this.notify.error("Change Password failed.");
        });
    }

    getCurrentUserInSessionStorage() {
        let currentUserRaw = sessionStorage.getItem("currentUser");
        let currentUser = JSON.parse(currentUserRaw);

        return currentUser;
    }
}