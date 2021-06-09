import 'fontsource-roboto';
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import './index.css';
import RootStore from './stores/RootStore';
import StoreContext from './stores/StoreContext';

ReactDOM.render(
    <StoreContext.Provider value={RootStore}>
      <App />
    </StoreContext.Provider>,
  document.getElementById('root')
);
