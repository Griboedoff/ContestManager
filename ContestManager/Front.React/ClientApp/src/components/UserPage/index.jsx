import React from 'react';
import { Link } from 'react-router-dom';
import { Alert, Button, Col, Container, Form, FormGroup, FormText, Input, Label, Row } from 'reactstrap';
import { UserRole } from '../../Enums/UserRole';
import { patch } from '../../Proxy';
import { roleToString, sexToString } from '../../UserFieldsMap';
import withUser from '../HOC/WithUser';
import './index.css';

class UserPage extends React.Component {
    constructor(props) {
        super(props);
        this.onEdit = this.onEdit.bind(this);
        this.save = this.save.bind(this);

        this.state = {
            user: this.props.user,
            isEditable: false,
        };
    }

    onEdit() {
        this.setState({
            isEditable: !this.state.isEditable
        });
    }

    async save() {
        this.onEdit();
        await patch('users', this.state.user);
        this.props.setUser(this.state.user);
    }

    handleChange = async (event) => {
        const { target } = event;
        const { name } = target;
        await this.setState({
            user: { ...this.state.user, [name]: target.value }
        });
    };

    render() {
        if (!this.props.user)
            return <Alert color="danger"> <Link to="/login">Войдите</Link> в систему.</Alert>;

        return <Container className="form-container">
            {this.state.isEditable ? this.EditableView() : this.ReadonlyView()}
        </Container>;
    }

    ReadonlyView() {
        const { role } = this.props.user;
        const { class: stateClass, school, role: stateRole, coach, name, sex, city } = this.state.user;
        return <React.Fragment>
            <UserInfoRow label="Имя" value={name} />
            <UserInfoRow label="Роль" value={stateRole} showValue={r => roleToString[r]} />
            {role === UserRole.Participant &&
            <UserInfoRow label="Пол" value={sex} showValue={r => sexToString[r]} />}
            {role === UserRole.Participant &&
            <UserInfoRow label="Класс" value={stateClass} />}
            {(role === UserRole.Participant || role === UserRole.Coach) &&
            <UserInfoRow label="Город" value={city} />}
            {(role === UserRole.Participant || role === UserRole.Coach) &&
            <UserInfoRow label="Школа" value={school} />}
            {role === UserRole.Participant &&
            <UserInfoRow label="Тренер" value={coach} />}
            <Col sm={{ offset: 1 }}>
                <Button onClick={this.onEdit}>Редактировать</Button>
            </Col>
        </React.Fragment>;
    }

    EditableView() {
        const { role } = this.props.user;
        const { class: stateClass, school, role: stateRole, coach, name, sex, city } = this.state.user;
        return <Form>
            <FormGroup row>
                <Label sm={1}>Имя</Label>
                <Col sm={4}>
                    <Input type="text" name="name" onChange={this.handleChange} value={name} />
                </Col>
            </FormGroup>
            <FormGroup row>
                <Label sm={1}>Роль</Label>
                <Col sm={4}>
                    <Input type="select" name="role" onChange={this.handleChange} value={stateRole}>
                        <option value={1}>Участник</option>
                        <option value={2}>Тренер</option>
                        <option value={4}>Волонтер</option>
                        <option value={8}>Проверяющий</option>
                    </Input>
                    <FormText>
                        При смене роли на Волонтер/Проверяющий<br /> вы не сможете обратно стать Участником/Тренером
                    </FormText>
                </Col>
            </FormGroup>
            {role === UserRole.Participant && <FormGroup row>
                <Label sm={1}>Пол</Label>
                <Col sm={4}>
                    <Input type="select" name="sex" onChange={this.handleChange} value={sex}>
                        <option value={0}>М</option>
                        <option value={1}>Ж</option>
                    </Input>
                </Col>
            </FormGroup>}
            {role === UserRole.Participant && <FormGroup row>
                <Label sm={1}>Класс</Label>
                <Col sm={4}>
                    <Input type="select" name="class" onChange={this.handleChange} value={stateClass}>
                        <option value={5}>5</option>
                        <option value={6}>6</option>
                        <option value={7}>7</option>
                        <option value={8}>8</option>
                        <option value={9}>9</option>
                        <option value={10}>10</option>
                        <option value={11}>11</option>
                    </Input>
                </Col>
            </FormGroup>}
            {(role === UserRole.Participant || role === UserRole.Coach) &&
            <FormGroup row>
                <Label sm={1}>Город</Label>
                <Col sm={4}>
                    <Input type="text"
                           name="city"
                           onChange={this.handleChange}
                           value={city} />
                </Col>
            </FormGroup>}
            {(role === UserRole.Participant || role === UserRole.Coach) &&
            <FormGroup row>
                <Label sm={1}>Школа</Label>
                <Col sm={4}>
                    <Input type="text"
                           name="school"
                           onChange={this.handleChange}
                           value={school} />
                </Col>
            </FormGroup>}
            {role === UserRole.Participant && <FormGroup row>
                <Label sm={1}>Тренер</Label>
                <Col sm={4}>
                    <Input type="text"
                           name="coach"
                           onChange={this.handleChange}
                           value={coach} />
                </Col>
            </FormGroup>}
            <FormGroup row>
                <Col sm={{ offset: 2 }}>
                    <Button onClick={this.save}>Сохранить</Button>
                </Col>
            </FormGroup>
        </Form>;
    }
}

class UserInfoRow extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            isEditable: false,
        };
    }

    render() {
        const { input, label, showValue, value } = this.props;
        return (
            <Row className="info-row">
                <Col sm="1">{label}</Col>
                <Col sm="3">{!this.state.isEditable
                    ? showValue
                        ? showValue(value)
                        : value
                    : input}
                </Col>
            </Row>
        );
    }
}


export default withUser(UserPage);
