import Collapse from '@material-ui/core/Collapse';
import IconButton from '@material-ui/core/IconButton';
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
  return (
    <Fragment>
      <TableRow key={row.source}>
        <TableCell>
          <IconButton aria-label="expand row" size="small" onClick={() => setOpen(!open)}>
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDown />}
          </IconButton>
        </TableCell>
        <TableCell>{row.source}</TableCell>
        <TableCell>{row.stance}</TableCell>
      </TableRow>
      <TableRow>
        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={2}>
          <Collapse in={open} timeout="auto" unmountOnExit>
            <List>
              <ListItem>
                <ListItemText>test</ListItemText>
              </ListItem>
              <ListItem>
                <ListItemText>test</ListItemText>
              </ListItem>
              <ListItem>
                <ListItemText>test</ListItemText>
              </ListItem>
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
            <StyledTableCell>Article</StyledTableCell>
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