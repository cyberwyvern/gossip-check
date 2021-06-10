import { makeAutoObservable } from "mobx";

const textMap = {
  'MostLikelyTrue': ['Not fake news!', 'Based on the sources we checked and referenced this article against, it is most likely credible!'],
  'LikelyTrue': ['Not fake news!', 'Based on the sources we checked and referenced this article against, it is likely credible!'],
  'Questionable': ['Questionable!', 'Based on the sources we checked and referenced this article against, it looks questionable!'],
  'LikelyFake': ['Fake news!', 'Based on the sources we checked and referenced this article against, it is likely fake!'],
  'MostLikelyFake': ['Fake news!', 'Based on the sources we checked and referenced this article against, it is most likely fake!'],
  'CouldNotDetermine': ['Could not determine!', 'We could not found sufficient ammount of sources to check this article against!'],
};

export default class VerdictPopupStore {
  isActive = false;
  verdict = null;

  get header() {
    return textMap[this.verdict] && textMap[this.verdict][0];
  }

  get text() {
    return textMap[this.verdict] && textMap[this.verdict][1];
  }

  constructor() {
    makeAutoObservable(this);
  }

  toggle = (verdict) => {
    if (verdict) {
      this.verdict = verdict;
    }
    this.isActive = !this.isActive;
  }
}
