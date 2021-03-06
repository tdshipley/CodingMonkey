﻿import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';
import 'ace';
import "ace/ext-language_tools";
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
        this.allTestsPassed = false;

        this.vm = this.createEmptyViewModel();
        
        this.exerciseId = 0;
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
        ace.require("ace/ext/language_tools");
        this.codeEditor = ace.edit("aceEditor");
        this.codeEditor.$blockScrolling = Infinity;
        this.codeEditor.setTheme("ace/theme/dreamweaver");
        this.codeEditor.getSession().setMode("ace/mode/csharp");
        this.codeEditor.setOptions({
            enableBasicAutocompletion: true,
            enableSnippets: true,
            enableLiveAutocompletion: true
        });
        
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
                this.vm.pageLoading = false;
            })
            .catch(err => {
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
        this.setCodeProcessingStatusToFalse();
        this.vm.codeProcessingStatus.processing = true;
        this.vm.codeProcessingStatus.compiling = true;

        this.http.baseUrl = this.baseUrl + '/api/CodeExecution/';
        
        this.http.fetch('Compile/' + this.exerciseId, {
            method: 'post',
            body: json({code: this.codeEditor.getValue()})
        })
        .then(response => response.json())
        .then(data => {
            this.vm.SubmittedCode = true;
            this.processCodeCompliationResults(data);
        })
        .then(() => {
            this.vm.codeProcessingStatus.compileComplete = true;
            this.executeCode();
        })
        .catch(err => {
            this.vm.codeProcessingStatus.processing = false;
            this.notify.error("Failed to submit code to test.");
        });

        this.vm.codeProcessingStatus.processing = false;
    }

    highlightErrors(data) {
        if (this.vm.codeHasCompilerErrors) {
            this.unhighlightError();
            this.notify.warning("The code has compiler errors. Fix them then submit again.");
            for (let compilerError of data.CompilerErrors) {
                this.highlightError(compilerError.LineNumberStart, compilerError.LineNumberEnd, 0, compilerError.ColEnd);
            }
        }
        else {
            this.unhighlightError();
        }
    }

    highlightError(startLine, endLine, startCol, endCol) {
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
        if (this.vm.codeHasCompilerErrors === false) {
            this.vm.codeProcessingStatus.executingTests = true;

            let testsPassed = true;
            this.http.baseUrl = this.baseUrl + '/api/CodeExecution/';

            this.http.fetch('Execute/' + this.exerciseId, {
                    method: 'post',
                    body: json({ code: this.codeEditor.getValue() })
                })
                .then(response => response.json())
                .then(data => {
                    this.processCodeExecutionResults(data);
                })
                .catch(err => {
                    this.vm.codeProcessingStatus.processing = false;
                    this.vm.codeProcessingStatus.executingTests = false;
                    this.notify.error("Failed to execute code");
                    this.vm.testResults = [];
                });
        }

        this.vm.codeProcessingStatus.executingTestsComplete = true;
        this.vm.showTestResults = this.getShowTestResultsValue();
    }

    getShowTestResultsValue() {
        return !this.vm.codeHasCompilerErrors &&
            !this.vm.codeHasRuntimeError &&
            !this.vm.codeProcessingStatus.processing &&
            this.vm.SubmittedCode &&
            this.vm.testResults.length > 0;
    }

    processCodeCompliationResults(data) {
        this.vm.codeHasCompilerErrors = data.HasCompilerErrors;
        this.vm.codeHasRuntimeError = data.HasRuntimeError;
        this.vm.compilerErrors = data.CompilerErrors;
        this.vm.runtimeError = data.RuntimeError;
        this.vm.codeProcessingStatus.compileSucessful = false;
        this.vm.codeProcessingStatus.compileSucessful = data.CompilerErrors.length === 0;
        this.highlightErrors(data);
    }

    processCodeExecutionResults(data) {
        this.vm.SubmittedCode = true;
        this.vm.testResults = [];

        this.vm.codeHasRuntimeError = data.HasRuntimeError;
        this.vm.runtimeError = data.RuntimeError;
        this.vm.allTestsExecuted = data.AllTestsExecuted;

        if (!data.HasRuntimeError) {
            this.vm.codeProcessingStatus.executeTestsSuccessful = true;
            let testsPassed = this.processTestResults(data);

            if (testsPassed) {
                this.notify.success("All tests passed!");
                this.allTestsPassed = true;
            } else {
                this.notify.warning("Tests Failed. Review the results and try again.");
                this.allTestsPassed = false;
            }
        } else {
            this.vm.codeProcessingStatus.executeTestsSuccessful = false;
            this.notify.warning("There was an error running your code. Review the error and try again.");
            this.allTestsPassed = false;
        }

        this.vm.codeProcessingStatus.executingTestsComplete = true;
        this.vm.showTestResults = this.getShowTestResultsValue();
    }

    processTestResults(data) {
        let lastTestPassed = true;

        for (let testResult of data.TestResults) {
            var testVM = {
                actualOutput: testResult.ActualOutput,
                description: testResult.Description,
                expectedOutput: testResult.ExpectedOutput,
                testPassed: testResult.TestPassed,
                testExecuted: testResult.TestExecuted,
                inputs: []
            };

            for (let testInput of testResult.Inputs) {
                var inputVM = {
                    argumentName: testInput.ArgumentName,
                    value: testInput.Value
                }

                testVM.inputs.push(inputVM);
            }

            if (lastTestPassed) {
                lastTestPassed = testVM.testPassed;
            }

            this.vm.testResults.push(testVM);
        }

        return lastTestPassed;
    }

    setCodeProcessingStatusToFalse() {
        this.vm.codeProcessingStatus.processing = false;
        this.vm.codeProcessingStatus.compiling = false;
        this.vm.codeProcessingStatus.compileComplete = false;
        this.vm.codeProcessingStatus.compileSucessful = false;
        this.vm.codeProcessingStatus.executingTests = false;
        this.vm.codeProcessingStatus.executingTestsComplete = false;
        this.vm.codeProcessingStatus.executeTestsSuccessful = false;
    }

    createEmptyViewModel() {
        return {
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
            testResults: [],
            codeProcessingStatus: {
                processing: false,
                compiling: false,
                compileComplete: false,
                compileSucessful: false,
                executingTests: false,
                executingTestsComplete: false,
                executeTestsSuccessful: false
            },
            pageLoading: true,
            showTestResults: false
        }
    }
}