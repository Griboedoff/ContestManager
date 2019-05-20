import React from 'react';
import {
    Collapse,
    Container, DropdownItem, DropdownMenu, DropdownToggle,
    Navbar,
    NavbarBrand,
    NavbarToggler,
    NavItem,
    NavLink,
    UncontrolledDropdown
} from 'reactstrap';
import { Link } from 'react-router-dom';
import './index.css';
import withUser from '../HOC/WithUser';

class NavMenu extends React.Component {
    constructor(props) {
        super(props);

        this.toggle = this.toggle.bind(this);
        this.state = {
            isOpen: false
        };
    }

    toggle() {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }

    render() {
        const LoginBlock = <React.Fragment>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/register">Зарегистрироваться</NavLink>
            </NavItem>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/login">Войти</NavLink>
            </NavItem>
        </React.Fragment>;


        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/">Вузак</NavbarBrand>
                        <NavbarToggler onClick={this.toggle} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>
                            <ul className="navbar-nav flex-grow">
                                {this.props.user ? this.ActionsBlock() : LoginBlock}
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }

    ActionsBlock() {
        return <React.Fragment>
            <UncontrolledDropdown nav inNavbar>
                <DropdownToggle nav caret>
                    {this.props.user.name}
                </DropdownToggle>
                <DropdownMenu right>
                    <DropdownItem>
                        <Link className="text-dark" to="/user">Данные</Link>
                    </DropdownItem>
                    <DropdownItem>
                        Option 2
                    </DropdownItem>
                    <DropdownItem divider />
                    <DropdownItem>
                        <Link className="text-dark" to="/" onClick={this.props.logout}>Выйти</Link>
                    </DropdownItem>
                </DropdownMenu>
            </UncontrolledDropdown>
        </React.Fragment>;
    }
}

export default withUser(NavMenu);