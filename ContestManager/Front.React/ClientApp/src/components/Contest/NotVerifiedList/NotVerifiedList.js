import React, { useState, useEffect } from 'react';
import { Alert, Button, Form, FormGroup, Input, Label, ListGroup, ListGroupItem } from 'reactstrap';
import { CenterSpinner } from '../../CenterSpinner';
import { Participant } from '../ParticipantsList';
import { get, post } from '../../../Proxy';

const getNotVerified = async (contestId, setParticipants, setFetching) => {
    const resp = await get(`contests/${contestId}/participants?onlyNotVerified=true`);
    if (resp.ok) {
        setParticipants(await resp.json());
    }
    setFetching(false);
};

export const NotVerifiedList = ({ contestId }) => {
    const [fetching, setFetching] = useState(true);
    const [participants, setParticipants] = useState([]);
    useEffect(() => {
        getNotVerified(contestId, setParticipants, setFetching);
    }, [contestId]);

    if (fetching)
        return <CenterSpinner />;

    if (participants.length === 0)
        return <Alert>Все подтверждены</Alert>;

    return <>
        <ListGroup>
            {participants.map(p => <ParticipantWithButton
                key={p.id}
                participant={p}
                onSuccess={async () => await getNotVerified(contestId, setParticipants, setFetching)} />)}
        </ListGroup>
    </>;
};

const ParticipantWithButton = ({ participant, onSuccess }) => {
    const [verification, setVerification] = useState(participant.verification);

    return <ListGroupItem key={participant.id}>
        <Participant participant={participant} />
        <Form className="mt-3" inline>
            <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                <Label className="mr-sm-2">Подтверждение</Label>
                <Input type="text"
                       name="verification"
                       value={verification}
                       onChange={e => setVerification(e.target.value)} />
            </FormGroup>
            <Button color="success" onClick={async () => {
                if (!verification)
                    return false;

                const resp = await post(`contestAdmin/${participant.contestId}/verify`, {
                    participantId: participant.id,
                    verification
                });
                if (resp.ok)
                    await onSuccess();
            }}>Подтвердить</Button>
        </Form>

    </ListGroupItem>;
};
NotVerifiedList.displayName = 'NotVerifiedList';

