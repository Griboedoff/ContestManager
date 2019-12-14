import React from 'react';
import {
    Alert,
    Button,
    Input,
    InputGroup,
    InputGroupAddon,
    Modal,
    ModalBody,
    ModalFooter,
    ModalHeader,
    Spinner
} from 'reactstrap';
import { post } from '../../Proxy';
import './index.css';

export default class ChangePasswordModal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            email: this.props.email,
            inputMail: !this.props.email,
            fetching: false,
            message: '',
            isError: false
        };
    }

    handleChange = async (event) => {
        const { target } = event;
        const { name } = target;
        await this.setState({
            [name]: target.value,
        });
    };

    clearMail = () => this.setState({ email: '', inputMail: true });

    restore = async () => {
        this.setState({
            fetching: true,
            message: '',
            isError: false
        });

        await post('users/restore', this.state.email)
            .then(resp => {
                if (resp.ok)
                    return resp;

                throw Error();
            }).then(_ => this.setState({
                message: `Письмо со ссылкой для входа отправлено на ${this.state.email}`
            })).catch(_ => this.setState({
                message: 'Произошла ошибка. Попробуйте позже',
                error: true
            })).finally(() => this.setState({
                fetching: false
            }));
    };

    render() {
        const { error, inputMail, message } = this.state;
        const { fetching, toggle, email, isOpen } = this.props;
        return (
            <Modal isOpen={isOpen} toggle={toggle}>
                <ModalHeader>Восстановление пароля</ModalHeader>
                <ModalBody>
                    Пароль можно сменить на странице редактирования данных.<br />
                    Мы отправим одноразовую ссылку для входа {' '}
                    {!inputMail && email && <>
                        на почту {email}<br />
                        <span onClick={this.clearMail}
                              className="small text-muted pseudo-link">Указать другую почту</span>
                    </>}
                    {inputMail && <>
                        <InputGroup className="mt-3">
                            <InputGroupAddon addonType="prepend">email</InputGroupAddon>
                            <Input type="email" name="email" onChange={this.handleChange} />
                        </InputGroup>
                    </>}
                </ModalBody>
                <ModalFooter>
                    {error && <Alert color="danger">{message}</Alert>}
                    {!error && message && <Alert color="success">{message}</Alert>}
                    <Button onClick={this.restore}>
                        {fetching && <Spinner color="secondary" />}
                        Восстановить</Button>
                </ModalFooter>
            </Modal>
        );
    }
}
