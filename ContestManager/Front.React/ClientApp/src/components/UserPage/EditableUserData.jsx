import React from 'react';
import { Button, Col, Form, FormGroup, Input, Label } from 'reactstrap';
import { UserRole } from '../../Enums/UserRole';
import { patch } from '../../Proxy';
import withUser from '../HOC/WithUser';

class EditableUserData extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            user: props.user,
        };
        this.save = this.save.bind(this);
    }

    async save() {
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
        if (name === 'role' || name === 'sex')
            value = parseInt(value, 10);

        await this.setState({
            user: { ...this.state.user, [name]: value }
        });
    };

    render() {
        const { class: stateClass, school, role, coach, name, sex, city } = this.state.user;
        const { small } = this.props;
        const smLabel = small ? 4 : 1;
        const smOffset = small ? 4 : 2;
        const smInput = small ? 8 : 4;
        return (<Form>
            <FormGroup row>
                <Label sm={smLabel}>Имя</Label>
                <Col sm={smInput}>
                    <Input type="text" name="name" onChange={this.handleChange} value={name} />
                </Col>
            </FormGroup>
            {role === UserRole.Participant && <FormGroup row>
                <Label sm={smLabel}>Пол</Label>
                <Col sm={smInput}>
                    <Input type="select" name="sex" onChange={this.handleChange} value={sex}>
                        <option value={0}>М</option>
                        <option value={1}>Ж</option>
                    </Input>
                </Col>
            </FormGroup>}
            {role === UserRole.Participant && <FormGroup row>
                <Label sm={smLabel}>Класс</Label>
                <Col sm={smInput}>
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
                <Label sm={smLabel}>Город</Label>
                <Col sm={smInput}>
                    <Input type="text" name="city" onChange={this.handleChange} value={city} />
                </Col>
            </FormGroup>}
            {(role === UserRole.Participant || role === UserRole.Coach) &&
            <FormGroup row>
                <Label sm={smLabel}>Школа</Label>
                <Col sm={smInput}>
                    <Input type="text"
                           name="school"
                           onChange={this.handleChange}
                           value={school} />
                </Col>
            </FormGroup>}
            {role === UserRole.Participant && <FormGroup row>
                <Label sm={smLabel}>Тренер</Label>
                <Col sm={smInput}>
                    <Input type="text"
                           name="coach"
                           onChange={this.handleChange}
                           value={coach} />
                </Col>
            </FormGroup>}
            {!small && <FormGroup row>
                <Label sm={smLabel}>Роль</Label>
                <Col sm={smInput}>
                    <Input type="select" name="role" onChange={this.handleChange} value={role}>
                        <option value={1}>Участник</option>
                        <option value={2}>Тренер</option>
                    </Input>
                </Col>
            </FormGroup>}
            {this.props.children}
            <FormGroup row>
                <Col sm={{ offset: smOffset }}>
                    <Button onClick={this.save}>{this.props.saveTitle || 'Сохранить'}</Button>
                </Col>
            </FormGroup>
        </Form>);
    }
}

export default withUser(EditableUserData);
