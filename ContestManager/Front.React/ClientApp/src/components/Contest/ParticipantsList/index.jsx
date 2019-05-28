import React from 'react';
import { ListGroup, ListGroupItem, Row } from 'reactstrap';
import Col from 'reactstrap/es/Col';
import Container from 'reactstrap/es/Container';
import { CenterSpinner } from '../../CenterSpinner';
import { get } from '../../../Proxy';
import WithParticipants from '../../HOC/WithParticipants';
import moment from '../News/NewsArticle';

class ParticipantsList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            news: [],
        };
    }

    componentDidMount() {
        this.props.startFetchingParticipants();
        get(`contests/${this.props.contestId}/participants`)
            .then(resp => {
                if (resp.ok)
                    return resp.json();

                throw Error();
            }).then(participants => this.props.storeParticipants(participants))
            .catch(_ => this.props.fetchingError());
    }

    render() {
        if (this.props.fetchingParticipants)
            return <CenterSpinner />;

        return <ListGroup>
            {this.props.participants.map(p => <ListGroupItem key={p.id}>
                <Participant participant={p} />
            </ListGroupItem>)}
        </ListGroup>;
    }
}

function Participant({ participant }) {
    const data = participant.userSnapshot;
    console.log(data);

    return <>
        <div className="d-flex align-items-baseline justify-content-between mb-4">
            <h4 className="mb-0">{data.name}</h4>
            <small className="mb-2">{data.class} класс</small>
        </div>
        <Container>
            <Row>
                <Col sm={2}>Школа</Col><Col sm={2}>{data.school}</Col>
            </Row>
            <Row>
                <Col sm={2}>Тренер</Col><Col sm={2}>{data.coach}</Col>
            </Row>
        </Container>
    </>;
}

export default WithParticipants(ParticipantsList);