import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';
import {DialogService} from 'aurelia-dialog';
import {DialogPrompt} from '../../dialog-prompt';
import toastr from 'toastr';

@inject(HttpClient, DialogService)
export class list {
    constructor(http, dialogService) {
        this.dialogService = dialogService;

        this.notify = toastr;
        this.notify.options.progressBar = true;
        
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
            })
          .catch(err => {
              this.notify.error("Failed to get exercises.")
          });
    }
    
    delete(exerciseName, exerciseId)
    {
        var question = "Are you sure you want to delete exercise '" + exerciseName + "'?";
        var questionHeader = "Confirm Delete Exercise";
        
        var dialogPromptModel = {
            question: question,
            questionHeader: questionHeader,
            dismissText: "Cancel",
            confirmText: "Delete"
        };
        
        this.dialogService.open({ viewModel: DialogPrompt, model: dialogPromptModel}).then(response => {
            if (!response.wasCancelled) {
                this.http.fetch('delete/' + exerciseId, {
                    method: "delete"
                })
                .then(response => response.json())
                .then(data => {
                    if(!data.deleted)
                    {
                        this.notify.error("Deleting Exercise '" + exerciseName + "' failed.");
                    }
                    else
                    {
                        this.exerciseList = this.exerciseList.filter(function( exercise ) {
                            return exercise.id !== exerciseId;
                        });
                        
                        this.notify.success("Deleting Exercise '" + exerciseName + "' was successful.");
                    }
                });
            }
        });
    }
}