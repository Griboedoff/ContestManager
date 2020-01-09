import React, { useState } from 'react';
import { Alert, Col, Container, FormText, Label, Modal, ModalBody, ModalHeader } from 'reactstrap';
import { patch } from '../../../Proxy';
import WithParticipants from '../../HOC/WithParticipants';
import { EditableUserData } from '../../UserPage';
import { AvGroup, AvInput } from 'availity-reactstrap-validation';

const EditParticipantModal = ({ isOpen, participant, contest, close, updateParticipant }) => {
    const [participateError, setParticipateError] = useState(null);
    const [verification, setVerification] = useState(participant.verification);

    const participate = (user) => {
        setParticipateError(null);
        patch(`contests/${participant.contestId}/updateParticipant`, { user, verification })
            .then(async resp => {
                if (!resp.ok) {
                    if (resp.status === 500)
                        throw new Error("Авторизируйтесь");
                    throw await resp.json();
                }
                updateParticipant({ ...participant, userSnapshot: user, verification });
                close();
            }).catch(err => setParticipateError(err));
    };
    const handleChange = async (event) => await setVerification(event.target.value);

    return <Modal isOpen={isOpen} toggle={close}>
        <ModalHeader toggle={close}>Обновите данные</ModalHeader>
        <ModalBody>
            <p>Исправьте ваши данные</p>
            {participateError && <Alert color="danger">{participateError}</Alert>}
            <Container>
                <EditableUserData user={participant.userSnapshot}
                                  saveTitle="Обновить данные"
                                  onSaveEverything={participate}
                                  small>
                    {contest.type === 0 && <AvGroup row>
                        <Label sm={4}>Подтверждение</Label>
                        <Col sm={8}>
                            <AvInput type="text" name="class" onChange={handleChange} value={verification} required />
                        </Col>
                    </AvGroup>}
                </EditableUserData>
            </Container>
        </ModalBody>
    </Modal>;
};

export default WithParticipants(EditParticipantModal);
