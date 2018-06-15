import React from 'react';
import {Col, Grid, PageHeader, Row} from 'react-bootstrap';
import {Route} from "react-router-dom";
import {withCookies} from 'react-cookie';

import Header from "./Header";
import ContestsList from './ContestsList';
import Login from "./Login/Login";
import Register from "./Login/Register";
import Contest from './Contest/Contest';
import ContestCreate from "./Contest/ContestCreate";

class App extends React.Component {
    constructor(props, context) {
        super(props, context);
        const {cookies} = this.props;

        this.state = {
            cookies: cookies,
            user: null,
            contest: null,
        };
    };

    setUser = () => {
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

    setContest = contest => {
        this.setState({contest: contest});
    };

    componentDidMount() {
        this.setUser();
    }

    render() {
        return <Grid fluid>
            <Row>
                <Route key="header" path="/" render={(props) =>
                    <Header {...props}
                            cookies={this.state.cookies}
                            onLogOut={this.setUser}
                            setContest={this.setContest}
                            user={this.state.user} />}
                />
            </Row>
            <Row>
                <Col smOffset={2} sm={8}>
                    <Route key="ContestList" exact path="/" render={(props) =>
                        <ContestsList {...props} setContest={this.setContest} />}
                    />
                    <Route key="login" path="/users/login" render={(props) =>
                        <Login {...props} onLogIn={this.setUser} />}
                    />
                    <Route key="register" path="/users/register" render={(props) =>
                        <Register {...props} />}
                    />
                    <Route key="CreateContest" path="/contests/create" render={(props) =>
                        <ContestCreate {...props} />}
                    />
                    <Route key="Contest" path="/contests/:id" render={(props) =>
                        <Contest {...props}
                                 setContest={this.setContest}
                                 user={this.state.user}
                                 contest={this.state.contest} />}
                    />
                </Col>
            </Row>
        </Grid>;
    };
}

export default withCookies(App);