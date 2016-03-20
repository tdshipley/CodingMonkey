var gulp = require('gulp');
var shell = require('gulp-shell');

gulp.task('dnx-watch', shell.task(['dnx-watch web']))