import React from 'react';
import { Link, Redirect, Route, Switch } from 'react-router-dom';
import { Alert, Button, Col, Container, Row } from 'reactstrap';
import { UserRole } from '../../Enums/UserRole';
import { roleToString, sexToString } from '../../UserFieldsMap';
import withUser from '../HOC/WithUser';
import './index.css';
import { default as EditableUserData } from './EditableUserData';

const USER = "/user";

class UserPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            user: this.props.user,
        };
    }

    render() {
        if (!this.props.user)
            return <Alert color="danger"> <Link to="/login">Войдите</Link> в систему.</Alert>;

        return <Container className="form-container">
            <Switch>
                <Route exact path={USER}>
                    <ReadonlyView {...this.props.user} />
                </Route>
                <Route exact path={`${USER}/edit`}>
                    <EditableUserData redirect={<Redirect to={USER} />}
                                      user={this.props.user}
                                      setUser={this.props.setUser}
                    />
                </Route>
            </Switch>
        </Container>;
    }
}

const ReadonlyView = ({ class: propsClass, school, role, coach, name, sex, city }) => {
    return <React.Fragment>
        <UserInfoRow label="ФИО" value={name} />
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
            <Button tag={Link} to={`${USER}/edit`}>Редактировать</Button>
        </Col>
    </React.Fragment>;
};

const UserInfoRow = ({ label, showValue, value }) => (
    <Row className="info-row">
        <Col sm="1">{label}</Col>
        <Col sm="3">{showValue ? showValue(value) : value}
        </Col>
    </Row>
);

export default withUser(UserPage);
