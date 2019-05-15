import React from 'react';
import { Alert, Button, ButtonGroup, Col, Form, FormGroup, Input, Label} from 'reactstrap';
import { post } from '../../Proxy';
import './index.css';

class Login extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            email: '',
            password: '',
            isProcessing: false,
            error: false,
        };
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
        }).then(() => {
                this.props.history.push(`/`);
                this.props.onLogIn();
            }
        ).catch(() => this.setState({ error: true }));
    };

    loginVK = () => {
        if (this.state.isProcessing || this.props.disabled) {
            return;
        }
        this.setState({ isProcessing: true });
        window.VK.Auth.login(this.checkLoginState);
    };

    loginPassword = () => {
        this.setState({ error: false });
        post('login/email',
            {
                email: this.state.email,
                password: this.state.password,
            })
            .then(() => {
                    this.props.history.push(`/`);
                }
            ).catch(() => this.setState({ error: true }));
    };

    handleChange = async (event) => {
        const { target } = event;
        const { name } = target;
        await this.setState({
            [name]: target.value,
        });
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
            <div className="form-container">
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
                        </Col>
                    </FormGroup>

                    <FormGroup row>
                        <Col sm={{ size: 6, offset: 3 }}>
                            <ButtonGroup>
                                <Button key="loginEmail" bsStyle="primary" onClick={this.loginPassword}>Войти</Button>
                                <Button key="loginVk" className="vk-button" onClick={this.loginVK}>Войти через VK</Button>
                            </ButtonGroup></Col>
                    </FormGroup>
                </Form>
            </div>
        ];
    }
}

export default Login;