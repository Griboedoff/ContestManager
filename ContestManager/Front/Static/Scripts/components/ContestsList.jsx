import React from 'react';
import Axios from 'axios';
import {ListGroup, ListGroupItem} from 'react-bootstrap';

export default class extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            constets: []
        };
    }

    componentWillMount() {
        Axios
            .get("/contests")
            .then(r => this.setState({contests: r.data}));
    }

    onClick = (to, contest) => {
        this.props.history.push(to);
    };

    render() {
        if (this.state.contests) {
            const titleStyle = {
                fontSize: '1.5em',
                color: '#007bff',
            };

            return (
                <ListGroup>
                    {this.state.contests.map(c => (
                        <ListGroupItem style={titleStyle}
                                       key={c.Id}
                                       onClick={() => this.onClick(`/contests/${c.Id}`, c)}>
                            {c.Title}
                        </ListGroupItem>))}
                </ListGroup>);
        }

        return "";
    }
}
