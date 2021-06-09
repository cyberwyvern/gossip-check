import Backdrop from '@material-ui/core/Backdrop';
import Button from '@material-ui/core/Button';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import SearchIcon from '@material-ui/icons/Search';
import StoreContext from '@stores/StoreContext';
import { observer } from "mobx-react";
import { React, useContext, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import styles from './styles/MainPage.module.css';
import SearchResultsTable from './SearchResultTable';
import VerdictPopup from './VerdictPopup';

const useStyles = makeStyles((theme) => ({
  backdrop: {
    zIndex: theme.zIndex.drawer + 1,
  },
}));

function MainPage() {
  const classes = useStyles();
  const store = useContext(StoreContext).mainPageStore;
  const [backdropOpen, setBackdropOpen] = useState(false);
  const { handleSubmit, control, errors } = useForm({ mode: 'onChange' });
  const onSubmit = async ({ urlOrClaim }) => {
    await store.verifyArticle(urlOrClaim);
    setBackdropOpen(true);
  }

  return (
    <div className={styles['root']}>
      <Backdrop className={classes.backdrop} open={backdropOpen} onClick={() => setBackdropOpen(false)}>
        {store.searchResults && <VerdictPopup verdict={store.searchResults.verdict} />}
      </Backdrop>
      <div className={styles['search-field']}>
        <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
          <Controller
            name="urlOrClaim"
            control={control}
            as={
              <TextField
                label="Input article URL or claim"
                variant="outlined"
                size="small"
                error={!!errors.urlOrClaim}
                helperText={errors.urlOrClaim && errors.urlOrClaim.message}
                fullWidth
              />
            }
            defaultValue=""
            rules={{
              required: "This field is required",
              maxLength: {
                value: 2048,
                message: "maximum length is 2048"
              }
            }}
          />
          <Button
            variant="contained"
            color="primary"
            startIcon={<SearchIcon />}
            type="submit">
            Search
          </Button>
        </form>
      </div>
      {store.searchResults && <SearchResultsTable rows={store.searchResults.relatedArticles} />}
    </div>
  );
}

export default observer(MainPage);