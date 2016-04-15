import bootstrap from 'bootstrap';

export class App {
  configureRouter(config, router) {
    config.title = 'Coding Monkey';
    config.map(
        [
            {
                settings: {
                    group: 'root'
                },
                route: ['', 'welcome'],
                name: 'welcome',
                moduleId: 'welcome',
                nav: true,
                title: 'Welcome'
            },
            {
                settings: {
                    group: 'root'
                },
                route: 'code/editor',  
                name: 'CodeEditor',
                moduleId: './code/editor',
                nav: true,
                title: 'Code Editor'
            },
            // Admin
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/create',
                name: 'AdminExerciseCreate', 
                moduleId: './admin/exercise/create',
                nav: true,
                title: "Exercise Create"
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/:id/:exerciseTemplateId',
                name: 'AdminExerciseDetails',
                moduleId: './admin/exercise/details',
                nav: false,
                title: "Exercise Details"
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/:id/:exerciseTemplateId/update',
                name: 'AdminExerciseUpdate',
                moduleId: './admin/exercise/update',
                nav: false,
                title: "Exercise Update"
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercises',
                name: 'AdminExercises',
                moduleId: './admin/exercise/list',
                nav: true,
                title: 'Exercise List'
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/categories',
                name: 'AdminExerciseCategories',
                moduleId: './admin/exercise-category/list',
                nav: true,
                title: 'Exercise Category List'
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/categories/:id',
                name: 'AdminExerciseCategoryDetails',
                moduleId: './admin/exercise-category/details',
                nav: false,
                title: 'Exercise Category Details'
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/categories/:id/update',
                name: 'AdminExerciseCategoryUpdate',
                moduleId: './admin/exercise-category/update',
                nav: false,
                title: 'Exercise Category Update'
            }
    ]);

    this.router = router;
  }
}
