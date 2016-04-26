import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class details {
        constructor(http, router) {
        var loc = window.location;
        
        this.heading = "Exercise Details";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
        this.appRouter = router;
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        http.configure(config => {
           config.useStandardConfiguration()
           .withBaseUrl(this.baseUrl + '/api/Exercise/');
        });
        
        this.http = http;
        
        this.hasCategories = false;
        this.vm = {
            exercise: {
                id: 0,
                name: "",
                guidance: "",
                categoryids: []
            },
            exerciseTemplate: {
                id: 0,
                initialCode: "",
                className: "",
                mainMethodName: ""
            },
            exerciseCategories: []
        };
    }
    
    activate(params) {
        this.http.fetch('details/' + params.id)
          .then(response => response.json())
          .then(data => {
              this.vm.exercise.id = data.Id;
              this.vm.exercise.name = data.Name;
              this.vm.exercise.guidance = data.Guidance;
              this.vm.exercise.categoryids = data.CategoryIds;
          })
          .then(() => {
              this.getExerciseTemplate(params.id)
          })
          .then(() => {
              this.getExerciseCategoriesForExercise(this.vm.exercise.categoryids);
          })
          .catch(err => {
              this.notify.error("Failed to get exercise.")
          });
    }
    
    getExerciseTemplate(exerciseId) {
        this.http.baseUrl = this.baseUrl + "/api/Exercise/" + exerciseId + "/ExerciseTemplate/";
        
        this.http.fetch('details')
          .then(response => response.json())
          .then(data => {
              this.vm.exerciseTemplate.id = data.Id;
              this.vm.exerciseTemplate.initialCode = data.InitialCode;
              this.vm.exerciseTemplate.className = data.ClassName;
              this.vm.exerciseTemplate.mainMethodName = data.MainMethodName;
          })
          .catch(err => {
              this.notify.error("Failed to get Exercise Template for Exercise.")
          });
    }
    
    getExerciseCategoriesForExercise(categoryIds) {
        this.http.baseUrl = this.baseUrl + '/api/ExerciseCategory/'
        
        if(categoryIds.count !== 0) {
            this.hasCategories = true;
            
            for(let categoryId of categoryIds) {

            this.http.fetch('details/' + categoryId)
                .then(response => response.json())
                .then(data => {
                    let category = {
                        name: data.Name,
                        description: data.Description
                    };
                    
                    this.vm.exerciseCategories.push(category);
                })
                .catch(err => {
                    this.notify.error("Failed to get Categories for Exercise.");
                })
            }
        }
    }
    
    goToTestList() {
        this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id + "/tests");
    }
    
    goToExerciseList() {
        this.appRouter.navigate("admin/exercises");
    }
}