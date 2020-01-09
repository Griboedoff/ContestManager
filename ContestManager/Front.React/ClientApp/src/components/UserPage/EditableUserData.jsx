import React from 'react';
import { Alert, Button, Col, Label } from 'reactstrap';
import { UserRole } from '../../Enums/UserRole';
import { patch } from '../../Proxy';
import withUser from '../HOC/WithUser';
import { AvForm, AvGroup, AvInput } from 'availity-reactstrap-validation';

class EditableUserData extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            user: props.user,
            hasErrors: false,
        };

        this.save = this.save.bind(this);
    }

    async save(event, errors) {
        if (errors.length) {
            this.setState({ hasErrors: true });
            return false;
        }
        await patch('users', this.state.user);
        this.props.setUser(this.state.user);
        if (this.props.onSave) {
            this.props.onSave();
        }
    }

    handleChange = async (event) => {
        const { target } = event;
        const { name } = target;
        let value = target.value;
        if (name === 'role' || name === 'sex' || name === 'class')
            value = parseInt(value, 10);
        if (name === 'sex')
            value--;

        await this.setState({
            user: {
                ...this.state.user, [name]: value,
            },
            hasErrors: false
        });
    };

    validateSelect = (value, ctx, input, cb) => {
        return value !== -1;
    };

    render() {
        const { class: stateClass, school, role, coach, name, sex, city } = this.state.user;
        const { small } = this.props;
        const smLabel = small ? 4 : 1;
        const smOffset = small ? 4 : 2;
        const smInput = small ? 8 : 4;
        return (<AvForm onSubmit={this.save}>
            <AvGroup row>
                <Label sm={smLabel}>Имя</Label>
                <Col sm={smInput}>
                    <AvInput type="text" name="name" onChange={this.handleChange} value={name} required />
                </Col>
            </AvGroup>
            {role === UserRole.Participant && <AvGroup row>
                <Label sm={smLabel}>Пол</Label>
                <Col sm={smInput}>
                    <AvInput type="select"
                             name="sex"
                             onChange={this.handleChange}
                             value={sex !== null ? sex + 1 : -1}
                             validate={{ async: this.validateSelect }}
                             required>
                        <option value={-1}>Не выбран</option>
                        <option value={1}>М</option>
                        <option value={2}>Ж</option>
                    </AvInput>
                </Col>
            </AvGroup>}
            {role === UserRole.Participant && <AvGroup row>
                <Label sm={smLabel}>Класс</Label>
                <Col sm={smInput}>
                    <AvInput type="select"
                             name="class"
                             onChange={this.handleChange}
                             value={stateClass !== null ? stateClass : -1}
                             validate={{ async: this.validateSelect }}
                             required>
                        <option value={-1}>Не выбран</option>
                        <option value={5}>5</option>
                        <option value={6}>6</option>
                        <option value={7}>7</option>
                        <option value={8}>8</option>
                        <option value={9}>9</option>
                        <option value={10}>10</option>
                        <option value={11}>11</option>
                    </AvInput>
                </Col>
            </AvGroup>}
            {(role === UserRole.Participant || role === UserRole.Coach) &&
            <AvGroup row>
                <Label sm={smLabel}>Город</Label>
                <Col sm={smInput}>
                    <AvInput type="text" name="city" onChange={this.handleChange} value={city} required />
                </Col>
            </AvGroup>}
            {(role === UserRole.Participant || role === UserRole.Coach) &&
            <AvGroup row>
                <Label sm={smLabel}>Школа</Label>
                <Col sm={smInput}>
                    <AvInput type="text"
                             name="school"
                             onChange={this.handleChange}
                             value={school}
                             required />
                </Col>
            </AvGroup>}
            {role === UserRole.Participant && <AvGroup row>
                <Label sm={smLabel}>Тренер</Label>
                <Col sm={smInput}>
                    <AvInput type="text"
                             name="coach"
                             onChange={this.handleChange}
                             value={coach} />
                </Col>
            </AvGroup>}
            {!small && <AvGroup row>
                <Label sm={smLabel}>Роль</Label>
                <Col sm={smInput}>
                    <AvInput type="select" name="role" onChange={this.handleChange} value={role} required>
                        <option value={1}>Участник</option>
                        <option value={2}>Тренер</option>
                    </AvInput>
                </Col>
            </AvGroup>}
            {this.props.children}
            {this.state.hasErrors && (
                <AvGroup row>
                    <Col sm={smInput + smLabel}>
                        <Alert color="danger">Исправьте ошибки в данных</Alert>
                    </Col>
                </AvGroup>
            )}
            <AvGroup row>
                <Col sm={{ offset: smOffset }}>
                    <Button>{this.props.saveTitle || 'Сохранить'}</Button>
                </Col>
            </AvGroup>
        </AvForm>);
    }
}

export default withUser(EditableUserData);
