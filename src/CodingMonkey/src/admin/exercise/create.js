import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
import 'fetch';
import toastr from 'toastr';

@inject(HttpClient, Router)
export class create {
    constructor(http, router) {
        var loc = window.location;
        
        this.notify = toastr;
        this.notify.options.progressBar = true;
        
        this.heading = "Create Exercise";
        this.exerciseCreateFailMessage = "";
        this.exerciseCreateFail = false;
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
            this.notify.error("Create Exercise failed.")
            console.log(err);
        })
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
            this.appRouter.navigate("admin/exercise/" + this.vm.exercise.id + "/" + this.vm.exerciseTemplate.id);
        })
        .catch(err => {
            this.notify.error("Create Exercise Template for Exercise failed.")
            console.log(err);
        })
    }
}