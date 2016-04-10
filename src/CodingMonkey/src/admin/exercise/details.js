import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class details {
        constructor(http) {
        var loc = window.location;
        
        
        this.heading = "Exercise Details";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
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
            }
        };
    }
    
    activate(params) {
        this.http.fetch('details/' + params.id)
          .then(response => response.json())
          .then(data => {
              this.vm.exercise.name = data.Name;
              this.vm.exercise.guidance = data.Guidance;
              this.vm.exercise.categoryids = data.CategoryIds;
              
              console.log("Exercise Details")
              console.log(data);
          })
          .then(() => {
              this.getExerciseTemplate(params.id, params.exerciseTemplateId)
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
              
              console.log("Exercise Template Details")
              console.log(data);
          })
          .catch(err => {
              console.log(err);
          });
    }
}