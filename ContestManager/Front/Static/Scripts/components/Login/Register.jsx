import React from 'react';
import {Alert, Button, Col, Form, FormControl, FormGroup, Tooltip, ControlLabel} from 'react-bootstrap';
import Axios from 'axios';
import {EmailValidator, LengthValidator, RussianLettersValidator} from "../../Common/Validators";
import FormGroupWithTooltip from './FormGroupWithTooltip'

const tt = (val) => <Tooltip id={`tooltipHelp${Math.random()}`}>{val}</Tooltip>;

const overlay = (val, text) => {
    switch (val) {
        case 'error':
            return tt(text);
        case 'warning':
            return tt("Обязательное поле");
        case 'success':
        case null:
            return <div />;
    }
};
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
            return ""
    }
};

class Register extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.state = {
            firstName: '',
            surname: '',
            patronymic: '',
            email: '',
            password: '',
            passwordRepeat: '',

            isSdkLoaded: false,
            isProcessing: false,
            error: false,
            errorMessage: '',
            confirmSend: false,
        };
    }

    componentDidMount() {
        document.title = "Регистрация";
        if (document.getElementById('vk-jssdk')) {
            this.sdkLoaded();
            return;
        }
        this.setVkAsyncInit();
        Register.loadSdkAsynchronously();

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

    registerInternal = response => {
        if (this.props.user)
            return;

        this.setState({isProcessing: false, error: false});

        const user = response.session.user;

        Axios.post('register/vk', {
            name: `${user.last_name} ${user.first_name}`,
            vkId: user.id,
        })
            .then((resp) => {
                    this.setState({errorMessage: parseRegisterStatus(resp.data)});
                    if (this.state.errorMessage)
                        this.setState({error: true});
                    else
                        this.props.history.push(`/`);
                }
            ).catch(() => this.setState({error: true}));
    };

    registerVK = () => {
        if (!this.state.isSdkLoaded || this.state.isProcessing || this.props.disabled) {
            return;
        }
        this.setState({isProcessing: true, error: false, errorMessage: ''});
        window.VK.Auth.login(this.registerInternal);
    };

    registerEmail = () => {
        this.setState({error: false, errorMessage: '', confirmSend: false});

        Axios.post('register/email', {
            email: this.state.email,
        })
            .then((resp) => {
                    this.setState({errorMessage: parseRegisterStatus(resp.data)});
                    if (this.state.errorMessage)
                        this.setState({error: true});
                    else
                        this.setState({confirmSend: true});
                }
            ).catch(() => this.setState({error: true}));
    };

    confirm = () => {
        this.setState({error: false, errorMessage: ''});

        Axios.post('register/email/confirm', {
            name: `${this.state.surname} ${this.state.firstName} ${this.state.patronymic}`,
            email: this.state.email,
            password: this.state.password,
            confirmationCode: this.state.confirmationCode,

        })
            .then((resp) => {
                    this.setState({errorMessage: parseRegisterStatus(resp.data)});
                    if (this.state.errorMessage)
                        this.setState({error: true});
                    else
                        this.props.history.push(`/`);
                }
            ).catch(() => this.setState({error: true}));
    };

    static validateName = val => val ? (RussianLettersValidator.validate(val) ? 'success' : 'error') : null;
    static validateNotEmptyName = val => val ? (RussianLettersValidator.validate(val) ? 'success' : 'error') : "warning";
    static validateEmail = val => val ? (EmailValidator.validate(val) ? 'success' : 'error') : "warning";
    static validatePass = val => {
        if (!val)
            return "warning";

        const validator = new LengthValidator(8, 20);

        return validator.validate(val) ? 'success' : 'error';
    };

    validatePassRepeat = val => {
        if (!this.state.password && !val)
            return "warning";

        return val === this.state.password ? 'success' : 'error';
    };

    handleNameChange = (e) => this.setState({firstName: e.target.value});
    handleSurnameChange = (e) => this.setState({surname: e.target.value});
    handlePatronymicChange = (e) => this.setState({patronymic: e.target.value});
    handleEmailChange = (e) => this.setState({email: e.target.value});
    handlePassChange = (e) => this.setState({password: e.target.value});
    handlePassRepeatChange = (e) => this.setState({passwordRepeat: e.target.value});
    handleConfirmCodeChange = (e) => this.setState({confirmCode: e.target.value});

    render() {
        return [
            this.state.error
                ? <Alert bsStyle="danger">
                    {this.state.errorMessage ? this.state.errorMessage :
                        <div>Ой! Что-то пошло не так
                            <br />
                            Попробуйте позже
                        </div>
                    }
                </Alert>
                : "",
            <Form horizontal className="login-form">
                <FormGroup controlId="login-btns">
                    <Col smOffset={4} sm={4}>
                        <Button key="loginVk" onClick={this.registerVK}>Зарегистрироваться через VK</Button>
                    </Col>
                </FormGroup>
                <hr />
                <FormGroupWithTooltip controlId="formHorizontalName"
                                      label="Имя"
                                      validationState={Register.validateNotEmptyName(this.state.firstName)}
                                      overlay={overlay(Register.validateNotEmptyName(this.state.firstName), "Имя должно быть из русских букв")}
                                      value={this.state.firstName}
                                      ph={"Иван"}
                                      onChange={this.handleNameChange} />
                <FormGroupWithTooltip controlId="formHorizontalSurname"
                                      label="Фамилия"
                                      validationState={Register.validateNotEmptyName(this.state.surname)}
                                      overlay={overlay(Register.validateNotEmptyName(this.state.surname), "Фамилия должнa быть из русских букв")}
                                      value={this.state.surname}
                                      ph={"Иванов"}
                                      onChange={this.handleSurnameChange} />
                <FormGroupWithTooltip controlId="formHorizontalFathersName"
                                      label="Отчество"
                                      validationState={Register.validateName(this.state.patronymic)}
                                      overlay={overlay(Register.validateName(this.state.patronymic), "Отчество должно быть из русских букв")}
                                      value={this.state.patronymic}
                                      ph={"Иванович"}
                                      onChange={this.handlePatronymicChange} />
                <FormGroupWithTooltip controlId="formHorizontalEmail"
                                      label="Email"
                                      validationState={Register.validateEmail(this.state.email)}
                                      overlay={overlay(Register.validateEmail(this.state.email), "Неверный формат")}
                                      value={this.state.email}
                                      ph={"Email"}
                                      onChange={this.handleEmailChange} />
                <FormGroupWithTooltip controlId="formHorizontalPassword"
                                      label="Пароль"
                                      validationState={Register.validatePass(this.state.password)}
                                      overlay={overlay(Register.validatePass(this.state.password), "Пароль должен быть длиннее 8 символов")}
                                      value={this.state.password}
                                      ph={""}
                                      onChange={this.handlePassChange}
                                      type="password" />
                <FormGroupWithTooltip controlId="formHorizontalRepeatPass"
                                      label="Повторите пароль"
                                      validationState={this.validatePassRepeat(this.state.passwordRepeat)}
                                      overlay={overlay(this.validatePassRepeat(this.state.passwordRepeat), "Пароли не совпадают")}
                                      value={this.state.passwordRepeat}
                                      ph={""}
                                      onChange={this.handlePassRepeatChange}
                                      type="password" />
                <FormGroup
                    controlId="register-btn">
                    <Col smOffset={4} sm={10}>
                        <Button key="loginEmail"
                                bsStyle="primary"
                                onClick={this.registerEmail}>
                            Зарегистрироваться
                        </Button>
                    </Col>
                </FormGroup>
            </Form>,
            this.state.confirmSend ? [
                <Alert> Код подтверждения отправлен на {this.state.email}</Alert>,
                <Form horizontal>
                    <FormGroup controlId="login-btn">
                        <Col componentClass={ControlLabel} sm={4}>Код подтверждения </Col>
                        <Col sm={2}>
                            <FormControl type="text"
                                         value={this.props.confirmCode}
                                         onChange={this.handleConfirmCodeChange} />
                        </Col>
                        <Col sm={2}>
                            <Button key="confirm" bsStyle="primary" onClick={this.confirm}>
                                Подтвердить
                            </Button>
                        </Col>
                    </FormGroup>
                </Form>
            ] : "",

        ];
    }
}

export default Register