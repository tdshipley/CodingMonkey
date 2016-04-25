import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';

@inject(HttpClient, Router)
export class exercises {
    constructor(http, router) {
        this.heading = "Select an Exercise";
        
        var loc = window.location;
        this.baseUrl = loc.protocol + "//" + loc.host;
        this.appRouter = router;
        
        http.configure(config => {
           config.useStandardConfiguration()
           .withBaseUrl(this.baseUrl + '/api/Exercise/');
        });
        
        this.http = http;

        this.vm = {
            exercises: []
        };
    }
    
    activate(params) {        
        this.http.fetch('list')
          .then(response => response.json())
          .then(data => {
                for (let exercise of data) {
                    if(exercise.CategoryIds.includes(Number(params.exerciseCategoryId))) {
                        var vm = {
                        id: exercise.Id,
                        exerciseTemplateId: exercise.ExerciseTemplateId,
                        name: exercise.Name,
                        guidance: exercise.Guidance,
                        categoryids: exercise.CategoryIds
                        };

                        this.vm.exercises.push(vm);
                    }
                }
                console.log(this.vm);
            })
          .catch(err => {
              this.notify.error("Failed to get exercises.")
          });
    }
}