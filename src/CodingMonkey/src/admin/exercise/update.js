import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';
import {Authentication} from './../../authentication/authentication.js';

@inject(HttpClient, Router)
export class update {
    constructor(http, router) {
        this.appRouter = router;

        // Check there is a logged in user or kick them to homepage
        new Authentication(this.appRouter).verifyUserLoggedIn();

        var loc = window.location;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        this.heading = "Update Exercise";

        this.baseUrl = loc.protocol + "//" + loc.host;
        
        http.configure(config => {
           config.useStandardConfiguration()
           .withBaseUrl(this.baseUrl + '/api/Exercise/');
        });
        
        this.http = http;
        
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
            }
        };
        
        this.selectedCategories = [];
        this.categoriesList = [];
        this.showAddCategoryForm = false;
    }
    
    activate(params) {
        this.http.fetch('details/' + params.id)
          .then(response => response.json())
          .then(data => {
              this.vm.exercise.id = data.Id;
              this.vm.exercise.name = data.Name;
              this.vm.exercise.guidance = data.Guidance;
              this.vm.exercise.categoryids = data.CategoryIds;
          })
          .then(() => {
                this.getExerciseTemplate(params.id);
              this.heading = "Update Exercise: " + this.vm.exercise.name;
          })
          .then(() => {
              this.getCategories();
          })
          .catch(err => {
                this.notify.error("Failed to get exercise");
            });
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
          .catch(err => {
                this.notify.error("Failed to get exercise template");
            });
    }
    
    updateExercise() {
        this.http.baseUrl = this.baseUrl + "/api/Exercise/";

        this.http.fetch('update/' + this.vm.exercise.id, {
                method: 'post',
                body: json({
                    Id: this.vm.exercise.id,
                    Name: this.vm.exercise.name,
                    Guidance: this.vm.exercise.guidance,
                    CategoryIds: this.getSelectedCategoryIds()
                })
            })
            .then(response => response.json())
            .then(data => {
                this.vm.exercise.id = data.Id;
            })
            .then(() => this.updateExerciseTemplate(this.vm.exercise.id))
            .catch(err => {
                this.notify.error("Update Exercise failed.");
            });

    }
    
    updateExerciseTemplate(exerciseId) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/' + exerciseId + '/ExerciseTemplate/';

        this.http.fetch('update', {
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
                this.notify.success("Updated Exercise '" + this.vm.exercise.name + "'");
                this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id);
            })
            .catch(err => {
                this.notify.error("Update Exercise Template for Exercise failed.");
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
                this.selectedCategories.push(categoryModel);
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

                    if(this.isExerciseCategorySelected(category.Id)) {
                        this.selectedCategories.push(categoryModel);
                    }
                }
            })
            .catch(err => {
                this.notify.error("Get Exercise Categories failed.");
            });
    }
    
    isExerciseCategorySelected(idToCheck) {
        if(this.vm.exercise.categoryids.indexOf(idToCheck) === -1) {
            return false;
        } else {
            return true;
        }
    }

    getSelectedCategoryIds() {
        let selectedCategoryIds = [];
        for(let selectedCategory of this.selectedCategories) {
            selectedCategoryIds.push(selectedCategory.id);
        }

        return selectedCategoryIds;
    }

    toggleAddCategoryForm() {
        this.showAddCategoryForm = !this.showAddCategoryForm;
    }

    extractExpectedCodeSnippets() {
        let classNameRegexPattern = /(class\s)(\s*\w*)/;
        let methodNameRegexPattern = /\S*\s*(?=\()/;
        let methodSignatureRegexPattern = /(public|protected|private)\s*(int|string|bool|char|bit|byte).+/;

        if (classNameRegexPattern.test(this.vm.exerciseTemplate.initialCode)) {
            this.vm.exerciseTemplate.className = this.vm.exerciseTemplate.initialCode.match(classNameRegexPattern)[2].trim();
        }

        if (methodSignatureRegexPattern.test(this.vm.exerciseTemplate.initialCode)) {
            this.vm.exerciseTemplate.mainMethodSignature = this.vm.exerciseTemplate.initialCode.match(methodSignatureRegexPattern)[0].trim();
            this.vm.exerciseTemplate.mainMethodName = this.vm.exerciseTemplate.mainMethodSignature.match(methodNameRegexPattern)[0].trim();
        }
    }
}