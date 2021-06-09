import Collapse from '@material-ui/core/Collapse';
import IconButton from '@material-ui/core/IconButton';
import Link from '@material-ui/core/Link';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Paper from '@material-ui/core/Paper';
import { withStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Typography from '@material-ui/core/Typography';
import KeyboardArrowDown from '@material-ui/icons/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import React, { Fragment, useState } from 'react';

const StyledTableCell = withStyles((theme) => ({
  head: {
    backgroundColor: theme.palette.secondary.dark,
    color: theme.palette.common.white,
  }
}))(TableCell);

function Row({ row }) {
  const [open, setOpen] = useState(false);
  const {
    articleUrl,
    stance,
    factuality,
    bias,
    mediaType,
    popularity,
    mbfcCredibilityRating,
    reasoning,
    country,
    worldPressFreedomRank,
    mbfcPageUrl
  } = row;

  return (
    <Fragment>
      <TableRow key={articleUrl}>
        <TableCell>
          <IconButton aria-label="expand row" size="small" onClick={() => setOpen(!open)}>
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDown />}
          </IconButton>
        </TableCell>
        <TableCell>
          <Link href={articleUrl} target="_blank">{articleUrl}</Link>
        </TableCell>
        <TableCell>{stance}</TableCell>
      </TableRow>
      <TableRow>
        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }}/>
        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }}>
          <Collapse in={open} timeout="auto" unmountOnExit>
            <Typography variant="h6">
              MBFC Report
            </Typography>
            <List dense>
              {factuality && <ListItem>
                <ListItemText>Factuality: {factuality}</ListItemText>
              </ListItem>}
              {bias && <ListItem>
                <ListItemText>Bias: {bias}</ListItemText>
              </ListItem>}
              {mediaType && <ListItem>
                <ListItemText>Media type: {mediaType}</ListItemText>
              </ListItem>}
              {popularity && <ListItem>
                <ListItemText>Popularity: {popularity}</ListItemText>
              </ListItem>}
              {mbfcCredibilityRating && <ListItem>
                <ListItemText>Credibility: {mbfcCredibilityRating}</ListItemText>
              </ListItem>}
              {reasoning && <ListItem>
                <ListItemText>Reasoning: {reasoning}</ListItemText>
              </ListItem>}
              {country && <ListItem>
                <ListItemText>Country: {country}</ListItemText>
              </ListItem>}
              {worldPressFreedomRank && <ListItem>
                <ListItemText>World press freedom rank: {worldPressFreedomRank}</ListItemText>
              </ListItem>}
              {mbfcPageUrl && <ListItem>
                <ListItemText>
                  <span>MBFC page URL: </span>
                  <Link href={mbfcPageUrl} target="_blank">{mbfcPageUrl}</Link>
                </ListItemText>
              </ListItem>}
            </List>
          </Collapse>
        </TableCell>
      </TableRow>
    </Fragment>
  );
}

export default function SearchResultsTable({ rows }) {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <StyledTableCell width={1} />
            <StyledTableCell>Related article</StyledTableCell>
            <StyledTableCell>Stance</StyledTableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rows.map((row) => <Row row={row} />)}
        </TableBody>
      </Table>
    </TableContainer>
  );
}