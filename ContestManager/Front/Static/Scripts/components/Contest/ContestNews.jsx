import React from 'react';
import {ListGroup, Well} from 'react-bootstrap';
import Axios from 'axios';

function News({children}) {
    return (
        <li className="list-group-item">
            <ReactMarkdown source={children} />
        </li>
    );
}

class ContestNews extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            news: [],
        };
    }

    componentWillMount() {
        Axios
            .get(`/contests/${this.props.contest.Id}/news`)
            .then(r => this.setState({news: r.data}));
    }

    render() {
        return this.state.news.length !== 0 ? (
            <ListGroup>
                {this.state.news.map(n => <News >n</News>)}
            </ListGroup>
        ) : <Well>Скоро здесь появятся новости</Well>;
    }
}

export default ContestNews;