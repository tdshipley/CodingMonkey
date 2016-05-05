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
                mainMethodName: ""
            },
            exerciseCategory: {
                id: 0,
                name: "",
                description: ""
            }
        };
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
                    MainMethodName: this.vm.exerciseTemplate.mainMethodName
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
    
    toggleAddCategoryForm() {
        this.showAddCategoryForm = !this.showAddCategoryForm;
    }
    
    goToExerciseList() {
        this.appRouter.navigate("admin/exercises");
    }

    getCurrentUserInSessionStorage() {
        let currentUserRaw = sessionStorage.getItem("currentUser");
        let currentUser = JSON.parse(currentUserRaw);

        return currentUser;
    }
}