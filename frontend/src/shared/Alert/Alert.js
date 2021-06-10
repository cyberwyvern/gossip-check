import { IconButton } from '@material-ui/core';
import Snackbar from '@material-ui/core/Snackbar';
import CloseIcon from '@material-ui/icons/Close';
import MuiAlert from '@material-ui/lab/Alert';
import StoreContext from '@stores/StoreContext';
import { observer } from "mobx-react";
import { Fragment, React, useContext } from 'react';

function Alert() {
  const store = useContext(StoreContext).alertStore;

  const handleClose = () => {
    store.closeAlert();
  }

  return (
    <Snackbar
      key={store.alertText}
      open={store.alertOpened}
      anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
      autoHideDuration={store.showDurationMs}
      onClose={handleClose}>
      <MuiAlert
        variant="filled"
        elevation={6}
        severity={store.alertSeverity}
        action={
          <Fragment>
            <IconButton size="small" color="inherit" onClick={handleClose}>
              <CloseIcon fontSize="inherit" />
            </IconButton>
          </Fragment>
        }>
        {store.alertText}
      </MuiAlert>
    </Snackbar>
  );
}

export default observer(Alert);
