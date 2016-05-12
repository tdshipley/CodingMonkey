import bootstrap from 'bootstrap';

export class App {
  configureRouter(config, router) {
    config.title = 'Coding Monkey';
    config.map(
        [
            // User Routes
            {
                settings: {
                    group: 'user'
                },
                route: ['', 'welcome'],
                name: 'welcome',
                moduleId: 'welcome',
                nav: true,
                title: 'Welcome'
            },
            {
                settings: {
                    group: 'user'
                },
                route: ['about'],
                name: 'about',
                moduleId: 'about',
                nav: true,
                title: 'About'
            },
            {
                settings: {
                    group: 'user'
                },
                route: 'code/editor/:exerciseId',  
                name: 'CodeEditor',
                moduleId: './code/editor',
                nav: false,
                title: 'Code Editor'
            },
            {
                settings: {
                    group: 'user'
                },
                route: 'categories',  
                name: 'Categories',
                moduleId: './exercise-selector/categories',
                nav: true,
                title: 'Exercise Categories'
            },
            {
                settings: {
                    group: 'user'
                },
                route: 'category/:exerciseCategoryId/exercises',  
                name: 'CategoryExercises',
                moduleId: './exercise-selector/exercises',
                nav: false,
                title: 'Exercises in Category'
            },
            // Authentication Routes
            {
                settings: {
                    group: 'user'
                },
                route: 'login',  
                name: 'Login',
                moduleId: './authentication/login',
                nav: false,
                title: 'Login'
            },
            {
                settings: {
                    group: 'user'
                },
                route: 'logout',  
                name: 'Logout',
                moduleId: './authentication/logout',
                nav: false,
                title: 'Logout'
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'authentication/changepassword',
                name: 'ChangePassword',
                moduleId: './authentication/changepassword',
                nav: true,
                title: 'Change Password'
            },
            // Admin Routes
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
                route: 'admin/exercise/:id/',
                name: 'AdminExerciseDetails',
                moduleId: './admin/exercise/details',
                nav: false,
                title: "Exercise Details"
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/:id/update',
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
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/:exerciseId/test/create',
                name: 'AdminExerciseTestCreate',
                moduleId: './admin/test/create',
                nav: false,
                title: 'Test Create'
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/:exerciseId/tests',
                name: 'AdminExerciseTests',
                moduleId: './admin/test/list',
                nav: false,
                title: 'Exercise Tests List'
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/:exerciseId/test/:id',
                name: 'AdminExerciseTestDetails',
                moduleId: './admin/test/details',
                nav: false,
                title: "Test Details"
            },
            {
                settings: {
                    group: 'admin'
                },
                route: 'admin/exercise/:exerciseId/test/:id/update',
                name: 'AdminExerciseTestUpdate',
                moduleId: './admin/test/update',
                nav: false,
                title: "Test Update"
            }
    ]);

    this.router = router;
  }
}
