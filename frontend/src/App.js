import { MainPage } from '@pages/MainPage';
import { Alert } from "@shared/Alert";
import { observer } from "mobx-react";
import {
  BrowserRouter as Router,
  Route, Switch
} from "react-router-dom";
import styles from './App.module.css';

function App() {
  return (
    <Router>
      <div className={styles['root']}>
        <div className={styles['content']}>
          <div className={styles['page-content']}>
            <Switch>
              <Route path="/" component={MainPage} />
            </Switch>
          </div>
        </div>
      </div>
      <Alert />
    </Router>
  );
}

export default observer(App);
