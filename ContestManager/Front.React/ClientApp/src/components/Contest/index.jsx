import React from 'react';
import { Link } from 'react-router-dom';
import { Alert, Button, Col, Container, ListGroup, ListGroupItem, Row } from 'reactstrap';
import { ContestOptions } from '../../Enums/ContestOptions';
import { UserRole } from '../../Enums/UserRole';
import { get, post } from '../../Proxy';
import { hasFlag } from '../../utils';
import { CenterSpinner } from '../CenterSpinner';
import WithContest from '../HOC/WithContest';
import WithParticipants from '../HOC/WithParticipants';
import WithUser from '../HOC/WithUser';
import AddResult from './AddResults';
import News from './News';
import AddNews from './News/AddNews';
import Options from './Options';
import ParticipantsList from './ParticipantsList';
import Seating from './Seating';

class Contest extends React.Component {
    constructor(props) {
        super(props);
        this.contestId = this.props.match.params.id;

        this.toggleDropdown = this.toggleDropdown.bind(this);
        this.participate = this.participate.bind(this);
        this.state = {
            activeTab: 1,
            dropdownOpen: false,
            participateError: '',
            participateSuccess: false,
        };
    }

    toggleDropdown() {
        this.setState({
            dropdownOpen: !this.state.dropdownOpen
        });
    }

    async componentDidMount() {
        await this.fetchContest();
        await this.fetchParticipants();
    }

    handleTabChange = n => _ => {
        this.setState({ activeTab: n });
    };

    fetching() {
        return this.props.fetchingContests || this.props.fetchingUser || this.props.fetchingParticipants;
    }

    render() {
        if (!this.props.contest || this.fetching())
            return <CenterSpinner />;

        return <Container>
            <Row><h1 className="mb-3">{this.props.contest.title}</h1></Row>
            <Row>
                <Col sm={9}>
                    {this.showParticipateButton() &&
                    <Button className="mb-3" onClick={this.participate}>Принять участие</Button>}
                    {this.state.participateError && <Alert color="danger">{this.state.participateError}</Alert>}
                    {this.state.participateSuccess &&
                    <Alert color="success">Вы зарегистрированы. Не забудте <Link to="/user">обновить данные о
                        себе</Link></Alert>}
                    {this.renderTab()}
                </Col>
                <Col sm={3}>
                    <ListGroup>
                        <ListGroupItem>
                            <a className="link" onClick={this.handleTabChange(Tab.News)}>Информация</a>
                        </ListGroupItem>
                        <ListGroupItem>
                            <a className="link" onClick={this.handleTabChange(Tab.Participants)}>Участники</a>
                        </ListGroupItem>
                        {hasFlag(this.props.contest.options, ContestOptions.ResultsOpen) && <ListGroupItem>
                            <a className="link" onClick={this.handleTabChange(Tab.Results)}>Результаты</a>
                        </ListGroupItem>}
                    </ListGroup>

                    {this.props.user && this.props.user.role === UserRole.Admin && <>
                        <h5 className="mt-3">Админка</h5>
                        <ListGroup>
                            <ListGroupItem>
                                <a className="link" onClick={this.handleTabChange(Tab.AddNews)}>Добавить новость</a>
                            </ListGroupItem>
                            {!hasFlag(this.props.contest.options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                                <a className="link" onClick={this.handleTabChange(Tab.Seating)}>Сгенерировать
                                    рассадку</a>
                            </ListGroupItem>}
                            {!hasFlag(this.props.contest.options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                                <a className="link" onClick={this.handleTabChange(Tab.AddResults)}>Добавить результаты</a>
                            </ListGroupItem>}
                            <ListGroupItem>
                                <a className="link" onClick={this.handleTabChange(Tab.Options)}>Настройки</a>
                            </ListGroupItem>
                        </ListGroup>
                    </>}
                </Col>
            </Row>
        </Container>;
    }

    showParticipateButton() {
        return hasFlag(this.props.contest.options, ContestOptions.RegistrationOpen) &&
            this.props.user &&
            this.props.user.role === UserRole.Participant &&
            !this.props.participants.some(p => p.userId === this.props.user.id);
    }

    renderTab() {
        switch (this.state.activeTab) {
            case Tab.AddNews:
                return <AddNews contestId={this.contestId} />;
            case Tab.Options:
                return <Options />;
            case Tab.Participants:
                return <ParticipantsList participants={this.props.participants} />;
            case Tab.Seating:
                return <Seating contestId={this.contestId} />;
            case Tab.News:
            default:
                return <AddResult contestId={this.contestId} />;
                return <News contestId={this.contestId} />;
        }
    }

    participate() {
        this.setState({ participateError: null, participateSuccess: false });
        post(`contests/${this.contestId}/participate`)
            .then(async resp => {
                if (resp.ok)
                    this.setState({ participateSuccess: true });

                throw await resp.json();
            }).catch(err => this.setState({ participateError: err }));
    }

    async fetchParticipants() {
        this.props.startFetchingParticipants();
        await get(`contests/${this.contestId}/participants`)
            .then(resp => {
                if (resp.ok)
                    return resp.json();

                throw Error();
            }).then(participants => this.props.storeParticipants(participants))
            .catch(_ => this.props.fetchingError());
    }

    async fetchContest() {
        if (this.props.contest && this.props.contest.id === this.contestId) {
            return;
        }

        this.props.getContest(this.contestId);
    }
}

const Tab = {
    News: 1,
    Participants: 2,
    Results: 3,

    AddNews: 4,
    Options: 5,
    Seating: 6,
    AddResults: 7,
};

export default WithParticipants(WithUser(WithContest(Contest)));
