# Coding Monkey - ASP.NET & Aurelia

The Coding Monkey website is based off the [Aurelia Skeleton Navigation Sample App](https://github.com/aurelia/skeleton-navigation). The site has been implemented with the following stack / tech:

* ASP.NET Core - the backend framework
* EF Core - ORM framework
* SQLite - the DB
* Aurelia - the frontend framework
* Babel compiler - for ES6 JavaScript
* Bootstrap - for styling
* Font Awesome - for icons and spinners
* Karma/Protractor/Jasmine - for testing

## Required Secrets File

To run the code it expects a secrets file named ```appsettings.secrets.json``` in src\codingmonkey which is not checked in and you will need to create both locally and in any deployment environment. It contains details used when communicating with identity server and the initial user to seed to the DB. For example:

```
{
  "InitialUser": {
    "UserName": "myusername",
    "Email":    "myuseremail@test.com",
    "Password": "myuserspassword"
  },
  "IdentityServer": {
    "ClientId":     "client_id_of_app_for_CodingMonkey.IdentityServer",
    "ClientSecret": "shared_secret_to_send_in_requests_to_CodingMonkey.IdentityServer"
  }
}

```

## Running The App

To run the app, follow these steps.

1. Ensure that [NodeJS](http://nodejs.org/) is installed. This provides the platform on which the build tooling runs.
2. From the project folder, execute the following command:

  ```shell
  npm install
  ```
3. Ensure that [Gulp](http://gulpjs.com/) is installed globally. If you need to install it, use the following command:

  ```shell
  npm install -g gulp
  ```
  > **Note:** Gulp must be installed globally, but a local version will also be installed to ensure a compatible version is used for the project.
4. Ensure that [jspm](http://jspm.io/) is installed globally. If you need to install it, use the following command:

  ```shell
  npm install -g jspm
  ```
  > **Note:** jspm must be installed globally, but a local version will also be installed to ensure a compatible version is used for the project.

  > **Note:** jspm queries GitHub to install semver packages, but GitHub has a rate limit on anonymous API requests. It is advised that you configure jspm with your GitHub credentials in order to avoid problems. You can do this by executing `jspm registry config github` and following the prompts. If you choose to authorize jspm by an access token instead of giving your password (see GitHub `Settings > Personal Access Tokens`), `public_repo` access for the token is required.
5. Ensure that [bower](http://bower.io/) is installed globally. If you need to install it, use the following command:

  ```shell
  npm install -g bower
  ```
  > **Note:** bower must be installed globally, but a local version will also be installed to ensure a compatible version is used for the project.
6. Install the client-side dependencies with jspm:

  ```shell
  jspm install -y
  ```
  >**Note:** Windows users, if you experience an error of "unknown command unzip" you can solve this problem by doing `npm install -g unzip` and then re-running `jspm install`.
7. Install client-side dependencies managed by bower, use the following command:

  ```shell
  bower install
  ```
8. To recompile the JavaScript code on file save, execute the following command:

  ```shell
  gulp watch
  ```
9. To run the app start the ASP.NET Core website, either run in Visual Studio or at the command line execute the following command:

  ```shell
  dnx web
  ```

9. Browse to [http://localhost:<DNX PORT NO>](http://localhost:9000) to see the app. You can make changes in the code found under `src` and the browser should auto-refresh itself as you save files.

> The Skeleton App uses [BrowserSync](http://www.browsersync.io/) for automated page refreshes on code/markup changes concurrently across multiple browsers. If you prefer to disable the mirroring feature set the [ghostMode option](http://www.browsersync.io/docs/options/#option-ghostMode) to false

## Bundling
Bundling is performed by [Aurelia Bundler](http://github.com/aurelia/bundler). A gulp task is already configured for that. Use the following command to bundle the app:

  ```shell
    gulp bundle
  ```

You can also unbundle using the command bellow:

  ```shell
  gulp unbundle
  ```
#### Configuration
The configuration is done by ```bundles.js``` file.
##### Optional
Under ```options``` of ```dist/aurelia``` add ```rev: true``` to add bundle file revision/version.

## Running The Unit Tests

To run the unit tests, first ensure that you have followed the steps above in order to install all dependencies and successfully build the library. Once you have done that, proceed with these additional steps:

1. Ensure that the [Karma](http://karma-runner.github.io/) CLI is installed. If you need to install it, use the following command:

  ```shell
  npm install -g karma-cli
  ```
2. Install Aurelia libs for test visibility:

```shell
jspm install aurelia-framework
jspm install aurelia-http-client
jspm install aurelia-router
```
3. You can now run the tests with this command:

  ```shell
  karma start
  ```

## Running The E2E Tests
Integration tests are performed with [Protractor](http://angular.github.io/protractor/#/).

1. Place your E2E-Tests into the folder ```test/e2e/src```
2. Install the necessary webdriver

  ```shell
  gulp webdriver-update
  ```

3. Configure the path to the webdriver by opening the file ```protractor.conf.js``` and adjusting the ```seleniumServerJar``` property. Typically its only needed to adjust the version number.

4. Make sure your app runs and is accessible

  ```shell
  gulp watch
  ```

5. In another console run the E2E-Tests

  ```shell
  gulp e2e
  ```

## Exporting bundled production version
A gulp task is already configured for that. Use the following command to export the app:

  ```shell
    gulp export
  ```
The app will be exported into ```export``` directory preserving the directory structure.
#### Configuration
The configuration is done by ```bundles.js``` file.
In addition, ```export.js``` file is available for including individual files.

### Resources

[![Aurelia Gitter Room https://gitter.im/aurelia/discuss](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/aurelia/discuss?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[Aurelia](http://www.aurelia.io/)
[Aurelia Blog](http://blog.durandal.io/)
