import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class details {
        constructor(http, router) {
        var loc = window.location;
        
        this.heading = "Test Details";
        this.baseUrl = loc.protocol + "//" + loc.host;
        this.appRouter = router;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
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
            }
        }
        
        this.exerciseId = 0;
        this.exerciseTemplateId = 0;
    }
    
    activate(params) {
        this.exerciseId = params.exerciseId;
        this.exerciseTemplateId = params.exerciseTemplateId;
        
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
          .catch(err => {
              this.notify.error("Failed to get test.")
          });
    }
    
    goToTestList() {
        this.appRouter.navigate("admin/exercise/" + this.exerciseId + "/" + this.exerciseTemplateId + "/tests");
    }
}