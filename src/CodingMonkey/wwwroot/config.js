System.config({
  defaultJSExtensions: true,
  transpiler: "babel",
  babelOptions: {
    "optional": [
      "es7.decorators",
      "es7.classProperties"
    ]
  },
  paths: {
    "*": "dist/*",
    "github:*": "jspm_packages/github/*",
    "npm:*": "jspm_packages/npm/*"
  },
  map: {
    "ace": "github:ajaxorg/ace-builds@1.2.6",
    "aurelia-animator-css": "npm:aurelia-animator-css@1.0.1",
    "aurelia-binding": "npm:aurelia-binding@1.2.1",
    "aurelia-bootstrapper": "npm:aurelia-bootstrapper@1.0.1",
    "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
    "aurelia-dialog": "npm:aurelia-dialog@1.0.0-beta.3.0.1",
    "aurelia-fetch-client": "npm:aurelia-fetch-client@1.1.1",
    "aurelia-framework": "npm:aurelia-framework@1.0.8",
    "aurelia-history-browser": "npm:aurelia-history-browser@1.0.0",
    "aurelia-loader": "npm:aurelia-loader@1.0.0",
    "aurelia-loader-default": "npm:aurelia-loader-default@1.0.1",
    "aurelia-logging": "npm:aurelia-logging@1.3.1",
    "aurelia-logging-console": "npm:aurelia-logging-console@1.0.0",
    "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
    "aurelia-pal": "npm:aurelia-pal@1.3.0",
    "aurelia-pal-browser": "npm:aurelia-pal-browser@1.1.0",
    "aurelia-path": "npm:aurelia-path@1.1.1",
    "aurelia-polyfills": "npm:aurelia-polyfills@1.2.0",
    "aurelia-router": "npm:aurelia-router@1.2.0",
    "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
    "aurelia-templating": "npm:aurelia-templating@1.4.0",
    "aurelia-templating-binding": "npm:aurelia-templating-binding@1.2.0",
    "aurelia-templating-resources": "npm:aurelia-templating-resources@1.2.0",
    "aurelia-templating-router": "npm:aurelia-templating-router@1.0.1",
    "babel": "npm:babel-core@5.8.38",
    "babel-runtime": "npm:babel-runtime@5.8.38",
    "bootstrap": "github:twbs/bootstrap@3.3.7",
    "core-js": "npm:core-js@1.2.7",
    "fetch": "github:github/fetch@0.10.1",
    "font-awesome": "npm:font-awesome@4.7.0",
    "jquery": "npm:jquery@3.1.1",
    "jquery-validation": "npm:jquery-validation@1.16.0",
    "jquery-validation-unobtrusive": "npm:jquery-validation-unobtrusive@3.2.6",
    "text": "github:systemjs/plugin-text@0.0.3",
    "toastr": "github:CodeSeven/toastr@2.1.3",
    "github:CodeSeven/toastr@2.1.3": {
      "css": "github:systemjs/plugin-css@0.1.32",
      "jquery": "npm:jquery@3.1.1"
    },
    "github:jspm/nodelibs-assert@0.1.0": {
      "assert": "npm:assert@1.4.1"
    },
    "github:jspm/nodelibs-buffer@0.1.0": {
      "buffer": "npm:buffer@3.6.0"
    },
    "github:jspm/nodelibs-path@0.1.0": {
      "path-browserify": "npm:path-browserify@0.0.0"
    },
    "github:jspm/nodelibs-process@0.1.2": {
      "process": "npm:process@0.11.9"
    },
    "github:jspm/nodelibs-util@0.1.0": {
      "util": "npm:util@0.10.3"
    },
    "github:jspm/nodelibs-vm@0.1.0": {
      "vm-browserify": "npm:vm-browserify@0.0.4"
    },
    "github:twbs/bootstrap@3.3.7": {
      "jquery": "npm:jquery@3.1.1"
    },
    "npm:assert@1.4.1": {
      "assert": "github:jspm/nodelibs-assert@0.1.0",
      "buffer": "github:jspm/nodelibs-buffer@0.1.0",
      "process": "github:jspm/nodelibs-process@0.1.2",
      "util": "npm:util@0.10.3"
    },
    "npm:aurelia-animator-css@1.0.1": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.0"
    },
    "npm:aurelia-binding@1.2.1": {
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0"
    },
    "npm:aurelia-bootstrapper@1.0.1": {
      "aurelia-event-aggregator": "npm:aurelia-event-aggregator@1.0.1",
      "aurelia-framework": "npm:aurelia-framework@1.0.8",
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-history-browser": "npm:aurelia-history-browser@1.0.0",
      "aurelia-loader-default": "npm:aurelia-loader-default@1.0.1",
      "aurelia-logging-console": "npm:aurelia-logging-console@1.0.0",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-pal-browser": "npm:aurelia-pal-browser@1.1.0",
      "aurelia-polyfills": "npm:aurelia-polyfills@1.2.0",
      "aurelia-router": "npm:aurelia-router@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.0",
      "aurelia-templating-binding": "npm:aurelia-templating-binding@1.2.0",
      "aurelia-templating-resources": "npm:aurelia-templating-resources@1.2.0",
      "aurelia-templating-router": "npm:aurelia-templating-router@1.0.1"
    },
    "npm:aurelia-dependency-injection@1.3.0": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-dialog@1.0.0-beta.3.0.1": {
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.0"
    },
    "npm:aurelia-event-aggregator@1.0.1": {
      "aurelia-logging": "npm:aurelia-logging@1.3.1"
    },
    "npm:aurelia-framework@1.0.8": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.0"
    },
    "npm:aurelia-history-browser@1.0.0": {
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-loader-default@1.0.1": {
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-loader@1.0.0": {
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-path": "npm:aurelia-path@1.1.1"
    },
    "npm:aurelia-logging-console@1.0.0": {
      "aurelia-logging": "npm:aurelia-logging@1.3.1"
    },
    "npm:aurelia-metadata@1.0.3": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-pal-browser@1.1.0": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-polyfills@1.2.0": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-route-recognizer@1.1.0": {
      "aurelia-path": "npm:aurelia-path@1.1.1"
    },
    "npm:aurelia-router@1.2.0": {
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-event-aggregator": "npm:aurelia-event-aggregator@1.0.1",
      "aurelia-history": "npm:aurelia-history@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-route-recognizer": "npm:aurelia-route-recognizer@1.1.0"
    },
    "npm:aurelia-task-queue@1.2.0": {
      "aurelia-pal": "npm:aurelia-pal@1.3.0"
    },
    "npm:aurelia-templating-binding@1.2.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-templating": "npm:aurelia-templating@1.4.0"
    },
    "npm:aurelia-templating-resources@1.2.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.0"
    },
    "npm:aurelia-templating-router@1.0.1": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-router": "npm:aurelia-router@1.2.0",
      "aurelia-templating": "npm:aurelia-templating@1.4.0"
    },
    "npm:aurelia-templating@1.4.0": {
      "aurelia-binding": "npm:aurelia-binding@1.2.1",
      "aurelia-dependency-injection": "npm:aurelia-dependency-injection@1.3.0",
      "aurelia-loader": "npm:aurelia-loader@1.0.0",
      "aurelia-logging": "npm:aurelia-logging@1.3.1",
      "aurelia-metadata": "npm:aurelia-metadata@1.0.3",
      "aurelia-pal": "npm:aurelia-pal@1.3.0",
      "aurelia-path": "npm:aurelia-path@1.1.1",
      "aurelia-task-queue": "npm:aurelia-task-queue@1.2.0"
    },
    "npm:babel-runtime@5.8.38": {
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:buffer@3.6.0": {
      "base64-js": "npm:base64-js@0.0.8",
      "child_process": "github:jspm/nodelibs-child_process@0.1.0",
      "fs": "github:jspm/nodelibs-fs@0.1.2",
      "ieee754": "npm:ieee754@1.1.8",
      "isarray": "npm:isarray@1.0.0",
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:core-js@1.2.7": {
      "fs": "github:jspm/nodelibs-fs@0.1.2",
      "path": "github:jspm/nodelibs-path@0.1.0",
      "process": "github:jspm/nodelibs-process@0.1.2",
      "systemjs-json": "github:systemjs/plugin-json@0.1.2"
    },
    "npm:font-awesome@4.7.0": {
      "css": "github:systemjs/plugin-css@0.1.32"
    },
    "npm:inherits@2.0.1": {
      "util": "github:jspm/nodelibs-util@0.1.0"
    },
    "npm:jquery-validation-unobtrusive@3.2.6": {
      "jquery": "npm:jquery@3.1.1",
      "jquery-validation": "npm:jquery-validation@1.16.0"
    },
    "npm:jquery-validation@1.16.0": {
      "jquery": "npm:jquery@3.1.1",
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:path-browserify@0.0.0": {
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:process@0.11.9": {
      "assert": "github:jspm/nodelibs-assert@0.1.0",
      "fs": "github:jspm/nodelibs-fs@0.1.2",
      "vm": "github:jspm/nodelibs-vm@0.1.0"
    },
    "npm:util@0.10.3": {
      "inherits": "npm:inherits@2.0.1",
      "process": "github:jspm/nodelibs-process@0.1.2"
    },
    "npm:vm-browserify@0.0.4": {
      "indexof": "npm:indexof@0.0.1"
    }
  },
  bundles: {
    "app-build.js": [
      "about.html!github:systemjs/plugin-text@0.0.3.js",
      "about.js",
      "admin/exercise-category/details.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/exercise-category/details.js",
      "admin/exercise-category/list.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/exercise-category/list.js",
      "admin/exercise-category/update.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/exercise-category/update.js",
      "admin/exercise/create.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/exercise/create.js",
      "admin/exercise/details.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/exercise/details.js",
      "admin/exercise/list.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/exercise/list.js",
      "admin/exercise/update.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/exercise/update.js",
      "admin/test/create.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/test/create.js",
      "admin/test/details.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/test/details.js",
      "admin/test/list.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/test/list.js",
      "admin/test/update.html!github:systemjs/plugin-text@0.0.3.js",
      "admin/test/update.js",
      "app.html!github:systemjs/plugin-text@0.0.3.js",
      "app.js",
      "authentication/authentication.js",
      "authentication/changepassword.html!github:systemjs/plugin-text@0.0.3.js",
      "authentication/changepassword.js",
      "authentication/login.html!github:systemjs/plugin-text@0.0.3.js",
      "authentication/login.js",
      "authentication/logout.html!github:systemjs/plugin-text@0.0.3.js",
      "authentication/logout.js",
      "blur-image.js",
      "code/editor.html!github:systemjs/plugin-text@0.0.3.js",
      "code/editor.js",
      "dialog-prompt.html!github:systemjs/plugin-text@0.0.3.js",
      "dialog-prompt.js",
      "exercise-selector/categories.html!github:systemjs/plugin-text@0.0.3.js",
      "exercise-selector/categories.js",
      "exercise-selector/exercises.html!github:systemjs/plugin-text@0.0.3.js",
      "exercise-selector/exercises.js",
      "github:CodeSeven/toastr@2.1.3.js",
      "github:CodeSeven/toastr@2.1.3/build/toastr.css!github:systemjs/plugin-css@0.1.32.js",
      "github:CodeSeven/toastr@2.1.3/build/toastr.css!github:systemjs/plugin-text@0.0.3.js",
      "github:CodeSeven/toastr@2.1.3/toastr.js",
      "github:ajaxorg/ace-builds@1.2.6.js",
      "github:ajaxorg/ace-builds@1.2.6/ace.js",
      "github:ajaxorg/ace-builds@1.2.6/ext-language_tools.js",
      "github:github/fetch@0.10.1.js",
      "github:github/fetch@0.10.1/fetch.js",
      "github:twbs/bootstrap@3.3.7.js",
      "github:twbs/bootstrap@3.3.7/css/bootstrap.css!github:systemjs/plugin-text@0.0.3.js",
      "github:twbs/bootstrap@3.3.7/js/bootstrap.js",
      "main.js",
      "nav-bar.html!github:systemjs/plugin-text@0.0.3.js",
      "nav-bar.js",
      "npm:aurelia-animator-css@1.0.1.js",
      "npm:aurelia-animator-css@1.0.1/aurelia-animator-css.js",
      "npm:aurelia-binding@1.2.1.js",
      "npm:aurelia-binding@1.2.1/aurelia-binding.js",
      "npm:aurelia-bootstrapper@1.0.1.js",
      "npm:aurelia-bootstrapper@1.0.1/aurelia-bootstrapper.js",
      "npm:aurelia-dependency-injection@1.3.0.js",
      "npm:aurelia-dependency-injection@1.3.0/aurelia-dependency-injection.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/ai-dialog-body.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/ai-dialog-footer.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/ai-dialog-header.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/ai-dialog.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/attach-focus.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/aurelia-dialog.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-configuration.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-controller.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-options.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-renderer.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-result.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/dialog-service.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/lifecycle.js",
      "npm:aurelia-dialog@1.0.0-beta.3.0.1/renderer.js",
      "npm:aurelia-event-aggregator@1.0.1.js",
      "npm:aurelia-event-aggregator@1.0.1/aurelia-event-aggregator.js",
      "npm:aurelia-fetch-client@1.1.1.js",
      "npm:aurelia-fetch-client@1.1.1/aurelia-fetch-client.js",
      "npm:aurelia-framework@1.0.8.js",
      "npm:aurelia-framework@1.0.8/aurelia-framework.js",
      "npm:aurelia-history-browser@1.0.0.js",
      "npm:aurelia-history-browser@1.0.0/aurelia-history-browser.js",
      "npm:aurelia-history@1.0.0.js",
      "npm:aurelia-history@1.0.0/aurelia-history.js",
      "npm:aurelia-loader-default@1.0.1.js",
      "npm:aurelia-loader-default@1.0.1/aurelia-loader-default.js",
      "npm:aurelia-loader@1.0.0.js",
      "npm:aurelia-loader@1.0.0/aurelia-loader.js",
      "npm:aurelia-logging-console@1.0.0.js",
      "npm:aurelia-logging-console@1.0.0/aurelia-logging-console.js",
      "npm:aurelia-logging@1.3.1.js",
      "npm:aurelia-logging@1.3.1/aurelia-logging.js",
      "npm:aurelia-metadata@1.0.3.js",
      "npm:aurelia-metadata@1.0.3/aurelia-metadata.js",
      "npm:aurelia-pal-browser@1.1.0.js",
      "npm:aurelia-pal-browser@1.1.0/aurelia-pal-browser.js",
      "npm:aurelia-pal@1.3.0.js",
      "npm:aurelia-pal@1.3.0/aurelia-pal.js",
      "npm:aurelia-path@1.1.1.js",
      "npm:aurelia-path@1.1.1/aurelia-path.js",
      "npm:aurelia-polyfills@1.2.0.js",
      "npm:aurelia-polyfills@1.2.0/aurelia-polyfills.js",
      "npm:aurelia-route-recognizer@1.1.0.js",
      "npm:aurelia-route-recognizer@1.1.0/aurelia-route-recognizer.js",
      "npm:aurelia-router@1.2.0.js",
      "npm:aurelia-router@1.2.0/aurelia-router.js",
      "npm:aurelia-task-queue@1.2.0.js",
      "npm:aurelia-task-queue@1.2.0/aurelia-task-queue.js",
      "npm:aurelia-templating-binding@1.2.0.js",
      "npm:aurelia-templating-binding@1.2.0/aurelia-templating-binding.js",
      "npm:aurelia-templating-resources@1.2.0.js",
      "npm:aurelia-templating-resources@1.2.0/abstract-repeater.js",
      "npm:aurelia-templating-resources@1.2.0/analyze-view-factory.js",
      "npm:aurelia-templating-resources@1.2.0/array-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.2.0/attr-binding-behavior.js",
      "npm:aurelia-templating-resources@1.2.0/aurelia-hide-style.js",
      "npm:aurelia-templating-resources@1.2.0/aurelia-templating-resources.js",
      "npm:aurelia-templating-resources@1.2.0/binding-mode-behaviors.js",
      "npm:aurelia-templating-resources@1.2.0/binding-signaler.js",
      "npm:aurelia-templating-resources@1.2.0/compose.js",
      "npm:aurelia-templating-resources@1.2.0/css-resource.js",
      "npm:aurelia-templating-resources@1.2.0/debounce-binding-behavior.js",
      "npm:aurelia-templating-resources@1.2.0/dynamic-element.js",
      "npm:aurelia-templating-resources@1.2.0/focus.js",
      "npm:aurelia-templating-resources@1.2.0/hide.js",
      "npm:aurelia-templating-resources@1.2.0/html-resource-plugin.js",
      "npm:aurelia-templating-resources@1.2.0/html-sanitizer.js",
      "npm:aurelia-templating-resources@1.2.0/if.js",
      "npm:aurelia-templating-resources@1.2.0/map-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.2.0/null-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.2.0/number-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.2.0/repeat-strategy-locator.js",
      "npm:aurelia-templating-resources@1.2.0/repeat-utilities.js",
      "npm:aurelia-templating-resources@1.2.0/repeat.js",
      "npm:aurelia-templating-resources@1.2.0/replaceable.js",
      "npm:aurelia-templating-resources@1.2.0/sanitize-html.js",
      "npm:aurelia-templating-resources@1.2.0/set-repeat-strategy.js",
      "npm:aurelia-templating-resources@1.2.0/show.js",
      "npm:aurelia-templating-resources@1.2.0/signal-binding-behavior.js",
      "npm:aurelia-templating-resources@1.2.0/throttle-binding-behavior.js",
      "npm:aurelia-templating-resources@1.2.0/update-trigger-binding-behavior.js",
      "npm:aurelia-templating-resources@1.2.0/with.js",
      "npm:aurelia-templating-router@1.0.1.js",
      "npm:aurelia-templating-router@1.0.1/aurelia-templating-router.js",
      "npm:aurelia-templating-router@1.0.1/route-href.js",
      "npm:aurelia-templating-router@1.0.1/route-loader.js",
      "npm:aurelia-templating-router@1.0.1/router-view.js",
      "npm:aurelia-templating@1.4.0.js",
      "npm:aurelia-templating@1.4.0/aurelia-templating.js",
      "npm:font-awesome@4.7.0/css/font-awesome.min.css!github:systemjs/plugin-text@0.0.3.js",
      "npm:jquery@3.1.1.js",
      "npm:jquery@3.1.1/dist/jquery.js",
      "utility-classes/string-helpers.js",
      "welcome.html!github:systemjs/plugin-text@0.0.3.js",
      "welcome.js"
    ]
  }
});