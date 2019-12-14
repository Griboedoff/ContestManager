import React from 'react';
import { Col, Container, ListGroup, ListGroupItem, Row } from 'reactstrap';
import WithParticipants from '../../HOC/WithParticipants';

class ParticipantsList extends React.Component {
    render() {
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
