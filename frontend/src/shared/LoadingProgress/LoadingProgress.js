import LinearProgress from '@material-ui/core/LinearProgress';
import StoreContext from '@stores/StoreContext';
import { observer } from 'mobx-react';
import React, { useContext } from 'react';

function LoadingProgress() {
  const store = useContext(StoreContext).loadingProgressStore;

  return (
    <div>
      {store.isLoadingInProggress && <LinearProgress color="secondary" />}
    </div>
  );
}

export default observer(LoadingProgress);
