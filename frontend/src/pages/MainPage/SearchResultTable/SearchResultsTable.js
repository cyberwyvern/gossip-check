import { Box } from '@material-ui/core';
import Collapse from '@material-ui/core/Collapse';
import IconButton from '@material-ui/core/IconButton';
import Link from '@material-ui/core/Link';
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

const HeaderCell = withStyles((theme) => ({
  head: {
    backgroundColor: theme.palette.secondary.dark,
    color: theme.palette.common.white,
    fontWeight: 'bold'
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
        <TableCell>
          <Typography>{stance}</Typography>
        </TableCell>
      </TableRow>
      <TableRow key={articleUrl}>
        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={3}>
          <Collapse in={open} timeout="auto" unmountOnExit>
            <Box p={2} pb={3}>
              <Typography variant="h6" gutterBottom><b>MBFC Report</b></Typography>
              {factuality && <Typography variant="body2"><b>Factuality:</b> {factuality}</Typography>}
              {bias && <Typography variant="body2"><b>Bias:</b> {bias}</Typography>}
              {mediaType && <Typography variant="body2"><b>Media type:</b> {mediaType}</Typography>}
              {popularity && <Typography variant="body2"><b>Popularity:</b> {popularity}</Typography>}
              {mbfcCredibilityRating && <Typography variant="body2"><b>Credibility:</b> {mbfcCredibilityRating}</Typography>}
              {reasoning && <Typography variant="body2"><b>Reasoning:</b> {reasoning}</Typography>}
              {country && <Typography variant="body2"><b>Country:</b> {country}</Typography>}
              {worldPressFreedomRank && <Typography variant="body2"><b>World press freedom rank:</b> {worldPressFreedomRank}</Typography>}
              {mbfcPageUrl && <Typography variant="body2"><b>MBFC page URL:</b> <Link href={mbfcPageUrl} target="_blank">{mbfcPageUrl}</Link></Typography>}
            </Box>
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
            <HeaderCell colSpan={2}>Related article</HeaderCell>
            <HeaderCell>Stance</HeaderCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rows.map(row => <Row row={row} />)}
        </TableBody>
      </Table>
    </TableContainer>
  );
}