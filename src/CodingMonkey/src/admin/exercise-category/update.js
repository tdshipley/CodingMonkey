import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class update {
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
        
        this.heading = "Update Exercise Category";

        this.baseUrl = loc.protocol + "//" + loc.host;
        
        http.configure(config => {
           config.useStandardConfiguration()
           .withBaseUrl(this.baseUrl + '/api/ExerciseCategory/');
        });
        
        this.http = http;
        
        this.vm = {
            exerciseCategory: {
                id: 0,
                name: "",
                description: "",
                exerciseids: []
            }
        };
    }
    
    activate(params) {
        this.http.fetch('details/' + params.id)
          .then(response => response.json())
          .then(data => {
              this.vm.exerciseCategory.id = data.Id;
              this.vm.exerciseCategory.name = data.Name;
              this.vm.exerciseCategory.description = data.Guidance;
              this.vm.exerciseCategory.exerciseids = data.ExerciseIds;
              
              this.heading = "Update Exercise Category: " + this.vm.exerciseCategory.name;
          })
          .catch(err => {
              this.notify.error("Failed to get Exercise Category.")
          });
    }
    
    updateExerciseCategory() {        
        this.http.fetch('update/' + this.vm.exerciseCategory.id, {
            method: 'post',
            body: json({
                Name: this.vm.exerciseCategory.name,
                Description: this.vm.exerciseCategory.description,
                ExerciseIds: this.vm.exerciseCategory.exerciseids
            })
        })
        .then(response => response.json())
        .then(data => {
            this.notify.success("Updated Exercise Category '" + this.vm.exerciseCategory.name + "'");
            this.appRouter.navigate("admin/exercise/categories/" + this.vm.exerciseCategory.id);
        })
        .catch(err => {
            this.notify.error("Update Exercise Category failed.")
        })
        
    }

    getCurrentUserInSessionStorage() {
        let currentUserRaw = sessionStorage.getItem("currentUser");
        let currentUser = JSON.parse(currentUserRaw);

        return currentUser;
    }
}