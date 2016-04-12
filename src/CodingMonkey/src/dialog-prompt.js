import {inject} from 'aurelia-framework';
import {DialogController} from "aurelia-dialog";

@inject(DialogController)
export class DialogPrompt {

  constructor(controller) {
    this.controller = controller;
    this.answer = null;
    this.vm = this;

    controller.settings.lock = false;
  }

  activate(dialogModel) {
    this.vm.questionHeader = dialogModel.questionHeader;
    this.vm.question = dialogModel.question;
    this.vm.dismissText = dialogModel.dismissText;
    this.vm.confirmText = dialogModel.confirmText;
  }
}