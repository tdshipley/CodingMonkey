import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient)
export class details {
        constructor(http) {
        var loc = window.location;
        
        this.heading = "Test Details";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
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
          .catch(err => {
              console.log(err);
              this.notify.error("Failed to get test.")
          });
    }
}