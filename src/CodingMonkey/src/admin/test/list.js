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
        this.heading = "Tests";
        this.baseUrl = loc.protocol + "//" + loc.host;

        http.configure(config => {
            config.useStandardConfiguration();
        });

        this.http = http;

        this.testList = [];
        
        this.exerciseId = 0;
        this.exerciseTemplateId = 0;
    }

    activate(params) {
        this.exerciseId = params.exerciseId;
        this.exerciseTemplateId = params.exerciseTemplateId;
        
        this.http.baseUrl = this.baseUrl + '/api/Exercise/' + params.exerciseId + '/Test/';
        
        this.http.fetch('list')
          .then(response => response.json())
          .then(data => {
              for (let test of data) {
                  var vm = {
                    id: test.Id,
                    description: test.Description,
                    testOutput: {
                        id: test.TestOutput.Id,
                        valueType: test.TestOutput.ValueType,
                        value: test.TestOutput.Value
                    },
                    testInputs: []
                };
                
                for (let testInput of test.TestInputs) {
                    vm.testInputs.push({
                        id: testInput.Id,
                        argumentName: testInput.ArgumentName,
                        valueType: testInput.ValueType,
                        value: testInput.Value
                    })
                }

                this.testList.push(vm);
              }
            })
          .catch(err => {
              console.log(err);
              this.notify.error("Failed to get tests.")
          });
    }
    
    delete(testId)
    {
        var question = "Are you sure you want to delete Test with id '" + testId + "'? "
        var questionHeader = "Confirm Delete Test";
        
        var dialogPromptModel = {
            question: question,
            questionHeader: questionHeader,
            dismissText: "Cancel",
            confirmText: "Delete"
        };
        
        this.dialogService.open({ viewModel: DialogPrompt, model: dialogPromptModel}).then(response => {
            if (!response.wasCancelled) {
                this.http.fetch('delete/' + testId, {
                    method: "delete"
                })
                .then(response => response.json())
                .then(data => {
                    if(!data.deleted)
                    {
                        this.notify.error("Deleting Test '" + testId + "' failed.");
                    }
                    else
                    {
                        // Removing Test from list
                        this.testList = this.testList.filter(function( test ) {
                            return test.id !== testId;
                        });
                        
                        this.notify.success("Deleting Test with Id '" + testId + "' was successful.");
                    }
                });
            }
        });
    }
}