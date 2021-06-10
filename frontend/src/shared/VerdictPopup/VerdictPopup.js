import { Backdrop, Box, Button, makeStyles } from '@material-ui/core';
import { green, orange, red } from '@material-ui/core/colors';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import HelpIcon from '@material-ui/icons/Help';
import ThumbDownIcon from '@material-ui/icons/ThumbDown';
import ThumbsUpDownIcon from '@material-ui/icons/ThumbsUpDown';
import ThumbUpIcon from '@material-ui/icons/ThumbUp';
import StoreContext from '@stores/StoreContext';
import { observer } from 'mobx-react-lite';
import { Fragment, React, useContext } from 'react';
import styles from './VerdictPopup.module.css';

const useStyles = makeStyles((theme) => ({
  backdrop: {
    zIndex: theme.zIndex.drawer + 1,
  },
}));

const mostLikelyFake = () =>
  <Fragment>
    <ThumbDownIcon style={{ fontSize: 100, color: red[500] }} />
    <ThumbDownIcon style={{ fontSize: 100, color: red[500] }} />
  </Fragment>

const likelyFake = () =>
  <Fragment>
    <ThumbDownIcon style={{ fontSize: 100, color: red[500] }} />
  </Fragment>

const questionable = () =>
  <Fragment>
    <ThumbsUpDownIcon style={{ fontSize: 100, color: orange[500] }} />
  </Fragment>

const likelyTrue = () =>
  <Fragment>
    <ThumbUpIcon style={{ fontSize: 100, color: green[500] }} />
  </Fragment>

const mostLikelyTrue = () =>
  <Fragment>
    <ThumbUpIcon style={{ fontSize: 100, color: green[500] }} />
    <ThumbUpIcon style={{ fontSize: 100, color: green[500] }} />
  </Fragment>

const couldNotDetermine = () =>
  <Fragment>
    <HelpIcon style={{ fontSize: 100 }} />
  </Fragment>

const icon = {
  'MostLikelyTrue': mostLikelyTrue,
  'LikelyTrue': likelyTrue,
  'Questionable': questionable,
  'LikelyFake': likelyFake,
  'MostLikelyFake': mostLikelyFake,
  'CouldNotDetermine': couldNotDetermine
}

function VerdictPopup() {
  const classes = useStyles();
  const { verdictPopupStore: { isActive, verdict, header, text, toggle } } = useContext(StoreContext);

  return (
    <Backdrop className={classes.backdrop} open={isActive}>
      <Paper elevation={3} className={styles['root']}>
        <Box>{icon[verdict] && icon[verdict]()}</Box>
        <Typography variant="h5">{header}</Typography>
        <Typography>{text}</Typography>
        <div className={styles['close']}>
          <Button
            variant="contained"
            color="secondary"
            size="small"
            onClick={() => toggle()}>
            Close
          </Button>
        </div>
      </Paper>
    </Backdrop>
  );
}

export default observer(VerdictPopup)