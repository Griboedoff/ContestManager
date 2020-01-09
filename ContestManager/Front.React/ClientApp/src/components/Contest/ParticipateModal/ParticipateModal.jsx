import React, { useState } from 'react';
import { Alert, Col, Container, FormText, Label, Modal, ModalBody, ModalHeader } from 'reactstrap';
import { post } from '../../../Proxy';
import { EditableUserData } from '../../UserPage';
import { AvGroup, AvInput } from 'availity-reactstrap-validation';

export const ParticipateModal = ({ user, contest, close, setUser }) => {
    const [participateError, setParticipateError] = useState(null);
    const [verification, setVerification] = useState(null);

    const participate = () => {
        setParticipateError(null);
        post(`contests/${contest.id}/participate`, verification)
            .then(async resp => {
                if (!resp.ok) {
                    if (resp.status === 500)
                        throw new Error("Авторизируйтесь");
                    throw await resp.json();
                }

                close();
            }).catch(err => setParticipateError(err));
    };
    const handleChange = async (event) => await setVerification(event.target.value);

    return <Modal isOpen={true} toggle={close}>
        <ModalHeader toggle={close}>Принять участие</ModalHeader>
        <ModalBody>
            <p>
                Проверьте ваши данные и исправьте если необходимо
            </p>
            {participateError && <Alert color="danger">{participateError}</Alert>}
            <Container>
                <EditableUserData user={user} saveTitle={'Участвовать'} onSave={participate} small setUser={setUser}>
                    {contest.type === 0 && <AvGroup row>
                        <Label sm={4}>Подтверждение</Label>
                        <Col sm={8}>
                            <AvInput type="text" name="class" onChange={handleChange} value={verification} required />
                        </Col>
                        <Col>
                            <FormText>
                                Для участия в этой олимпиаде нужно подтверждение — ссылка на результаты или диплом.
                            </FormText>
                        </Col>
                    </AvGroup>}
                </EditableUserData>
            </Container>
        </ModalBody>
    </Modal>;
};
