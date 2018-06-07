import React from 'react';
import {Route} from "react-router-dom";
import {withCookies} from 'react-cookie';

import Header from "./Header";
import Login from "./Login";
import Register from "./Register";

class App extends React.Component {
    constructor(props, context) {
        super(props, context);
        const { cookies } = this.props;
        
        this.state = {cookies: cookies};
    };

    render() {
        return [
            <Route key="header" path="/" render={(props) =>
                <Header {...props} cookies={this.state.cookies} user={this.state.user} />}
            />,
            <Route key="login" path="/users/login" render={(props) =>
                <Login {...props} user={this.state.user} />}
            />,
            <Route key="register" path="/users/register" render={(props) =>
                <Register {...props} />}
            />
        ]
    };
}

export default withCookies(App);