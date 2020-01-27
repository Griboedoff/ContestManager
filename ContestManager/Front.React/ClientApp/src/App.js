import React from 'react';
import { Route } from 'react-router';
import { Switch } from 'react-router-dom';
import { AcceptInvite } from './components/AcceptInvite';
import Layout from './components/Layout';
import { ContestList } from './components/ContestList';
import { Login } from './components/Login';
import { UserPage } from './components/UserPage';
import { CreateContest } from './components/CreateContest';
import { Contest } from './components/Contest';
import { Register } from './components/Register';

export default () => (
    <Layout>
        <Switch>
            <Route exact path='/' component={ContestList} />
            <Route path='/login' component={Login} />
            <Route path='/register' component={Register} />
            <Route path='/user' component={UserPage} />
            <Route path='/createContest' component={CreateContest} />
            <Route path='/contests/:id' component={Contest} />
        </Switch>
    </Layout>
);
