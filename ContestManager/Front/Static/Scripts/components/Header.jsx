import React from 'react';
import {Navbar, Nav, NavItem} from 'react-bootstrap';
import {LinkContainer} from "react-router-bootstrap";
import {Link} from 'react-router-dom';
import UserRole from '../Common/UserRole';

class Header extends Navbar {
    logOut = () => {
        this.props.cookies.remove("CM-User");
        this.props.history.push('/');
        
        this.props.onLogOut();
    };


    getUserSection = () => {
        const nameStyle = {
            fontWeight: 'bold'
        };

        // const user = this.getUser();

        if (this.props.user)
            return (
                <Nav pullRight>
                    <NavItem style={nameStyle}> {this.props.user.name} </NavItem>
                    {UserRole[this.props.user.role] & UserRole.ContestManager
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
                        <Link to="/">ContestManager</Link>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>{this.getUserSection()}</Navbar.Collapse>
            </Navbar>
        );
    }
}

export default Header;