import React from 'react';
import {Navbar, Nav, NavItem} from 'react-bootstrap';
import {LinkContainer} from "react-router-bootstrap";

class Header extends Navbar {
    constructor(props, context) {
        super(props, context);
    }

    logOut = () => this.props.onLogOut(null);

    render() {
        return (
            <Navbar staticTop inverse>
                <Navbar.Header>
                    <Navbar.Brand>
                        <a href="/">ContestManager</a>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    {this.props.user
                        ? (
                            <Nav pullRight>
                                <Navbar.Text>
                                    {this.props.user.Name}
                                </Navbar.Text>
                                <NavItem onClick={this.logOut}>
                                    Выйти
                                </NavItem>
                            </Nav>)
                        : (
                            <Nav pullRight>
                                <LinkContainer to="/users/register">
                                    <NavItem>
                                        Зарегистрироваться
                                    </NavItem>
                                </LinkContainer>
                                <LinkContainer to="/users/login">
                                    <NavItem>
                                        Войти
                                    </NavItem>
                                </LinkContainer>
                            </Nav>)}
                </Navbar.Collapse>
            </Navbar>
        );
    }
}

export default Header