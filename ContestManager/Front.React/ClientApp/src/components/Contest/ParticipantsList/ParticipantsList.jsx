import React from 'react';
import { ListGroup, ListGroupItem } from 'reactstrap';
import { UserRole } from '../../../Enums/UserRole';
import WithParticipants from '../../HOC/WithParticipants';
import WithUser from '../../HOC/WithUser';
import { Participant } from './Participant';

const ParticipantsList = ({ contest, user, participants }) => {
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

export default WithUser(WithParticipants(ParticipantsList));
