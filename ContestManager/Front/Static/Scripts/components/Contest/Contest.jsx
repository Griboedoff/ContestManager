import React from 'react';
import {Navbar} from 'react-bootstrap';
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
                <h1 key="ContestHeader">{this.props.contest.Title}</h1>,
                <ContestNews key="ContestNews" {...this.props} contest={this.props.contest} />
            ];
        }

        return "";
    }
}

export default Contest;