import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class update {
    constructor(http, router) {
        var loc = window.location;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        this.heading = "Update Exercise";

        this.baseUrl = loc.protocol + "//" + loc.host;
        this.appRouter = router;
        
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
                mainMethodName: ""
            }
        };
        
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
              this.getExerciseTemplate(params.id)
              this.heading = "Update Exercise: " + this.vm.exercise.name;
          })
          .then(() => {
              this.getCategories();
          })
          .catch(err => {
              console.log(err);
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
          })
          .catch(err => {
              console.log(err);
          });
    }
    
    updateExercise() {
        this.http.baseUrl = this.baseUrl + "/api/Exercise/";
        
        this.http.fetch('update/' + this.vm.exercise.id, {
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
        .then(() => this.updateExerciseTemplate(this.vm.exercise.id))
        .catch(err => {
            this.notify.error("Update Exercise failed.")
            console.log(err);
        })
        
    }
    
    updateExerciseTemplate(exerciseId) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/' + exerciseId + '/ExerciseTemplate/';
        
        this.http.fetch('update', {
            method: 'post',
            body: json({
                ExerciseId: exerciseId,
                InitialCode: this.vm.exerciseTemplate.initialCode,
                ClassName: this.vm.exerciseTemplate.className,
                MainMethodName: this.vm.exerciseTemplate.mainMethodName
            })
        })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            this.vm.exerciseTemplate.id = data.Id;
        })
        .then(() => {
            this.notify.success("Updated Exercise '" + this.vm.exercise.name + "'");
            this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id);
        })
        .catch(err => {
            this.notify.error("Update Exercise Template for Exercise failed.")
        })
    }
    
    createCategory() {
        this.http.baseUrl = this.baseUrl + '/api/ExerciseCategory/'
        
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
            this.notify.error("Create category '" + categoryModel.name + "' failed.")
        })
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
                this.notify.error("Get Exercise Categories failed.")
            })
    }
    
    isExerciseCategorySelected(idToCheck) {
        if(this.vm.exercise.categoryids.indexOf(idToCheck) === -1) {
            return false;
        } else {
            return true;
        }
    }
}