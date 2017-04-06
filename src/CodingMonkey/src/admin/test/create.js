import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';
import {Authentication} from './../../authentication/authentication.js';

@inject(HttpClient, Router)
export class create {
    constructor(http, router) {
        this.appRouter = router;

        // Check there is a logged in user or kick them to homepage
        new Authentication(this.appRouter).verifyUserLoggedIn();

        var loc = window.location;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        this.heading = "Create Exercise Test";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
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
                mainMethodName: "",
                mainMethodSignature: ""
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
              this.getExerciseTemplate(params.exerciseId);
          })
          .catch(err => {
              this.notify.error("Failed to get Exercise and Exercise Template");
          });
          
          this.valueTypeList = ["Boolean", "Integer", "String"];
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
              this.vm.exerciseTemplate.mainMethodSignature = data.MainMethodSignature;
          })
          .then(() => {
              this.createTestInputs(this.vm.exerciseTemplate.mainMethodSignature);
              this.createTestOutput(this.vm.exerciseTemplate.mainMethodSignature);
          })
          .catch(err => {
              this.notify.error("Failed to get Exercise Template for Exercise.")
          });
          
          this.heading = "Create Test for Exercise: " + this.vm.exercise.name;
    }
    
    createTest(addAnotherTest) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/' + this.vm.exercise.id + '/Test/';
        
        let testOutputToCreate = {
            ValueType: this.vm.test.testOutput.valueType,
            Value: this.vm.test.testOutput.value
        }
        
        var testInputsToCreate = [];
        
        for (let testInput of this.vm.test.testInputs) {
            testInputsToCreate.push({
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
            this.notify.success("Create Exercise Test succeeded.");
            if(addAnotherTest) {
                this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id + "/test/create");
                this.vm.test = {
                    id: 0,
                    description: "",
                    testOutput: {
                        id: 0,
                        valueType: "Boolean",
                        value: ""
                    },
                    testInputs: []
                };
            } else {
                this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id + "/tests");
            }
        })
        .catch(err => {
            this.notify.error("Create Exercise Test failed.")
        });
    }
    
    createTestInputs(mainMethodNameToParse) {
        let methodStart = mainMethodNameToParse.split("(");

        if(methodStart.length <= 2 && methodStart[1] == ")") {
            return;
        }

        let methodArgsAndTypes = mainMethodNameToParse.split("(")[1].split(",");
        methodArgsAndTypes[methodArgsAndTypes.length - 1] = methodArgsAndTypes[methodArgsAndTypes.length - 1].slice(0, -1);

        for(let methodArgAndType of methodArgsAndTypes) {
            let methodArgAndTypeSplit = methodArgAndType.trim().split(" ");
            let argName = methodArgAndTypeSplit[methodArgAndTypeSplit.length - 1];
            let argType = this.getValueType(methodArgAndTypeSplit[0]);

            this.vm.test.testInputs.push({
                argumentName: argName,
                valueType: argType,
                value: ""
            });
        }
    }

    createTestOutput(mainMethodNameToParse) {
        let returnTypeRegexPatten = /public\s+(int|string|bool)/;
        let returnTypeMatch = "";
        if (returnTypeRegexPatten.test(mainMethodNameToParse)) {
            returnTypeMatch = mainMethodNameToParse.match(returnTypeRegexPatten)[0];
        }

        let returnTypeSplit = returnTypeMatch.split(" ")
        this.vm.test.testOutput.valueType = this.getValueType(returnTypeSplit[returnTypeSplit.length - 1]);
    }

    getValueType(typeToConvert) {
        let argType = "";
        switch(typeToConvert.toLowerCase()) {
            case "int":
                argType = "Integer";
                break;
            case "string":
                argType = "String";
                break;
            case "bool":
                argType = "Boolean";
                break;
            default:
                break;
        }

        return argType;
    }
    
    goToTestList() {
        this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id + "/tests");
    }
}