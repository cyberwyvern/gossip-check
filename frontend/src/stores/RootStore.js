import { ApiClient, ErrorInterceptor, WaitingInterceptor } from '@api';
import config from '@config';
import { AlertStore } from '@shared/Alert';
import { LoadingProgressStore } from '@shared/LoadingProgress';

class RootStore {
  constructor() {
    this.alertStore = new AlertStore(config.API_URL);
    this.loadingProgressStore = new LoadingProgressStore();

    this.apiClient = ApiClient();
    this.apiClient = ErrorInterceptor(this.apiClient, this);
    this.apiClient = WaitingInterceptor(this.apiClient, this);
  }
}

export default new RootStore();
