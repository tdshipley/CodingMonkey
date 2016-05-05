import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class create {
    constructor(http, router) {
        this.appRouter = router;
        // Check there is a logged in user or kick them to homepage
        var currentUser = this.getCurrentUserInSessionStorage();

        if (!currentUser.isLoggedIn) {
            this.appRouter.navigate("/");
        }

        var loc = window.location;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        this.heading = "Update Exercise Test";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
        http.configure(config => {
           config.useStandardConfiguration();
        });
        
        this.http = http;
        
        this.vm = {
            test: {
                id: 0,
                description: "",
                testOutput: {},
                testInputs: []
            },
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
            }
        }
    }
    
    activate(params) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/' + params.exerciseId + '/Test/';
        
        this.http.fetch('details/' + params.id)
          .then(response => response.json())
          .then(data => {
              this.vm.test.id = data.Id
              this.vm.test.description = data.Description;
              this.vm.test.testOutput = {
                    id: data.TestOutput.Id,
                    valueType: data.TestOutput.ValueType,
                    value: data.TestOutput.Value
              }
              
              for (let testInput of data.TestInputs) {
                  let testInputToShow = {
                      id: testInput.Id,
                      argumentName: testInput.ArgumentName,
                      valueType: testInput.ValueType,
                      value: testInput.Value
                  }
                  
                  this.vm.test.testInputs.push(testInputToShow);
              }
          })
          .then(() => {
              this.getExercise(params.exerciseId);
          })
          .then(() => {
              this.getExerciseTemplate(params.exerciseId);
          })
          .catch(err => {
                this.notify.error("Failed to get test.");
            });
          
          this.valueTypeList = ["Boolean", "Integer", "String"];
    }
    
    getExercise(exerciseId) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/';
        
        this.http.fetch('details/' + exerciseId)
          .then(response => response.json())
          .then(data => {
              this.vm.exercise.id = data.Id
              this.vm.exercise.name = data.Name;
              this.vm.exercise.guidance = data.Guidance;
              this.vm.exercise.categoryids = data.CategoryIds;
          })
          .catch(err => {
              this.notify.error("Failed to get Exercise for Exercise Test.")
          })
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
              this.notify.error("Failed to get Exercise Template for Exercise Test.")
          });
          
          this.heading = "Create Test for Exercise: " + this.vm.exercise.name;
    }
    
    updateTest() {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/' + this.vm.exercise.id + '/Test/';
        
        let testInputsToUpdate = [];
        
        for(let testInput of this.vm.test.testInputs) {
            testInputsToUpdate.push({
                Id: testInput.id,
                ArgumentName: testInput.argumentName,
                ValueType: testInput.valueType,
                Value: testInput.value
            });
        }

        this.http.fetch('update/' + this.vm.test.id, {
                method: 'post',
                body: json({
                    Id: this.vm.test.id,
                    Description: this.vm.test.description,
                    TestOutput: {
                        Id: this.vm.test.testOutput.id,
                        Value: this.vm.test.testOutput.value,
                        ValueType: this.vm.test.testOutput.valueType
                    },
                    TestInputs: testInputsToUpdate
                })
            })
            .then(response => response.json())
            .then(data => {
                if (data.updated === false) {
                    this.notify.error("Failed update Exercise Test '" + this.vm.test.id + "'.");
                } else {
                    this.notify.success("Updated Exercise Test '" + this.vm.test.id + "'");
                    this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id + "/tests");
                }
            })
            .catch(err => {
                this.notify.error("Failed update Exercise Test '" + this.vm.test.id + "'.");
            });
    }
    
    
    addTestInput() {      
        this.vm.test.testInputs.push({
            argumentName: "",
            valueType: "",
            value: ""
        });
        return false;
    }
    
    removeTestInput(index) {
        if(index > -1) {
            this.vm.test.testInputs.splice(index, 1);
        }
        return false;
    }
    
    goToTestList() {
        this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id + "/tests");
    }

    getCurrentUserInSessionStorage() {
        let currentUserRaw = sessionStorage.getItem("currentUser");
        let currentUser = JSON.parse(currentUserRaw);

        return currentUser;
    }
}