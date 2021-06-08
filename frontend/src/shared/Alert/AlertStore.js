import { makeAutoObservable } from "mobx";

export default class AlertStore {
  alertText = '';
  alertSeverity = '';
  alertOpened = false;
  showDurationMs = 0;

  constructor() {
    makeAutoObservable(this);
  }

  showError = (text) => this.showAlert(text, 'error', 3000);
  showWarning = (text) => this.showAlert(text, 'warning', 3000);
  showInfo = (text) => this.showAlert(text, 'info', 3000);
  showSuccess = (text) => this.showAlert(text, 'success', 3000);

  showAlert(text, severity, ms) {
    this.alertText = text;
    this.alertSeverity = severity;
    this.showDurationMs = ms;
    this.alertOpened = true;
  }

  closeAlert() {
    this.alertOpened = false;
  }
}
