import React from 'react';
import { ListGroup, ListGroupItem, Row } from 'reactstrap';
import Col from 'reactstrap/es/Col';
import Container from 'reactstrap/es/Container';
import { CenterSpinner } from '../../CenterSpinner';
import { get } from '../../../Proxy';
import WithParticipants from '../../HOC/WithParticipants';

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

    return <>
        <Container>
            <Row className="d-flex align-items-baseline justify-content-between mb-2">
                <h4>{data.name}</h4>
                <small>{data.class} класс</small>
            </Row>
            <Row> <Col sm={6}>{data.city}, {data.school}</Col> <Col sm={6}>Тренер: {data.coach}</Col> </Row>
        </Container>
    </>;
}

export default WithParticipants(ParticipantsList);