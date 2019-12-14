import React from 'react';
import { Link } from 'react-router-dom';
import {
    Collapse,
    Container,
    DropdownItem,
    DropdownMenu,
    DropdownToggle,
    Navbar,
    NavbarBrand,
    NavbarToggler,
    NavItem,
    NavLink,
    UncontrolledDropdown
} from 'reactstrap';
import { UserRole } from '../../Enums/UserRole';
import withUser from '../HOC/WithUser';
import './index.css';

const NavMenu = ({ user, logout }) => {
    let [isOpen, setIsOpen] = React.useState(false);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">Вузак</NavbarBrand>
                    <NavbarToggler onClick={() => setIsOpen(!isOpen)} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={isOpen} navbar>
                        <ul className="navbar-nav flex-grow">
                            {user ? <ActionsBlock name={user.name} role={user.role} logout={logout} /> : <LoginBlock />}
                        </ul>
                    </Collapse>
                </Container>
            </Navbar>
        </header>
    );
};

const LoginBlock = () => <React.Fragment>
    <NavItem>
        <NavLink tag={Link} className="text-dark" to="/register">Зарегистрироваться</NavLink>
    </NavItem>
    <NavItem>
        <NavLink tag={Link} className="text-dark" to="/login">Войти</NavLink>
    </NavItem>
</React.Fragment>;

const ActionsBlock = ({ name, role, logout }) => <React.Fragment>
    <UncontrolledDropdown nav inNavbar>
        <DropdownToggle nav caret>
            {name}
        </DropdownToggle>
        <DropdownMenu right>
            <DropdownItem> <Link className="text-dark" to="/user">Мои данные</Link> </DropdownItem>
            {role === UserRole.Admin && <DropdownItem>
                <Link className="text-dark" to="/createContest">Новое соревнование</Link>
            </DropdownItem>}
            <DropdownItem divider />
            <DropdownItem>
                <Link className="text-dark" to="/" onClick={logout}>Выйти</Link>
            </DropdownItem>
        </DropdownMenu>
    </UncontrolledDropdown>
</React.Fragment>;

export default withUser(NavMenu);
