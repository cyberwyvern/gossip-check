import IconButton from '@material-ui/core/IconButton';
import InputAdornment from '@material-ui/core/InputAdornment';
import TextField from '@material-ui/core/TextField';
import SearchIcon from '@material-ui/icons/Search';
import { VerdictPopup } from '@shared/VerdictPopup';
import StoreContext from '@stores/StoreContext';
import { observer } from "mobx-react";
import { React, useContext } from 'react';
import { Controller, useForm } from 'react-hook-form';
import styles from './MainPage.module.css';
import { SearchResultsTable } from './SearchResultTable';

function MainPage() {
  const { mainPageStore, verdictPopupStore } = useContext(StoreContext);
  const { handleSubmit, control, errors } = useForm({ mode: 'onChange' });

  const onSubmit = async ({ urlOrClaim }) => {
    await mainPageStore.verifyArticle(urlOrClaim);
    verdictPopupStore.toggle(mainPageStore.searchResults.verdict);
  }

  return (
    <div className={styles['root']}>
      <VerdictPopup />
      <div className={styles['search-field']}>
        <form autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
          <Controller
            name="urlOrClaim"
            control={control}
            as={
              <TextField
                label="Input article URL or claim"
                variant="outlined"
                error={!!errors.urlOrClaim}
                helperText={errors.urlOrClaim && errors.urlOrClaim.message}
                fullWidth
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton type="submit" edge="end">
                        <SearchIcon />
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
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
        </form>
      </div>
      {mainPageStore.searchResults && <SearchResultsTable rows={mainPageStore.searchResults.relatedArticles} />}
    </div>
  );
}

export default observer(MainPage);