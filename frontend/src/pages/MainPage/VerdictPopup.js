import { green, orange, red } from '@material-ui/core/colors';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import HelpIcon from '@material-ui/icons/Help';
import ThumbDownIcon from '@material-ui/icons/ThumbDown';
import ThumbsUpDownIcon from '@material-ui/icons/ThumbsUpDown';
import ThumbUpIcon from '@material-ui/icons/ThumbUp';
import { Fragment } from 'react';
import styles from './styles/VerdictPopup.module.css';

const mostLikelyFake = () =>
  <Fragment>
    <ThumbDownIcon style={{ fontSize: 100, color: red[500] }} />
    <ThumbDownIcon style={{ fontSize: 100, color: red[500] }} />
  </Fragment>

const likelyFake = () =>
  <Fragment>
    <ThumbDownIcon style={{ fontSize: 200, color: red[500] }} />
  </Fragment>

const hardToSay = () =>
  <Fragment>
    <ThumbsUpDownIcon style={{ fontSize: 200, color: orange[500] }} />
  </Fragment>

const likelyTrue = () =>
  <Fragment>
    <ThumbUpIcon style={{ fontSize: 200, color: green[500] }} />
  </Fragment>

const mostLikelyTrue = () =>
  <Fragment>
    <ThumbUpIcon style={{ fontSize: 100, color: green[500] }} />
    <ThumbUpIcon style={{ fontSize: 100, color: green[500] }} />
  </Fragment>

const unableToDetermine = () =>
  <Fragment>
    <HelpIcon style={{ fontSize: 200 }} />
  </Fragment>

const iconMap = {
  'MostLikelyTrue': mostLikelyTrue,
  'LikelyTrue': likelyTrue,
  'HardToSay': hardToSay,
  'LikelyFake': likelyFake,
  'MostLikelyFake': mostLikelyFake,
  'UnableToDetermine': unableToDetermine
}

const textMap = {
  'MostLikelyTrue': 'This article is most likely true',
  'LikelyTrue': 'This article is likely true',
  'HardToSay': 'It\'s hard to say whether this article true or fake',
  'LikelyFake': 'This article is likely fake',
  'MostLikelyFake': 'This article is most likely fake',
  'UnableToDetermine': 'Can\'t find related data so unable to determine verity of this article'
}

export default function VerdictPopup({ verdict }) {
  return (
    <Paper elevation={3} className={styles['root']}>
      <div>{iconMap[verdict]()}</div>
      <Typography variant="h5">
        {textMap[verdict]}
      </Typography>
    </Paper>
  );
}