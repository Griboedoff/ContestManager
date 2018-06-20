import React from 'react';
import {Button, Nav, Navbar, NavItem} from 'react-bootstrap';
import UserRole from '../../Common/UserRole';

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
        if (UserRole[this.props.user.role] & UserRole.User)
            items.push(<NavItem onClick={this.handleShow}>
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
            <Navbar key="ContestHeader">
                <Navbar.Header>
                    <Navbar.Brand>
                        {this.props.contest.Title}
                    </Navbar.Brand>
                </Navbar.Header>
                {this.buildNav()}
            </Navbar>
            ,

            <Modal.Dialog>
                <Modal.Header>
                    <Modal.Title>Modal title</Modal.Title>
                </Modal.Header>

                <Modal.Body>One fine body...</Modal.Body>

                <Modal.Footer>
                    <Button bsStyle="primary">Save changes</Button>
                </Modal.Footer>
            </Modal.Dialog>
        ];
    }
}

export default ContestHeader;