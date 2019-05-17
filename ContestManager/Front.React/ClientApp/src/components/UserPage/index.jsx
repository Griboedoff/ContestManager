import React from 'react';
import { Alert, Col, Container, Row } from 'reactstrap';
import { Link } from 'react-router-dom';
import './index.css';
import withUser from '../HOC/WithUser';

class UserPage extends React.Component {
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
        if (!this.props.user)
            return <Alert color="danger"> <Link to="/login">Войдите</Link> в систему.</Alert>;

        return <Container className="form-container">
            <Row>
                <Col sm="2">adsfasdf</Col>
                <Col>asdfasdf</Col>
            </Row>
        </Container>;
    }
}


export default withUser(UserPage);