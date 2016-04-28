import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';
import 'ace';
import toastr from 'toastr';

@inject(HttpClient)
export class Editor {    
    constructor(http) {
        this.heading = "";

        var loc = window.location;
        this.baseUrl = loc.protocol + "//" + loc.host;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        http.configure(config => {
            config
                .useStandardConfiguration();
        });

        this.http = http;
        this.vm = this;
        this.markedLines = [];
        
        this.vm = {
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
            },
            testResults: []
        }
        
        this.exerciseId = 0;
        this.codeHasErrors = false;
    }

    activate(params) {      
        this.exerciseId = params.exerciseId;
    }

    attached(params) {
        //Config for ace - https://github.com/jspm/registry/issues/38#issuecomment-168572405
        let base = System.normalizeSync('ace');
        base = base.substr(0, base.length - 3);
        ace.config.set('basePath', base);

        //Ace settings
        this.codeEditor = ace.edit("aceEditor");
        this.codeEditor.setTheme("ace/theme/monokai");
        this.codeEditor.getSession().setMode("ace/mode/csharp");
        
        this.getExerciseTemplate(this.exerciseId);
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
            .then(() => {
                this.codeEditor.setValue(this.vm.exerciseTemplate.initialCode, -1);
                this.getExercise(this.exerciseId);
            })
            .catch(err => {
            console.log(err);
                this.notify.error('Failed to get exercise template.');
            });
    }
    
    getExercise(exerciseId) {
        this.http.baseUrl = this.baseUrl + "/api/Exercise/";
        
        this.http.fetch('details/' + exerciseId)
            .then(response => response.json())
            .then(data => {
                this.vm.exercise.id = data.Id;
                this.vm.exercise.name = data.Name;
                this.vm.exercise.guidance = data.Guidance;
                this.vm.exercise.categoryids = data.CategoryIds;
            })
            .then(() => {
                this.heading = this.vm.exercise.name;
            })
            .catch(err => {
                this.notify.error('Failed to get exercise.');
            });
    }

    submitCode() {
        this.http.baseUrl = this.baseUrl + '/api/CodeExecution/';
        
        this.http.fetch('Compile/' + this.exerciseId, {
            method: 'post',
            body: json({code: this.codeEditor.getValue()})
        })
        .then(response => response.json())
        .then(data => {
            this.vm = data;
            this.vm.SubmittedCode = true;
            this.codeHasErrors = data.HasErrors;
            this.highlightErrors(data);
        })
        .then(() => {
            this.executeCode();
        })
        .catch(err => {
            this.notify.error("Failed to submit code to test.");
        });
    }

    highlightErrors(data) {
        if (this.codeHasErrors) {
            this.notify.warning("The code has compiler errors. Fix them then submit again.");
            for (let error of data.Errors) {
                this.highlightError(error.LineNumberStart, error.LineNumberEnd, 0, error.ColEnd);
            }
        }
        else {
            this.unhighlightError();
        }
    }

    highlightError(startLine, endLine, startCol, endCol) {
        this.unhighlightError();
        var Range = ace.require("ace/range").Range;
        this.markedLines.push(this.codeEditor.session.addMarker(new Range(startLine - 1, startCol, endLine - 1, endCol), "errorHighlight", "fullLine"));
    }

    unhighlightError() {
        for (var i = 0; i < this.markedLines.length; i++) {
            this.codeEditor.getSession().removeMarker(this.markedLines[i]);
        }

        this.markedLines = [];
    }

    executeCode() {
        if (this.codeHasErrors === false) {
            let testsPassed = true;

            this.http.baseUrl = this.baseUrl + '/api/CodeExecution/';

            this.http.fetch('Execute/' + this.exerciseId, {
                    method: 'post',
                    body: json({ code: this.codeEditor.getValue() })
                })
                .then(response => response.json())
                .then(data => {
                    this.vm.SubmittedCode = true;
                    this.vm.testResults = [];

                    for (let testResult of data.TestResults) {
                        var testVM = {
                            actualOutput: testResult.ActualOutput,
                            description: testResult.Description,
                            expectedOutput: testResult.ExpectedOutput,
                            testPassed: testResult.TestPassed,
                            inputs: []
                        };

                        for (let testInput of testResult.Inputs) {
                            var inputVM = {
                                argumentName: testInput.ArgumentName,
                                value: testInput.Value
                            }

                            testVM.inputs.push(inputVM);
                        }

                        if (testsPassed) {
                            testsPassed = testVM.testPassed;
                        }

                        this.vm.testResults.push(testVM);
                    }

                    if (testsPassed) {
                        this.notify.success("All tests passed!");
                    } else {
                        this.notify.warning("Tests Failed. Have a look at the results and try again.");
                    }
                })
                .catch(err => {
                    console.log(err);
                });
        }
    }
}