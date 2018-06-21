import React from 'react';
import {Nav, Navbar, NavItem} from 'react-bootstrap';
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
        if (this.props.user && UserRole[this.props.user.role] === UserRole.User)
            items.push(<NavItem key="Participate" onClick={this.handleShow}>
                Участвовать
            </NavItem>);
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