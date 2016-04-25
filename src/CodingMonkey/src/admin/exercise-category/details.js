import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class details {
        constructor(http, router) {
        var loc = window.location;
        
        this.heading = "Exercise Category Details";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
        this.appRouter = router;
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        http.configure(config => {
           config.useStandardConfiguration()
           .withBaseUrl(this.baseUrl + '/api/ExerciseCategory/');
        });
        
        this.http = http;
        
        this.hasExercises = false;
        this.vm = {
            exerciseCategory: {
                id: 0,
                name: "",
                description: "",
                exerciseids: []
            },
            exercises: []
        };
    }
    
    activate(params) {
        this.http.fetch('details/' + params.id)
          .then(response => response.json())
          .then(data => {
              this.vm.exerciseCategory.id = data.Id;
              this.vm.exerciseCategory.name = data.Name;
              this.vm.exerciseCategory.description = data.Description;
              this.vm.exerciseCategory.exerciseids = data.ExerciseIds;
          })
          .then(() => {
              this.getExercisesForExerciseCategory(this.vm.exerciseCategory.exerciseids);
          })
          .catch(err => {
              this.notify.error('Failed to get exercise.')
          });
    }
    
    getExercisesForExerciseCategory(exerciseIds) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/'
        
        if(exerciseIds.count !== 0) {
            this.hasExercises = true;
            
            for(let exerciseId of exerciseIds) {

            this.http.fetch('details/' + exerciseId)
                .then(response => response.json())
                .then(data => {
                    let exercise = {
                        name: data.Name,
                        guidance: data.Guidance
                    };
                    
                    this.vm.exercises.push(exercise);
                })
                .catch(err => {
                    this.notify.error("Failed to get Exercises for Exercise Category.");
                })
            }
        }
    }
    
    goToExerciseCategoryList() {
        this.appRouter.navigate("admin/exercise/categories");
    }
}