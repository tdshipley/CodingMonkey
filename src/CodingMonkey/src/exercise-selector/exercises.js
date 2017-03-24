import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';
import {StringHelpers} from './../utility-classes/string-helpers.js'

@inject(HttpClient, Router)
export class exercises {
    constructor(http, router) {
        this.heading = "Select an Exercise";
        
        var loc = window.location;
        this.baseUrl = loc.protocol + "//" + loc.host;
        this.appRouter = router;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        http.configure(config => {
           config.useStandardConfiguration();
        });
        
        this.http = http;

        this.vm = {
            exercises: [],
            exerciseCategory: {
                id: 0,
                name: "",
                description: "",
                exerciseids: []
            },
            pageLoading: true
        };

        this.stringHelper = new StringHelpers();
    }
    
    activate(params) {
        this.http.baseUrl = this.baseUrl + '/api/Exercise/';

        this.http.fetch('list')
            .then(response => response.json())
            .then(data => {
                for (let exercise of data) {
                    if (exercise.CategoryIds.includes(Number(params.exerciseCategoryId))) {
                        let exercisePageId = this.stringHelper.convertTitleToPageId(exercise.Name);

                        var vm = {
                            id: exercise.Id,
                            exerciseTemplateId: exercise.ExerciseTemplateId,
                            name: exercise.Name,
                            guidance: exercise.Guidance,
                            categoryids: exercise.CategoryIds,
                            pageId: exercisePageId
                        };

                        this.vm.exercises.push(vm);
                    }
                }
            })
            .then(() => {
                this.getCategories(params.exerciseCategoryId);
            })
            .catch(err => {
                this.notify.error("Failed to get exercises.");
            });
    }
    
    
    getCategories(exerciseCategoryId) {
        this.http.baseUrl = this.baseUrl + '/api/ExerciseCategory/';
        
        this.http.fetch('details/' + exerciseCategoryId)
            .then(response => response.json())
            .then(data => {
                this.vm.exerciseCategory.id = data.Id;
                this.vm.exerciseCategory.name = data.Name;
                this.vm.exerciseCategory.description = data.Description;
                this.vm.exerciseCategory.exerciseids = data.ExerciseIds;
                
                this.heading = this.vm.exerciseCategory.name + " Exercises";

                this.vm.pageLoading = false;
            })
            .catch(err => {
                this.notify.error('Failed to get category.');
            });
    }
    
    goToCatgeories() {
        this.appRouter.navigate("categories");
    }
    
    goToExercise(exerciseId) {
        this.appRouter.navigate("code/editor/" + exerciseId);
    }
}