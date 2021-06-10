import { makeAutoObservable, runInAction } from "mobx";
//import { response as mock } from './mockResponse';

export default class MainPageStore {
  searchResults;

  constructor(api) {
    this.api = api;
    makeAutoObservable(this, { api: false });
  }

  async verifyArticle(urlOrClaim) {
    runInAction(() => this.searchResults = null);

    let response = await this.api.post('/gossip-check/verify', { textOrigin: urlOrClaim });
    //let response = { data: mock };
    runInAction(() => this.searchResults = response.data);
  }
}
