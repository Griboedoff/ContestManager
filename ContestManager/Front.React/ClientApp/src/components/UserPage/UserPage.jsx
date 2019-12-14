import React from 'react';
import { Link } from 'react-router-dom';
import { Alert, Button, Col, Container, Row } from 'reactstrap';
import { UserRole } from '../../Enums/UserRole';
import { roleToString, sexToString } from '../../UserFieldsMap';
import withUser from '../HOC/WithUser';
import './index.css';
import { default as EditableUserData } from './EditableUserData';

class UserPage extends React.Component {
    constructor(props) {
        super(props);
        this.onEdit = this.onEdit.bind(this);

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

    render() {
        if (!this.props.user)
            return <Alert color="danger"> <Link to="/login">Войдите</Link> в систему.</Alert>;

        return <Container className="form-container">
            {this.state.isEditable
                ? <EditableUserData onSave={this.onEdit} />
                : this.ReadonlyView()}
        </Container>;
    }

    ReadonlyView() {
        const { class: propsClass, school, role, coach, name, sex, city } = this.props.user;
        return <React.Fragment>
            <UserInfoRow label="Имя" value={name} />
            <UserInfoRow label="Роль" value={role} showValue={r => roleToString[r]} />
            {role === UserRole.Participant &&
            <UserInfoRow label="Пол" value={sex} showValue={r => sexToString[r]} />}
            {role === UserRole.Participant &&
            <UserInfoRow label="Класс" value={propsClass} />}
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
}

const UserInfoRow = ({ label, showValue, value }) => (
    <Row className="info-row">
        <Col sm="1">{label}</Col>
        <Col sm="3">{showValue ? showValue(value) : value}
        </Col>
    </Row>
);

export default withUser(UserPage);
