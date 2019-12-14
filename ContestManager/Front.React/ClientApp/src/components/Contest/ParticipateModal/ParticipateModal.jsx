import React, { useState } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button, Container, Alert } from 'reactstrap';
import { post } from '../../../Proxy';
import EditableUserData from '../../UserPage/EditableUserData';

export const ParticipateModal = ({ contestId, close }) => {
    const [participateError, setParticipateError] = useState(null);
    const participate = () => {
        setParticipateError(null);
        post(`contests/${contestId}/participate`)
            .then(async resp => {
                if (!resp.ok)
                    throw await resp.json();
                else
                    close();
            }).catch(err => setParticipateError(err));
    };

    return <Modal isOpen={true} toggle={close}>
        <ModalHeader toggle={close}>Принять участие</ModalHeader>
        <ModalBody>
            <p>
                Проверьте ваши данные и исправьте если необходимо
            </p>
            <Container className="form-container-modal">
                <EditableUserData saveTitle='Участвовать' onSave={participate} small />
            </Container>
        </ModalBody>
    </Modal>;
};
