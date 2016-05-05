import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';
import {Router} from 'aurelia-router';
import {DialogService} from 'aurelia-dialog';
import {DialogPrompt} from '../../dialog-prompt';
import toastr from 'toastr';

@inject(HttpClient, DialogService, Router)
export class list {
    constructor(http, dialogService, router) {
        this.appRouter = router;
        // Check there is a logged in user or kick them to homepage
        var currentUser = this.getCurrentUserInSessionStorage();

        if (!currentUser.isLoggedIn) {
            this.appRouter.navigate("/");
        }

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
    }

    activate(params) {
        this.exerciseId = params.exerciseId;
        
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
                this.notify.error("Failed to get tests.");
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
    
    goToCreateTest() {
        this.appRouter.navigate("admin/exercise/" + this.exerciseId + "/test/create");
    }
    
    goToExercise() {
        this.appRouter.navigate("admin/exercise/" + this.exerciseId);
    }

    getCurrentUserInSessionStorage() {
        let currentUserRaw = sessionStorage.getItem("currentUser");
        let currentUser = JSON.parse(currentUserRaw);

        return currentUser;
    }
}