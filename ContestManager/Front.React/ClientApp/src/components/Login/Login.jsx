import React from 'react';
import { Alert, Button, Col, Form, FormGroup, FormText, Input, Label } from 'reactstrap';
import { post } from '../../Proxy';
import './index.css';
import withUser from '../HOC/WithUser';
import ChangePasswordModal from './ChangePasswordModal';

class Login extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            email: '',
            password: '',
            isProcessing: false,
            error: false,
            isOpen: false,
            errorText: ''
        };

        this.toggle = this.toggle.bind(this);
    }

    checkLoginState = response => {
        if (this.props.user)
            return;

        this.setState({ isProcessing: false, error: false });

        const session = response.session;

        post('users/login/vk', {
            Expire: session.expire,
            Mid: session.mid,
            Secret: session.secret,
            Sid: session.sid,
            Sig: session.sig
        }).then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            throw Error();
        }).then(u => {
            this.props.setUser(u);
            this.props.history.push(`/`);
        }).catch(() => this.setState({ error: true }));
    };

    loginVK = () => {
        if (this.state.isProcessing || this.props.disabled) {
            return;
        }
        this.setState({ isProcessing: true });
        window.VK.Auth.login(this.checkLoginState);
    };

    loginPassword = async () => {
        this.setState({ error: false, errorText: '' });
        const resp = await post('users/login/email', {
            email: this.state.email,
            password: this.state.password,
        });

        if (resp.ok) {
            const user = resp.json();
            this.props.setUser(user);
            document.location = "/";
            return
        }

        if (resp.status === 401)
            this.setState({ errorText: 'Неверный логин или пароль' });

        this.setState({ error: true });
    };

    handleChange = async (event) => {
        const { target } = event;
        const { name } = target;
        await this.setState({
            [name]: target.value,
        });
    };

    toggle() {
        this.setState(state => ({
            isOpen: !state.isOpen
        }));
    }

    restorePassword = () => {
        this.toggle();
    };

    render() {
        return <div className="form-container row flex-column">
            {
                this.state.error &&
                <Alert color="danger">
                    {this.state.errorText ? this.state.errorText : <>
                        Ой! Что-то пошло не так
                        <br />
                        Попробуйте позже
                    </>}
                </Alert>
            }
            <Button
                className="mb-5 align-self-center vk-button"
                onClick={this.loginVK}>Войти через ВКонтакте</Button>

            <Form>
                <FormGroup row>
                    <Label sm={3}>Email</Label>
                    <Col sm={9}>
                        <Input type="email" placeholder="Email" name="email" onChange={this.handleChange} />
                    </Col>
                </FormGroup>
                <FormGroup row>
                    <Label sm={3}>Пароль</Label>
                    <Col sm={9}>
                        <Input type="password" placeholder="Пароль" name="password" onChange={this.handleChange} />
                        <FormText onClick={this.restorePassword} className="pseudo-link">Я забыл пароль</FormText>
                    </Col>
                </FormGroup>

                <FormGroup row>
                    <Col sm={{ size: 6, offset: 3 }}>
                        <Button key="loginEmail" onClick={this.loginPassword}>Войти</Button>
                    </Col>
                </FormGroup>
            </Form>
            {this.state.isOpen &&
            <ChangePasswordModal email={this.state.email} isOpen={this.state.isOpen} toggle={this.toggle} />}
        </div>;
    }
}

export default withUser(Login);
