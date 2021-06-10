import { ApiClient, ErrorInterceptor, WaitingInterceptor } from '@api';
import config from '@config';
import { MainPageStore } from '@pages/MainPage';
import { AlertStore } from '@shared/Alert';
import { LoadingProgressStore } from '@shared/LoadingProgress';
import { VerdictPopupStore } from '@shared/VerdictPopup';

class RootStore {
  constructor() {
    this.alertStore = new AlertStore();
    this.loadingProgressStore = new LoadingProgressStore();
    this.apiClient = ApiClient;
    this.apiClient = ErrorInterceptor(this.apiClient, this);
    this.apiClient = WaitingInterceptor(this.apiClient, this);
    this.apiClient = this.apiClient(config.API_URL);

    this.mainPageStore = new MainPageStore(this.apiClient);
    this.verdictPopupStore = new VerdictPopupStore();
  }
}

export default new RootStore();
