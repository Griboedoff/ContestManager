import React from 'react';
import {
    Button,
    Modal,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Input,
    InputGroupAddon,
    InputGroup,
    Spinner,
    Alert
} from 'reactstrap';
import './index.css';
import { post } from '../../Proxy';

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

        await post('users/restore', {
            email: this.state.email
        }).then(resp => {
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
        return (
            <Modal isOpen={this.props.isOpen} toggle={this.props.toggle}>
                <ModalHeader>Восстановление пароля</ModalHeader>
                <ModalBody>
                    Пароль можно сменить на странице редактирования данных.<br />
                    Мы отправим одноразовую ссылку для входа
                    {!this.state.inputMail && this.props.email && <>
                        на почту {this.props.email}<br />
                        <span onClick={this.clearMail} className="small text-muted pseudo-link">Указать другую почту</span>
                    </>}
                    {this.state.inputMail && <>
                        <InputGroup className="mt-3">
                            <InputGroupAddon addonType="prepend">email</InputGroupAddon>
                            <Input type="email" name="email" onChange={this.handleChange} />
                        </InputGroup>
                    </>}
                </ModalBody>
                <ModalFooter>
                    {this.state.error && <Alert color="danger">{this.state.message}</Alert>}
                    {!this.state.error && this.state.message && <Alert color="success">{this.state.message}</Alert>}
                    <Button onClick={this.restore}>
                        {this.props.fetching && <Spinner color="secondary" />}
                        Восстановить</Button>
                </ModalFooter>
            </Modal>
        );
    }
}
