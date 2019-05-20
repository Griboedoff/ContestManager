import React from 'react';
import { Alert, Button, Col, Container, Form, FormGroup, FormText, Input, Label, Row } from 'reactstrap';
import { Link } from 'react-router-dom';
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
        return <React.Fragment>
            <UserInfoRow label="Имя" value={this.state.user.name} />
            <UserInfoRow label="Роль" value={this.state.user.role} showValue={r => roleToString[r]} />
            {this.props.user.role === 1 &&
            <UserInfoRow label="Пол" value={this.state.user.sex} showValue={r => sexToString[r]} />}
            {this.props.user.role === 1 && <UserInfoRow label="Класс" value={this.state.user.class} />}
            {(this.props.user.role === 1 || this.props.user.role === 2) &&
            <UserInfoRow label="Школа" value={this.state.user.school} />}
            <Col sm={{ offset: 1 }}>
                <Button onClick={this.onEdit}>Редактировать</Button>
            </Col>
        </React.Fragment>;
    }

    EditableView() {
        return <Form>
            <FormGroup row>
                <Label sm={1}>Имя</Label>
                <Col sm={4}>
                    <Input type="text" name="name" onChange={this.handleChange} value={this.state.user.name} />
                </Col>
            </FormGroup>
            <FormGroup row>
                <Label sm={1}>Роль</Label>
                <Col sm={4}>
                    <Input type="select" name="role" onChange={this.handleChange} value={this.state.user.role}>
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
            {this.props.user.role === 1 && <FormGroup row>
                <Label sm={1}>Пол</Label>
                <Col sm={4}>
                    <Input type="select" name="sex" onChange={this.handleChange} value={this.state.user.sex}>
                        <option value={0}>М</option>
                        <option value={1}>Ж</option>
                    </Input>
                </Col>
            </FormGroup>}
            {this.props.user.role === 1 && <FormGroup row>
                <Label sm={1}>Класс</Label>
                <Col sm={4}>
                    <Input type="select" name="class" onChange={this.handleChange} value={this.state.user.class}>
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
            {(this.props.user.role === 1 || this.props.user.role === 2) && <FormGroup row>
                <Label sm={1}>Школа</Label>
                <Col sm={4}>
                    <Input type="text"
                           placeholder=""
                           name="school"
                           onChange={this.handleChange}
                           value={this.state.user.school} />
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

    onEdit() {
        this.setState({
            isEditable: !this.state.isEditable
        });
    }

    render() {
        return (
            <Row className="info-row">
                <Col sm="1">{this.props.label}</Col>
                <Col sm="3">{!this.state.isEditable
                    ? this.props.showValue
                        ? this.props.showValue(this.props.value)
                        : this.props.value
                    : this.props.input}
                </Col>
            </Row>
        );
    }
}


export default withUser(UserPage);