import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class categories {
    constructor(http, router) {
        this.heading = "Select a Category";
        
        var loc = window.location;
        this.baseUrl = loc.protocol + "//" + loc.host;
        this.appRouter = router;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        http.configure(config => {
           config.useStandardConfiguration()
           .withBaseUrl(this.baseUrl + '/api/ExerciseCategory/');
        });
        
        this.http = http;

        this.vm = {
            categories: [],
            pageLoading: true
        };
    }
    
    activate() {
        this.http.fetch('list')
          .then(response => response.json())
          .then(data => {
              for (let exerciseCategory of data) {
                  var vm = {
                      id: exerciseCategory.Id,
                      name: exerciseCategory.Name,
                      description: exerciseCategory.Description
                  };

                  this.vm.categories.push(vm);
              }

                this.vm.pageLoading = false;
            })
          .catch(err => {
                this.notify.error("Failed to get exercise categories.");
            });
    }
    
    goToExercisesInCategory(categoryId) {
        this.appRouter.navigate("category/" + categoryId + "/exercises");
    }
}