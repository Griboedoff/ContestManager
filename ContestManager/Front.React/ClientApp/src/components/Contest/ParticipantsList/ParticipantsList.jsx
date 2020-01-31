import React, { useState } from 'react';
import { Col, Container, ListGroup, ListGroupItem, Row } from 'reactstrap';
import { UserRole } from '../../../Enums/UserRole';
import WithParticipants from '../../HOC/WithParticipants';
import WithUser from '../../HOC/WithUser';
import { EditParticipantModal } from '../ParticipateModal';

export const ParticipantsList = ({ contest, user, participants }) => {
    return <>
        <ListGroup>
            {participants.map(p => (
                <ListGroupItem key={p.id}>
                    <Participant participant={p}
                                 withEdit={user && (user.role === UserRole.Admin || user.id === p.userId)}
                                 contest={contest} />
                </ListGroupItem>
            ))}
        </ListGroup>
    </>;
};
ParticipantsList.displayName = 'ParticipantsList';

const Participant = ({ participant, contest, withEdit }) => {
    const user = participant.userSnapshot;
    const [edit, setEdit] = useState(false);

    return <Container>
        <Row className="d-flex align-items-baseline justify-content-between mb-2">
            <h4>{user.name}</h4>
            <div>
                {withEdit && <span className="d-block link" onClick={() => setEdit(true)}>редактировать</span>}
                <small>{user.class} класс</small>
            </div>
        </Row>
        <Row>
            <Col sm={6}>{user.city}, {user.school}</Col>
            <Col sm={6}>Тренер: {user.coach}</Col>
        </Row>

        {edit && <EditParticipantModal
            isOpen={edit}
            participant={participant}
            contest={contest}
            close={() => {
                setEdit(false);
            }} />}
    </Container>;
};
Participant.displayName = 'Participant';

export default WithUser(WithParticipants(ParticipantsList));
