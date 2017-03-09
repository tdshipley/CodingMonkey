// http://davismj.me/blog/aurelia-bundling/
module.exports = {
    "bundles": {
        "dist/app-build": {
            "includes": [
            // First, we bundle all css, html, and javascript in the root folder 
      	    // and all subfolders. The bundler reads the mapping from the
      	    // config.js file, which by default uses the './dist/' folder.
            // Checking that is a issue caused by bundling system.js files
            "**/*.js",
            "**/*.html!text",
            "**/*.css!text",

            // Next, we need to bundle all project dependencies. It is a good 
            // idea to explicitly all required Aurelia libraries.
            "aurelia-framework",
            "aurelia-bootstrapper",
            "aurelia-binding",
            "aurelia-templating",
            "aurelia-templating-binding",
            "aurelia-templating-resources",
            "aurelia-loader",
            "aurelia-loader-default",
            "aurelia-pal",
            "aurelia-pal-browser",
            "aurelia-polyfills",

            // Next, we include the optional Aurelia dependencies. Your project 
            // may use dependencies not listed here.
            "aurelia-fetch-client",
            "aurelia-router",
            "aurelia-animator-css",
            "aurelia-history-browser",
            "aurelia-logging",
            "aurelia-logging-console",
            "aurelia-templating-router",
            "aurelia-dialog",
            "aurelia-metadata",
            "aurelia-path",
            "aurelia-task-queue",


            // Last, we include any other project dependencies.
            "bootstrap",
            "bootstrap/css/bootstrap.css!text",
            "toastr",
            "toastr/build/toastr.css!text",
            "font-awesome/css/font-awesome.min.css!text",
            "fetch"
        ],
        "options": {
            "minify": false
        }
    }
  }
};
