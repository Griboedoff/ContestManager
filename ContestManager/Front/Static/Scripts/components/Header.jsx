import React from 'react';
import {Navbar, Nav, NavItem} from 'react-bootstrap';
import {LinkContainer} from "react-router-bootstrap";
import UserRole from '../Common/UserRole';

class Header extends Navbar {
    logOut = () => {
        this.props.cookies.remove("CM-User");
        this.props.history.push('/');
        this.render();
    };

    getUser = () => {
        const str = this.props.cookies.get("CM-User");
        let user = null;
        if (str) {
            const splitted = str.split('&');
            user = {
                name: splitted[1].split('=')[1],
                role: splitted[2].split('=')[1],
            };
        }

        return user;
    };

    getUserSection = () => {
        const nameStyle = {
            fontWeight: 'bold'
        };

        const user = this.getUser();

        if (user)
            return (
                <Nav pullRight>
                    <Navbar.Text style={nameStyle}> {user.name} </Navbar.Text>
                    {UserRole[user.role] & UserRole.ContestManager
                        ?
                        (<LinkContainer to="/contests/create">
                            <NavItem> Создать контест </NavItem>
                        </LinkContainer>)
                        : ""}
                    <NavItem onClick={this.logOut}> Выйти </NavItem>
                </Nav>);

        return (
            <Nav pullRight>
                <LinkContainer to="/users/register">
                    <NavItem> Зарегистрироваться </NavItem>
                </LinkContainer>
                <LinkContainer to="/users/login">
                    <NavItem> Войти </NavItem>
                </LinkContainer>
            </Nav>);
    };

    render() {
        return (
            <Navbar staticTop inverse>
                <Navbar.Header>
                    <Navbar.Brand>
                        <a href="/">ContestManager</a>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>{this.getUserSection()}</Navbar.Collapse>
            </Navbar>
        );
    }
}

export default Header;