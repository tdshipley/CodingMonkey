var gulp = require('gulp');
var shell = require('gulp-shell');

gulp.task('dnu-build', shell.task(['dnu build']))