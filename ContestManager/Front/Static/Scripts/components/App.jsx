import React from 'react';
import {Route} from "react-router-dom";
import {withCookies} from 'react-cookie';

import Header from "./Header";
import Login from "./Login/Login";
import Register from "./Login/Register";
import ContestCreate from "./Contest/ContestCreate";

class App extends React.Component {
    constructor(props, context) {
        super(props, context);
        const {cookies} = this.props;

        this.state = {cookies: cookies};
    };

    onLogIn = () => this.render();

    render() {
        return [
            <Route key="header" path="/" render={(props) =>
                <Header {...props} cookies={this.state.cookies} />}
            />,
            <Route key="login" path="/users/login" render={(props) =>
                <Login {...props} onLogIn={this.onLogIn} />}
            />,
            <Route key="register" path="/users/register" render={(props) =>
                <Register {...props} />}
            />,
            <Route key="CreateContest" path="/contests/create" render={(props) =>
                <ContestCreate {...props} />}
            />
        ];
    };
}

export default withCookies(App);