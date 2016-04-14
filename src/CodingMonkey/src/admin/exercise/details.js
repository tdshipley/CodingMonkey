import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient)
export class details {
        constructor(http) {
        var loc = window.location;
        
        this.heading = "Exercise Details";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        http.configure(config => {
           config.useStandardConfiguration()
           .withBaseUrl(this.baseUrl + '/api/Exercise/');
        });
        
        this.http = http;
        
        this.vm = {
            exercise: {
                name: "",
                guidance: "",
                categoryids: []
            },
            exerciseTemplate: {
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
              this.vm.exercise.name = data.Name;
              this.vm.exercise.guidance = data.Guidance;
              this.vm.exercise.categoryids = data.CategoryIds;
          })
          .then(() => {
              this.getExerciseTemplate(params.id, params.exerciseTemplateId)
          })
          .then(() => {
              this.getExerciseCategoriesForExercise(this.vm.exercise.categoryids);
          })
          .catch(err => {
              console.log(err);
          });
    }
    
    getExerciseTemplate(exerciseId, exerciseTemplateId) {
        this.http.baseUrl = this.baseUrl + "/api/Exercise/" + exerciseId + "/ExerciseTemplate/";
        
        this.http.fetch('details/' + exerciseTemplateId)
          .then(response => response.json())
          .then(data => {
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