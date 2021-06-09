import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import SearchIcon from '@material-ui/icons/Search';
import React from 'react';
import styles from './MainPage.module.css';
import SearchResultsTable from './SearchResults/SearchResultTable';

export default function MainPage() {
  return (
    <div className={styles['root']}>
      <div className={styles['search-field']}>
        <TextField label="Input article URL or claim" variant="outlined" fullWidth />
        <Button
          variant="contained"
          color="primary"
          size="large"
          startIcon={<SearchIcon />}>
          Search
        </Button>
      </div>
      <SearchResultsTable rows={[{source: 'test', stance: 'test'},{source: 'test', stance: 'test'}]} />
    </div>
  );
}