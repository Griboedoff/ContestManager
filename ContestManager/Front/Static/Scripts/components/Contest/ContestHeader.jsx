import React from 'react';
import {MenuItem, Nav, Navbar, NavDropdown, NavItem} from 'react-bootstrap';
import {LinkContainer} from 'react-router-bootstrap/lib/ReactRouterBootstrap';
import UserRole from '../../Common/UserRole';
import ParticipateModal from './ParticipateModal';

class ContestHeader extends Navbar {
    constructor(props) {
        super(props);

        this.state = {
            show: false
        };
    }

    handleShow = () => {
        this.setState({show: true});
    };

    handleHide = () => {
        this.setState({show: false});
    };

    buildNav = () => {
        const items = [];
        if (this.props.user
            && UserRole[this.props.user.role] === UserRole.User
            && this.props.contest.State === "RegistrationOpen")
            items.push(
                <NavItem key="Participate" onClick={this.handleShow}>
                    Участвовать
                </NavItem>);
        if (this.props.contest.State === "")
            items.push(
                <NavItem key="Participate" onClick={this.handleShow}>
                    Участвовать
                </NavItem>);
        
        if (this.props.user
            && UserRole[this.props.user.role] & UserRole.Admin)
            items.push(
                <NavDropdown key="Orgdd" title="Организатор" id="Orgdd">
                    <LinkContainer key="addNewsLink" to={`${this.props.location.pathname}/addnews`}>
                        <MenuItem> Добавить новость </MenuItem>
                    </LinkContainer>,
                    <LinkContainer key="optionsLink" to={`${this.props.location.pathname}/options`}>
                        <MenuItem> Настройки контеста </MenuItem>
                    </LinkContainer>
                </NavDropdown>
            );
        return (
            <Nav>
                {items}
            </Nav>
        );
    };

    render() {
        return [
            <h2 key="CTitle">{this.props.contest.Title}</h2>,
            <Navbar key="ContestHeader">
                {this.buildNav()}
            </Navbar>,
            <ParticipateModal key="ParticipateModal" {...this.props}
                              show={this.state.show}
                              handleHide={this.handleHide} />
        ];
    }
}

export default ContestHeader;