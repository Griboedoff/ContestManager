import { AvFeedback, AvForm, AvGroup, AvInput } from 'availity-reactstrap-validation';
import React, { useEffect, useState } from 'react';
import { Redirect } from 'react-router-dom';
import { Button, Col, Container, FormGroup, Label } from 'reactstrap';
import { get, post } from '../../Proxy';
import { CenterSpinner } from '../CenterSpinner';
import { InviteLinkStatus } from './InviteLinkStatus';

const changePass = async (code, password) => {
    await post(`users/changePass/${code}`, password);
};

export const AcceptInvite = (props) => {
    const [status, setStatus] = useState(null);
    const [fetching, setFetching] = useState(true);
    const [password, setPassword] = useState('');
    const [saved, setSaved] = useState(false);

    const code = props.match.params.id;

    useEffect(() => {
        const checkCode = async () => {
            try {
                const resp = await get(`invite/${code}`);
                let status = InviteLinkStatus.Error;
                if (resp.ok)
                    status = await resp.json();
                setStatus(status);
            } catch {
                setStatus(InviteLinkStatus.Error);
            } finally {
                setFetching(false);
            }
        };

        checkCode();
    }, [code]);

    const renderContent = (status) => {
        switch (status) {
            case InviteLinkStatus.WrongLink:
                return 'Неверная ссылка';
            case InviteLinkStatus.AlreadyUsed:
                return 'Ссылка уже использована';
            case InviteLinkStatus.RestorePassword:
                return <AvForm>
                    <AvGroup row>
                        <Label sm={3}>Новый пароль</Label>
                        <Col sm={9}>
                            <AvInput onChange={e => setPassword(e.target.value)}
                                     name="password"
                                     type="password"
                                     required
                                     pattern=".{8}.*"
                                     id="password"
                            />
                            <AvFeedback>Пароль не должен быть короче 8 символов</AvFeedback>
                        </Col>
                    </AvGroup>
                    <FormGroup row>
                        <Col sm={{ size: 10, offset: 3 }}>
                            <Button className="align-self-right"
                                    onClick={() => changePass(code, password).then(() => setSaved(true))}>Сменить</Button>
                        </Col>
                    </FormGroup>
                </AvForm>;
            case InviteLinkStatus.Error:
            default:
                return 'Внутренняя ошибка';
        }
    };

    if (fetching)
        return <CenterSpinner />;

    if (saved || status === InviteLinkStatus.Ok)
        return <Redirect to="/" />;

    return <Container>
        {renderContent(status)}
    </Container>;
};

