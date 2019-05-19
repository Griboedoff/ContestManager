import React from 'react';
import { Alert, Col, Container, Row } from 'reactstrap';
import { Link } from 'react-router-dom';
import Input from 'reactstrap/es/Input';
import { roleToString, sexToString } from '../../UserFieldsMap';
import withUser from '../HOC/WithUser';
import './index.css';

class UserPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            ...this.props.user
        };
    }

    handleChange = async (event) => {
        const { target } = event;
        const { name } = target;
        await this.setState({
            [name]: target.value
        });
        this.props.updateUser(this.state);
    };

    render() {
        if (!this.props.user)
            return <Alert color="danger"> <Link to="/login">Войдите</Link> в систему.</Alert>;

        return <Container className="form-container">
            <UserInfoRow
                label="Имя"
                value={this.state.name}
                isEditable
                input={<Input type="text" placeholder="" name="name" onChange={this.handleChange} />}
            />
            <UserInfoRow
                label="Роль"
                value={this.state.role}
                showValue={r => roleToString[r]}
            />
            <UserInfoRow
                label="Пол"
                value={this.state.sex}
                showValue={r => sexToString[r]}
                isEditable
                input={<Input type="select" name="sex" onChange={this.handleChange} value={this.state.sex}>
                    <option value={0}>М</option>
                    <option value={1}>Ж</option>
                </Input>}
            />
            <UserInfoRow
                label="Класс"
                value={this.state.class}
                isEditable
                input={<Input type="select" name="class" onChange={this.handleChange} value={this.state.class}>
                    <option value={5}>5</option>
                    <option value={6}>6</option>
                    <option value={7}>7</option>
                    <option value={8}>8</option>
                    <option value={9}>9</option>
                    <option value={10}>10</option>
                    <option value={11}>11</option>
                </Input>}
            />
            <UserInfoRow
                label="Школа"
                value={this.state.school}
                isEditable
                input={<Input type="text" placeholder="" name="school" onChange={this.handleChange} />}
            />
        </Container>;
    }
}

class UserInfoRow extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.onEdit = this.onEdit.bind(this);
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
                {this.props.isEditable && this.state.isEditable
                    ? <i className="fas fa-save" onClick={this.onEdit} />
                    : <i className="fas fa-pen" onClick={this.onEdit} />}
            </Row>
        );
    }
}


export default withUser(UserPage);