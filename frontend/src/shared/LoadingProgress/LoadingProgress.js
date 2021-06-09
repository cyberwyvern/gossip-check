import LinearProgress from '@material-ui/core/LinearProgress';
import StoreContext from '@stores/StoreContext';
import { observer } from 'mobx-react';
import React, { useContext } from 'react';

function LoadingProgress() {
  const store = useContext(StoreContext).loadingProgressStore;
  const visibility = store.isLoadingInProggress ? 'visible' : 'hidden';

  return <LinearProgress color="secondary" style={{ visibility: visibility }} />
}

export default observer(LoadingProgress);
