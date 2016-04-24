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
        this.heading = "Exercise Categories";
        this.baseUrl = loc.protocol + "//" + loc.host;

        http.configure(config => {
            config.useStandardConfiguration()
                .withBaseUrl(this.baseUrl + '/api/ExerciseCategory/');
        });

        this.http = http;

        this.exerciseCategoryList = [];
    }

    activate() {
        this.http.fetch('list')
          .then(response => response.json())
          .then(data => {
              for (let exerciseCatgeory of data) {
                  var vm = {
                      id: exerciseCatgeory.Id,
                      name: exerciseCatgeory.Name,
                      description: exerciseCatgeory.Description,
                      exerciseids: exerciseCatgeory.ExerciseIds
                  };

                  this.exerciseCategoryList.push(vm);
              }
            })
          .catch(err => {
              this.notify.error("Failed to get exercise categories")
          });
    }
    
    delete(exerciseCategoryName, exerciseCategoryId)
    {
        var question = "Are you sure you want to delete Exercise Category '" + exerciseCategoryName + "'? " + 
            "This will also remove the Category from existing Exercises";
        var questionHeader = "Confirm Delete Exercise Category";
        
        var dialogPromptModel = {
            question: question,
            questionHeader: questionHeader,
            dismissText: "Cancel",
            confirmText: "Delete"
        };
        
        this.dialogService.open({ viewModel: DialogPrompt, model: dialogPromptModel}).then(response => {
            if (!response.wasCancelled) {
                this.http.fetch('delete/' + exerciseCategoryId, {
                    method: "delete"
                })
                .then(response => response.json())
                .then(data => {
                    if(!data.deleted)
                    {
                        this.notify.error("Deleting Exercise Category '" + exerciseCategoryName + "' failed.");
                    }
                    else
                    {
                        // Removing Exercise Category from list
                        this.exerciseCategoryList = this.exerciseCategoryList.filter(function( exerciseCatgeory ) {
                            return exerciseCatgeory.id !== exerciseCategoryId;
                        });
                        
                        this.notify.success("Deleting Exercise Category '" + exerciseCategoryName + "' was successful.");
                    }
                });
            }
        });
    }
}