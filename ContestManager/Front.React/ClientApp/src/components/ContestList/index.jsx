import React from 'react';
import { Link } from 'react-router-dom';
import { ListGroup, ListGroupItem, Spinner } from 'reactstrap';
import { ContestState } from '../../Enums/ContestsState';
import WithContests from '../HOC/WithContests';

class ContestList extends React.Component {
    renderContestsList() {
        const ongoing = this.props.contests.filter(c => c.state !== ContestState.Finished);
        const finished = this.props.contests.filter(c => c.state === ContestState.Finished);

        return <>
            {ongoing.length !== 0 && <>
                <h3>Текущие</h3>
                <ListGroup className="mb-3">
                    {ongoing.map(c => <ListGroupItem tag={Link}
                                                     to={`contests/${c.id}`}
                                                     id={c.id}>{c.title}</ListGroupItem>)}
                </ListGroup>
            </>}
            {finished.length !== 0 && <>
                <h3>Прошедшие</h3>
                <ListGroup>
                    {finished.map(c => <ListGroupItem tag={Link}
                                                      to={`contests/${c.id}`}
                                                      id={c.id}>{c.title}</ListGroupItem>)}
                </ListGroup>
            </>}
        </>;
    }

    render() {
        return this.props.fetching
            ? <Spinner style={{ width: '3rem', height: '3rem' }} />
            : this.renderContestsList();
    }
}

export default WithContests(ContestList);
