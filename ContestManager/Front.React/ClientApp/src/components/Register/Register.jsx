import React from 'react';
import { Alert, Button, Col, FormGroup, Label, Tooltip } from 'reactstrap';
import { post } from '../../Proxy';
import { AvForm, AvGroup, AvInput, AvFeedback } from 'availity-reactstrap-validation';
import './../../common.css';

function parseRegisterStatus(s) {
    switch (s) {
        case 1:
            return "Такой email уже зарегистрирован";
        case 3:
            return "Пользователь с таким VK уже зарегистрирован";
        default:
            return "";
    }
}

export class Register extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            name: '',
            surname: '',
            patronymic: '',
            email: '',
            password: '',

            isProcessing: false,
            error: false,
            errorMessage: '',
            tooltipOpen: false
        };

        this.toggle = this.toggle.bind(this);
    }

    russianLetters = "^[' А-ЯЁа-яё-]*$";

    registerVKInternal = response => {
        if (this.props.user)
            return;

        this.setState({ isProcessing: false, error: false });

        const user = response.session.user;

        post('users/register/vk', {
            Name: `${user.last_name} ${user.first_name}`,
            VkId: user.id,
        }).then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            throw Error();
        }).then(status => {
            this.setState({ errorMessage: parseRegisterStatus(status) });
            if (this.state.errorMessage)
                this.setState({ error: true });
            else
                this.props.history.push(`/`);
        }).catch(() => this.setState({ error: true }));
    };

    registerVK = () => {
        if (this.state.isProcessing || this.props.disabled) {
            return;
        }
        this.setState({ isProcessing: true, error: false, errorMessage: '' });
        window.VK.Auth.login(this.registerVKInternal);
    };

    registerEmail = () => {
        this.setState({ error: false, errorMessage: '' });

        post('users/register/email', {
            Email: this.state.email,
            Name: this.state.name,
            Surname: this.state.surname,
            Password: this.state.password,
            Patronymic: this.state.patronymic
        }).then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            throw Error();
        }).then(status => {
            this.setState({ errorMessage: parseRegisterStatus(status) });
            if (this.state.errorMessage)
                this.setState({ error: true });
            else
                this.setState({ confirmSend: true });
        }).catch(() => this.setState({ error: true }));
    };

    handleChange = async (event) => {
        const { target } = event;
        const { name } = target;
        await this.setState({
            [name]: target.value,
        });
    };

    toggle() {
        this.setState({
            tooltipOpen: !this.state.tooltipOpen
        });
    }

    render() {
        return <div className="form-container row flex-column">
            {this.state.error && <Alert color="danger">
                {this.state.errorMessage ? this.state.errorMessage :
                    <div>Ой! Что-то пошло не так
                        <br />
                        Попробуйте позже
                    </div>
                }
            </Alert>}
            <Button
                className="mb-5 align-self-center row vk-button"
                onClick={this.registerVK}>Зарегистрироваться через ВКонтакте</Button>

            <AvForm>
                <legend>Регистрация по email</legend>
                <AvGroup row>
                    <Label sm={3}>Фамилия</Label>
                    <Col sm={9}>
                        <AvInput onChange={this.handleChange}
                                 placeholder="Иванов"
                                 name="surname"
                                 required
                                 pattern={this.russianLetters} />
                        <AvFeedback>Только русские буквы</AvFeedback>
                    </Col>
                </AvGroup>
                <AvGroup row>
                    <Label sm={3}>Имя</Label>
                    <Col sm={9}>
                        <AvInput onChange={this.handleChange}
                                 placeholder="Иван"
                                 name="name"
                                 required
                                 pattern={this.russianLetters} />
                        <AvFeedback>Только русские буквы</AvFeedback>
                    </Col>
                </AvGroup>
                <AvGroup row>
                    <Label sm={3}>Отчество</Label>
                    <Col sm={9}>
                        <AvInput onChange={this.handleChange}
                                 placeholder="Иванович"
                                 name="patronymic"
                                 pattern={this.russianLetters} />
                        <AvFeedback>Только русские буквы</AvFeedback>
                    </Col>
                </AvGroup>
                <AvGroup row>
                    <Label sm={3}>Email</Label>
                    <Col sm={9}>
                        <AvInput onChange={this.handleChange}
                                 placeholder="example@gmail.com"
                                 name="email"
                                 type="email"
                                 required />
                    </Col>
                </AvGroup>
                <AvGroup row>
                    <Label sm={3}>Пароль</Label>
                    <Col sm={9}>
                        <AvInput onChange={this.handleChange}
                                 name="password"
                                 type="password"
                                 required
                                 pattern=".{8}.*"
                                 id="password"
                        />
                        <AvFeedback>Пароль не должен быть короче 8 символов</AvFeedback>
                    </Col>
                    <Tooltip placement="right"
                             target="password"
                             isOpen={this.state.tooltipOpen}
                             autohide={false}
                             toggle={this.toggle}>
                        Для большей безопасности используйте{' '}
                        <a href="https://hsto.org/getpro/habr/post_images/b31/1dc/991/b311dc991048ad8c8dc5a0dc33d7c179.png">парольные
                            фразы</a>{' '}
                        &mdash; их проще запомнить и сложнее сломать.
                    </Tooltip>
                </AvGroup>
                <FormGroup row>
                    <Col sm={{ size: 10, offset: 3 }}>
                        <Button className="align-self-right" onClick={this.registerEmail}> Зарегистрироваться </Button>
                    </Col>
                </FormGroup>
                {this.state.confirmSend &&
                <Alert>На адрес {this.state.email} отправлено письмо с подтверждением</Alert>}
            </AvForm>
        </div>;
    }
}
