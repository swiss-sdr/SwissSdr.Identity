/// <binding ProjectOpened="watch" />
var gulp = require("gulp");

var watch = require("gulp-watch");
var sass = require("gulp-sass");
var sourcemaps = require("gulp-sourcemaps");
var plumber = require("gulp-plumber");
var filter = require("gulp-filter");
var autoprefixer = require("gulp-autoprefixer");
var concat = require("gulp-concat");
var rename = require("gulp-rename");
var cleancss = require("gulp-clean-css");
var uglify = require("gulp-uglify");

var fonts = [
	"node_modules/material-design-icons/iconfont/MaterialIcons-Regular.eot",
	"node_modules/material-design-icons/iconfont/MaterialIcons-Regular.woff2",
	"node_modules/material-design-icons/iconfont/MaterialIcons-Regular.woff",
	"node_modules/material-design-icons/iconfont/MaterialIcons-Regular.ttf"
];
var styles = [
	"wwwroot/css/app.css"
];
var scripts = [
	"node_modules/material-design-lite/dist/material.min.js"
];

gulp.task("watch", function () {
	gulp.watch("styles/**/*.scss", ["sass"]);
});

gulp.task("sass", function () {
	return gulp.src("styles/**/*.scss")
		.pipe(plumber())
		.pipe(sourcemaps.init())
		.pipe(sass().on("error", sass.logError))
		.pipe(autoprefixer({ browsers: ["last 4 versions"] }))
		.pipe(sourcemaps.write("./", { includeContent: false }))
		.pipe(gulp.dest("./wwwroot/css/"));
});

gulp.task("prod-fonts", function () {
	return gulp.src(fonts)
		.pipe(gulp.dest("./wwwroot/fonts"));
});

gulp.task("prod-css", function () {
	return gulp.src(styles)
		.pipe(concat("app.css"))
		.pipe(autoprefixer({ browsers: ["last 4 versions"] }))
		.pipe(cleancss())
		.pipe(rename("app.min.css"))
		.pipe(gulp.dest("./wwwroot/css/"));
});

gulp.task("prod-js", function () {
	return gulp.src(scripts)
		.pipe(concat("app.js"))
		.pipe(uglify())
		.pipe(rename("app.min.js"))
		.pipe(gulp.dest("./wwwroot/js"));
});

gulp.task("prod", ["prod-fonts", "prod-css", "prod-js"]);
