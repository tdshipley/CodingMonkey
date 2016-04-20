import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class create {
    constructor(http, router) {
        var loc = window.location;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        this.heading = "Create Exercise Test";
        this.baseUrl = loc.protocol + "//" + loc.host;
        this.appRouter = router;
        
        http.configure(config => {
           config.useStandardConfiguration();
        });
        
        this.http = http;

        this.vm = {
            test: {
                id: 0,
                description: "",
                testOutput: {
                    id: 0,
                    valueType: "",
                    value: ""
                },
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
        };
    }
    
    activate(params) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/';
        
        this.http.fetch('details/' + params.exerciseId)
          .then(response => response.json())
          .then(data => {
              this.vm.exercise.id = data.Id
              this.vm.exercise.name = data.Name;
              this.vm.exercise.guidance = data.Guidance;
              this.vm.exercise.categoryids = data.CategoryIds;
          })
          .then(() => {
              this.getExerciseTemplate(params.exerciseId, params.exerciseTemplateId)
          })
          .catch(err => {
              this.notify.error("Failed to get Exercise and Exercise Template")
          });
          
          this.valueTypeList = ["Boolean", "Integer", "String"];
    }
    
    getExerciseTemplate(exerciseId, exerciseTemplateId) {
        this.http.baseUrl = this.baseUrl + "/api/Exercise/" + exerciseId + "/ExerciseTemplate/";
        
        this.http.fetch('details/' + exerciseTemplateId)
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
          
          this.heading = "Create Test for Exercise: " + this.vm.exercise.name;
    }
    
    createTest() {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/' + this.vm.exercise.id + '/Test';
        
        let testOutputToCreate = {
            ValueType: this.vm.test.valueType,
            Value: this.vm.test.value
        }
        
        let testInputToCreate = []
        
        for (let testInput of this.vm.test.testInputs) {
            this.testInputToCreate.push({
                ArgumentName: testInput.argumentName,
                ValueType: testInput.valueType,
                Value: testInput.value
            })
        }
        
        this.http.fetch('create', {
            method: 'post',
            body: json({
                Description: this.vm.test.description,
                TestInputs: testInputsToCreate,
                TestOutput: testOutputToCreate
            })
        })
        .then(response => response.json())
        .then(data => {
            this.vm.test.id = data.Id;
            this.vm.test.testInputs = data.TestInputs;
            this.vm.test.testOutput = data.TestOutput;
        })
        .catch(err => {
            this.notify.error("Create Exercise Test failed.")
        });
    }
    
    addTestInput() {      
        this.vm.test.testInputs.push({
            argumentName: "",
            valueType: "",
            value: ""
        });
    }
}