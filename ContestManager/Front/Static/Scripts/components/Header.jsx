import React from 'react';
import {Navbar, Nav, NavItem} from 'react-bootstrap';
import {LinkContainer} from "react-router-bootstrap";
import UserRole from '../Common/UserRole';

class Header extends Navbar {
    constructor(props) {
        super(props);
    }

    logOut = () => {
        this.props.cookies.remove("CM-User");
        this.props.history.push('/');
        this.props.onLogOut();
    };

    toMain = () => {
        this.props.setContest(null);
        this.props.history.push('/');
    };

    inContest = () => this.props.location.pathname.includes("contests");

    getControls = user => {
        const nameStyle = {
            fontWeight: 'bold'
        };

        const isAdmin = UserRole[user.role] & UserRole.ContestManager;
        const controls = isAdmin
            ? (
                !this.inContest()
                    ? (
                        <LinkContainer key="createContestLink" to="/contests/create">
                            <NavItem> Создать контест </NavItem>
                        </LinkContainer>
                    )
                    : ([
                        <LinkContainer key="addNewsLink" to="/contests/addnews">
                            <NavItem> Добавить новость </NavItem>
                        </LinkContainer>,
                        <LinkContainer key="optionsLink" to="/contests/options">
                            <NavItem> Настройки контеста </NavItem>
                        </LinkContainer>
                    ])
            )
            : "";

        return (
            <Nav pullRight>
                <NavItem style={nameStyle}> {user.name} </NavItem>
                {controls}
                <NavItem onClick={this.logOut}> Выйти </NavItem>
            </Nav>
        );
    };
    getUserSection = () => {
        if (this.props.user)
            return this.getControls(this.props.user);

        return (
            <Nav pullRight>
                <LinkContainer to="/users/register">
                    <NavItem> Зарегистрироваться </NavItem>
                </LinkContainer>
                <LinkContainer to="/users/login">
                    <NavItem> Войти </NavItem>
                </LinkContainer>
            </Nav>
        );
    };

    render() {
        return (
            <Navbar staticTop inverse>
                <Navbar.Header>
                    <Navbar.Brand onClick={this.toMain}>
                        ContestManager
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>{this.getUserSection()}</Navbar.Collapse>
            </Navbar>
        );
    }
}

export default Header;