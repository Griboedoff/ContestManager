import React from 'react';
import { Link } from 'react-router-dom';
import { ListGroup, ListGroupItem, Spinner } from 'reactstrap';
import { ContestOptions } from '../../Enums/ContestOptions';
import WithContests from '../HOC/WithContests';
import WithUser from '../HOC/WithUser';

const ContestList = ({ fetching, contests }) => {
    if (fetching) {
        return <Spinner style={{ width: '3rem', height: '3rem' }} />;
    }

    const ongoing = contests.filter(c => (c.options & ContestOptions.Finished) !== ContestOptions.Finished);
    const finished = contests.filter(c => (c.options & ContestOptions.Finished) === ContestOptions.Finished);

    const toListItem = c => (
        <ListGroupItem tag={Link} to={`contests/${c.id}`} id={c.id} key={c.id}>
            {c.title}
        </ListGroupItem>);
    
    return <>
        {ongoing.length !== 0 && <>
            <h3>Текущие</h3>
            <ListGroup className="mb-3">{ongoing.map(toListItem)}</ListGroup>
        </>}
        {finished.length !== 0 && <>
            <h3>Прошедшие</h3>
            <ListGroup>{finished.map(toListItem)}</ListGroup>
        </>}
    </>;
};

export default WithUser(WithContests(ContestList));
