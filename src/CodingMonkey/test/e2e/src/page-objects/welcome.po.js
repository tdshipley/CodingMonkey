export class PageObjectWelcome {

  constructor() {

  }

  getGreeting() {
    return element(by.tagName('h1')).getText();
  }

  openAlertDialog() {
    return browser.wait(() => {
      this.pressSubmitButton();

      return browser.switchTo().alert().then(
        // use alert.accept instead of alert.dismiss which results in a browser crash
        function(alert) { alert.accept(); return true; },
        function() { return false; }
      );
    });
  }
}
