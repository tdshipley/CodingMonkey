export class PageObjectLogin {

  constructor() {

  }

  setUsername(username) {
      element(by.id('username')).sendKeys(username);
  }

  setPassword(password) {
      element(by.id('password')).sendKeys(password);
  }

  pressLoginButton() {
      element(by.id('login')).click();
  }
}
