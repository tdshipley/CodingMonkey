var gulp = require('gulp');
var aureliabundler = require('aurelia-bundler');
var bundles = require('../bundles.js');

var bundler_config = {
  force: true,
  baseURL: './wwwroot',
  configPath: './wwwroot/config.js',
  bundles: bundles.bundles
};

gulp.task('bundle', ['unbundle', 'build'], function() {
    return aureliabundler.bundle(bundler_config);
});

gulp.task('unbundle', function() {
    return aureliabundler.unbundle(bundler_config);
});
