import React from 'react';
import {Col, Grid, Row} from 'react-bootstrap';
import {Route, Switch} from "react-router-dom";
import {withCookies} from 'react-cookie';

import Header from "./Header";
import ContestsList from './ContestsList';
import Login from "./Login/Login";
import Register from "./Login/Register";
import ContestCreate from "./Contest/ContestCreate";

class App extends React.Component {
    constructor(props, context) {
        super(props, context);
        const {cookies} = this.props;

        this.state = {
            cookies: cookies,
            user: null
        };
    };

    getUser = () => {
        const str = this.state.cookies.get("CM-User");
        let user = null;
        if (str) {
            const splitted = str.split('&');
            user = {
                name: splitted[1].split('=')[1],
                role: splitted[2].split('=')[1],
            };
        }

        this.setState({user: user});
    };

    onLogInOut = () => this.getUser();

    componentDidMount() {
        this.getUser();
    }

    render() {
        return <Grid fluid>
            <Row>
                <Route key="header" path="/" render={(props) =>
                    <Header {...props} cookies={this.state.cookies}
                            onLogOut={this.onLogInOut}
                            user={this.state.user} />}
                />
            </Row>
            <Row>
                <Col smOffset={2} sm={8}>
                    <Route key="ContestList" exact path="/" render={(props) =>
                        <ContestsList {...props} />}
                    />
                    <Route key="login" path="/users/login" render={(props) =>
                        <Login {...props} onLogIn={this.onLogInOut} />}
                    />
                    <Route key="register" path="/users/register" render={(props) =>
                        <Register {...props} />}
                    />
                    <Route key="CreateContest" path="/contests/create" render={(props) =>
                        <ContestCreate {...props} />}
                    />
                </Col>
            </Row>
        </Grid>;
    };
}

export default withCookies(App);