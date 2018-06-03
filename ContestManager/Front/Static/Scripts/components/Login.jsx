import React from 'react';
import {FormGroup, FormControl, ControlLabel, Form, Col, ButtonGroup, Alert} from 'react-bootstrap';
import Button from "react-bootstrap/es/Button";
import Axios from 'axios';

class Login extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.onLogIn = props.onLogIn;

        this.state = {
            email: '',
            password: '',
            isSdkLoaded: false,
            isProcessing: false,
            error: false,
        };
    }

    componentDidMount() {
        if (document.getElementById('vk-jssdk')) {
            this.sdkLoaded();
            return;
        }
        this.setVkAsyncInit();
        Login.loadSdkAsynchronously();
    }

    setVkAsyncInit() {
        window.vkAsyncInit = () => {
            window.VK.init({apiId: 5186294});
            this.setState({isSdkLoaded: true});
        };
    }

    sdkLoaded() {
        this.setState({isSdkLoaded: true});
    }

    static loadSdkAsynchronously() {
        const el = document.createElement('script');
        el.type = 'text/javascript';
        el.src = 'https://vk.com/js/api/openapi.js?154';
        el.async = true;
        el.id = 'vk-jssdk';
        document.getElementsByTagName('head')[0].appendChild(el);
    }

    checkLoginState = response => {
        if (this.props.user)
            return;

        this.setState({isProcessing: false, error: false});

        const session = response.session;

        Axios.post('vk', {
            expire: session.expire,
            mid: session.mid,
            secret: session.secret,
            sid: session.sid,
            sig: session.sig
        })
            .then((resp) => {
                    this.onLogIn(resp.data);
                }
            ).catch(() => this.setState({error: true}));
    };

    loginVK = () => {
        if (!this.state.isSdkLoaded || this.state.isProcessing || this.props.disabled) {
            return;
        }
        this.setState({isProcessing: true});
        window.VK.Auth.login(this.checkLoginState);
    };

    loginPassword = () => {
        this.setState({error: false});
        Axios.post('email', {
            email: this.state.email,
            password: this.state.password,
        })
            .then((resp) => {
                    this.onLogIn(resp.data);
                }
            ).catch(() => this.setState({error: true}));
    };

    render() {
        return [
            this.state.error
                ? <Alert bsStyle="danger">
                    Ой! Что-то пошло не так
                    <br />
                    Попробуйте позже
                </Alert>
                : "",
            <Form horizontal className="login-form">
                <FormGroup controlId="formHorizontalEmail">
                    <Col componentClass={ControlLabel} sm={4}>
                        Email
                    </Col>
                    <Col sm={4}>
                        <FormControl type="email" placeholder="Email" />
                    </Col>
                </FormGroup>

                <FormGroup controlId="formHorizontalPassword">
                    <Col componentClass={ControlLabel} sm={4}>
                        Password
                    </Col>
                    <Col sm={4}>
                        <FormControl type="password" placeholder="Пароль" />
                    </Col>
                </FormGroup>

                <FormGroup controlId="login-btns">
                    <Col smOffset={4} sm={10}>
                        <ButtonGroup>
                            <Button key="loginEmail" bsStyle="primary" onClick={this.loginPassword}>Войти</Button>
                            <Button key="loginVk" onClick={this.loginVK}>Войти через VK</Button>
                        </ButtonGroup></Col>
                </FormGroup>
            </Form>,
        ];
    }
}

export default Login