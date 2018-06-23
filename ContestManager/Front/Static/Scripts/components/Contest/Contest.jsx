import React from 'react';
import {Navbar} from 'react-bootstrap';
import {Route, Switch} from 'react-router-dom';
import ContestHeader from './ContestHeader';
import ContestNews from './ContestNews';
import Axios from 'axios';

class Contest extends Navbar {
    constructor(props) {
        super(props);

        this.state = {
            contestId: props.match.params.id,
        };
    }

    componentWillMount() {
        if (!this.props.contest) {
            Axios
                .get(`/contests/${this.state.contestId}/info`)
                .then(r => {
                    this.props.setContest(r.data);
                });
        }
    }

    render() {
        if (this.props.contest) {
            return [
                <ContestHeader key="ContestHeader" {...this.props}>
                    {this.props.contest.Title}
                </ContestHeader>,
                <Switch>
                    <Route key="ContestNews" exact path="" render={(props) =>
                        <ContestNews {...props} contest={this.props.contest} />}
                    />   
                    <Route key="Results" path="results" render={(props) =>
                        <ContestResults {...props} contest={this.props.contest} />}
                    />
                </Switch>
            ];
        }

        return "";
    }
}

export default Contest;