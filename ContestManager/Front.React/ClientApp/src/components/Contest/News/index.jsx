import React from 'react';
import { Alert, ListGroup, ListGroupItem } from 'reactstrap';
import WithNews from '../../HOC/WithNews';
import { get } from '../../../Proxy';
import NewsArticle from './NewsArticle';

class News extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            news: [],
        };
    }

    componentDidMount() {
        this.props.fetchingNews();
        get(`contests/${this.props.contestId}/news`)
            .then(resp => {
                if (resp.ok)
                    return resp.json();

                throw Error();
            }).then(news => this.props.storeNews(news, this.props.contestId))
            .catch(_ => this.props.fetchingError());
    }

    render() {
        if (!this.props.news || !this.props.news[this.props.contestId])
            return <Alert color="info">Скоро здесь появятся новости</Alert>;

        return <ListGroup>
            {this.props.news[this.props.contestId].map(n => <ListGroupItem key={n.id}>
                <NewsArticle article={n} />
            </ListGroupItem>)}
        </ListGroup>;
    }
}

export default WithNews(News);