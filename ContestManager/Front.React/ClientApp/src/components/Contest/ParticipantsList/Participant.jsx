import React, { useState } from 'react';

import { Col, Container, Row } from 'reactstrap';
import { EditParticipantModal } from '../ParticipateModal';

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
            {user.coach && <Col sm={6}>Тренер: {user.coach}</Col>}
        </Row>
        <Row>
            <Col sm={6}>{user.city}, {user.school}</Col>
            {user.coach && <Col sm={6}>Тренер: {user.coach}</Col>}
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

export { Participant };
