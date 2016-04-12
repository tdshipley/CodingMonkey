import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {DialogService} from 'aurelia-dialog';
import {DialogPrompt} from '../../dialog-prompt';
import 'fetch';

@inject(HttpClient, DialogService)
export class details {
    constructor(http, dialogService) {
        this.dialogService = dialogService;
        
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
    
    delete(exerciseName)
    {
        var question = "Are you sure you want to delete exercise '" + exerciseName + "' ?";
        var questionHeader = "Confirm Delete Exercise";
        
        var dialogPromptModel = {
            question: question,
            questionHeader: questionHeader
        };
        
        this.dialogService.open({ viewModel: DialogPrompt, model: dialogPromptModel}).then(response => {
        if (!response.wasCancelled) {
            console.log('good - ', response.output);
        } else {
            console.log('bad');
        }
        console.log(response.output);
        });
    }
}