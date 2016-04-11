import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';

@inject(HttpClient)
export class details {
    constructor(http) {
        var loc = window.location;


        this.heading = "Exercises";
        this.baseUrl = loc.protocol + "//" + loc.host;

        http.configure(config => {
            config.useStandardConfiguration()
                .withBaseUrl(this.baseUrl + '/api/Exercise/');
        });

        this.http = http;

        this.exerciseList = [];
    }

    activate() {
        this.http.fetch('list')
          .then(response => response.json())
          .then(data => {
              for (let exercise of data) {
                  var vm = {
                      id: exercise.Id,
                      exerciseTemplateId: exercise.ExerciseTemplateId,
                      name: exercise.Name,
                      guidance: exercise.Guidance,
                      categoryids: exercise.CategoryIds
                  };

                  this.exerciseList.push(vm);
              }

                console.log(data);
                console.log(this.exerciseList);
            })
          .catch(err => {
              console.log(err);
          });
    }
}