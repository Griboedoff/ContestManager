import React from 'react';
import { Alert, Button, Col, FormGroup, Label, Tooltip } from 'reactstrap';
import { post } from '../../Proxy';
import { AvForm, AvGroup, AvInput, AvFeedback } from 'availity-reactstrap-validation';
import './../../common.css';

const parseRegisterStatus = (s) => {
    switch (s) {
        case "EmailAlreadyUsed" :
            return "Такой email уже зарегистрирован";
        case "RequestAlreadyUsed" :
        case "WrongConfirmationCode" :
            return "Неверный код подтверждения";
        case "VkIdAlreadyUsed" :
            return "Пользователь с таким VK уже зарегистрирован";
        case"Success" :
        case"RequestCreated" :
            return "";
    }
};

class Register extends React.Component {
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
            confirmSend: false,
            tooltipOpen: false
        };
        document.title = "Регистрация";

        this.toggle = this.toggle.bind(this);
    }

    russianLetters = "^[' а-яё-]*$";
    registerInternal = response => {
        if (this.props.user)
            return;

        this.setState({ isProcessing: false, error: false });

        const user = response.session.user;

        post('register/vk', {
            name: `${user.last_name} ${user.first_name}`,
            vkId: user.id,
        }).then((resp) => {
                this.setState({ errorMessage: parseRegisterStatus(resp.data) });
                if (this.state.errorMessage)
                    this.setState({ error: true });
                else
                    this.props.history.push(`/`);
            }
        ).catch(() => this.setState({ error: true }));
    };

    registerVK = () => {
        if (this.state.isProcessing || this.props.disabled) {
            return;
        }
        this.setState({ isProcessing: true, error: false, errorMessage: '' });
        window.VK.Auth.login(this.registerInternal);
    };

    registerEmail = () => {
        this.setState({ error: false, errorMessage: '' });

        post('register/email', {
            email: this.state.email,
        }).then((resp) => {
                this.setState({ errorMessage: parseRegisterStatus(resp.data) });
                if (this.state.errorMessage)
                    this.setState({ error: true });
                else
                    this.setState({ confirmSend: true });
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

    toggle() {
        this.setState({
            tooltipOpen: !this.state.tooltipOpen
        });
    }

    render() {
        return <div className="form-container row flex-column">
            {this.state.error && <Alert bsStyle="danger">
                {this.state.errorMessage ? this.state.errorMessage :
                    <div>Ой! Что-то пошло не так
                        <br />
                        Попробуйте позже
                    </div>
                }
            </Alert>}
            <Button className="align-self-center row vk-button" onClick={this.registerVK}>Зарегистрироваться через
                ВКонтакте</Button>
            <hr />

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
                                 type="text"
                                 required
                                 min="8"
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
            </AvForm>
        </div>;
    }
}

export default Register;
;