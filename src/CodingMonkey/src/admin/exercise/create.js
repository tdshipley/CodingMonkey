import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import {Validator, ValidationEngine, length, required} from 'aurelia-validatejs';
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

        this.notify = toastr;
        this.notify.options.progressBar = true;

        var loc = window.location;
        this.heading = "Create Exercise";
        this.baseUrl = loc.protocol + "//" + loc.host;
        
        http.configure(config => {
           config.useStandardConfiguration()
           .withBaseUrl(this.baseUrl + '/api/Exercise/');
        });
        
        this.http = http;
        
        this.selectedCategories = [];
        this.categoriesList = [];
        this.getCategories();
        this.showAddCategoryForm = false;

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
                mainMethodName: "",
                mainMethodSignature: ""
            },
            exerciseCategory: {
                id: 0,
                name: "",
                description: ""
            }
        };

        this.reporter = ValidationEngine.getValidationReporter(this.vm);
        this.validator = new Validator(this.vm).ensure('exercise.name').required();
    }
    
    createExercise() {
        var exerciseId;
                  
        this.http.baseUrl = this.baseUrl + '/api/Exercise/';

        this.http.fetch('Create', {
            method: 'post',
            body: json({
                Name: this.vm.exercise.name,
                Guidance: this.vm.exercise.guidance,
                CategoryIds: this.vm.exercise.categoryids
            })
        })
        .then(response => response.json())
        .then(data => {
            this.vm.exercise.id = data.Id;
        })
        .then(() => this.createExerciseTemplate(this.vm.exercise.id))
        .catch(err => {
            this.notify.error("Create Exercise failed.");
        });
    }
    
    createExerciseTemplate(exerciseId) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/' + exerciseId + '/ExerciseTemplate/';

        this.http.fetch('Create', {
                method: 'post',
                body: json({
                    ExerciseId: exerciseId,
                    InitialCode: this.vm.exerciseTemplate.initialCode,
                    ClassName: this.vm.exerciseTemplate.className,
                    MainMethodName: this.vm.exerciseTemplate.mainMethodName,
                    MainMethodSignature: this.vm.exerciseTemplate.mainMethodSignature
                })
            })
            .then(response => response.json())
            .then(data => {
                this.vm.exerciseTemplate.id = data.Id;
            })
            .then(() => {
                this.notify.success("Created Exercise '" + this.vm.exercise.name + "'");
                this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id + "/test/create");
            })
            .catch(err => {
                this.notify.error("Create Exercise Template for Exercise failed.");
            });
    }
    
    createCategory() {
        this.http.baseUrl = this.baseUrl + '/api/ExerciseCategory/';

        this.http.fetch('Create', {
                method: 'post',
                body: json({
                    name: this.vm.exerciseCategory.name,
                    description: this.vm.exerciseCategory.description,
                })
            })
            .then(response => response.json())
            .then(data => {
                let categoryModel = {
                    id: data.Id,
                    name: data.Name,
                    description: data.Description
                }

                this.categoriesList.push(categoryModel);
                this.toggleAddCategoryForm();

                this.notify.success("Create category '" + categoryModel.name + "' succeeded.");
            })
            .catch(err => {
                this.notify.error("Create category '" + categoryModel.name + "' failed.");
            });
    }
    
    getCategories() {
        this.http.baseUrl = this.baseUrl + '/api/ExerciseCategory/';

        this.http.fetch('List')
            .then(response => response.json())
            .then(data => {
                this.categoriesList = [];

                for (let category of data) {
                    let categoryModel = {
                        id: category.Id,
                        name: category.Name,
                        description: category.Description
                    }

                    this.categoriesList.push(categoryModel);
                }
            })
            .catch(err => {
                this.notify.error("Get Exercise Categories failed.");
            });
    }
    
    toggleAddCategoryForm() {
        this.showAddCategoryForm = !this.showAddCategoryForm;
    }
    
    goToExerciseList() {
        this.appRouter.navigate("admin/exercises");
    }

    extractExpectedCodeSnippets() {
        let classNameRegexPattern = /(class\s)(\s*\w*)/;
        let methodNameRegexPattern = /\S*\s*(?=\()/;
        let methodSignatureRegexPattern = /(public|private|protected|internal)\s*(int|string|bool|char|bit|byte)\s*\S*\(\S*\s*\S*\)/;

        if (classNameRegexPattern.test(this.vm.exerciseTemplate.initialCode)) {
            this.vm.exerciseTemplate.className = this.vm.exerciseTemplate.initialCode.match(classNameRegexPattern)[2];
        }

        if (methodSignatureRegexPattern.test(this.vm.exerciseTemplate.initialCode)) {
            this.vm.exerciseTemplate.mainMethodSignature = this.vm.exerciseTemplate.initialCode.match(methodSignatureRegexPattern)[0];
            this.vm.exerciseTemplate.mainMethodName = this.vm.exerciseTemplate.mainMethodSignature.match(methodNameRegexPattern);
        }
    }

    getCurrentUserInSessionStorage() {
        let currentUserRaw = sessionStorage.getItem("currentUser");
        let currentUser = JSON.parse(currentUserRaw);

        return currentUser;
    }
}