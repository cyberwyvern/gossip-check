import { makeAutoObservable } from "mobx";

export default class LoadingProgressStore {
  pendingRequestsNumber = 0;

  get isLoadingInProggress() {
    return this.pendingRequestsNumber > 0;
  }

  constructor() {
    makeAutoObservable(this);
  }

  incrementPendingRequestsNumber = () => this.pendingRequestsNumber++;
  decrementPendingRequestsNumber = () => this.pendingRequestsNumber--;
}
