import React from 'react';
import { Col, Container, ListGroup, ListGroupItem, Row } from 'reactstrap';
import { UserRole } from '../../../Enums/UserRole';
import WithParticipants from '../../HOC/WithParticipants';
import WithUser from '../../HOC/WithUser';

export const ParticipantsList = ({ contest, user, participants }) => {
    return <>
        <ListGroup>
            {participants.map(p => (
                <ListGroupItem key={p.id}>
                    <Participant participant={p} withEdit={user.role === UserRole.Admin || user.id === p.userId}/>
                </ListGroupItem>
            ))}
        </ListGroup>
        {/*<ParticipateModal user={user} contest={contest} saveTitle="Обновить данные" />*/}
    </>;
};

const Participant = ({ participant, withEdit }) => {
    const user = participant.userSnapshot;

    return <Container>
        <Row className="d-flex align-items-baseline justify-content-between mb-2">
            <h4>{user.name}</h4>
            <div>
                {withEdit && <span className="d-block link">редактировать</span>}
                <small>{user.class} класс</small>
            </div>
        </Row>
        <Row>
            <Col sm={6}>{user.city}, {user.school}</Col>
            <Col sm={6}>Тренер: {user.coach}</Col>
        </Row>
    </Container>;
};

export default WithUser(WithParticipants(ParticipantsList));
