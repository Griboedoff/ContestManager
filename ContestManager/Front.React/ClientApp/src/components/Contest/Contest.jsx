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
import { AddResult } from './AddResult';
import { AddNews, News } from './News';
import { Options } from './Options';
import { ParticipantsList } from './ParticipantsList';
import { Seating } from './Seating';

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

        const { title, options } = this.props.contest;
        const { participateSuccess, participateError } = this.state;
        return <Container>
            <Row><h1 className="mb-3">{title}</h1></Row>
            <Row>
                <Col sm={9}>
                    {this.showParticipateButton() &&
                    <Button color="success" className="mb-3" onClick={this.participate}>Принять участие</Button>}
                    {participateError && <Alert color="danger">{participateError}</Alert>}
                    {participateSuccess &&
                    <Alert color="success">Вы зарегистрированы. Не забудте <Link to="/user">обновить данные о
                        себе</Link></Alert>}
                    {this.renderTab()}
                </Col>
                <Col sm={3}>
                    <ListGroup>
                        <ListGroupItem>
                            <span className="link" onClick={this.handleTabChange(Tab.News)}>Информация</span>
                        </ListGroupItem>
                        <ListGroupItem>
                            <span className="link" onClick={this.handleTabChange(Tab.Participants)}>Участники</span>
                        </ListGroupItem>
                        {hasFlag(options, ContestOptions.ResultsOpen) && <ListGroupItem>
                            <span className="link" onClick={this.handleTabChange(Tab.Results)}>Результаты</span>
                        </ListGroupItem>}
                    </ListGroup>

                    {this.props.user && this.props.user.role === UserRole.Admin && <>
                        <h5 className="mt-3">Админка</h5>
                        <ListGroup>
                            <ListGroupItem>
                                <span className="link"
                                      onClick={this.handleTabChange(Tab.AddNews)}>Добавить новость</span>
                            </ListGroupItem>
                            {!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                                <span className="link" onClick={this.handleTabChange(Tab.Seating)}>Сгенерировать
                                    рассадку</span>
                            </ListGroupItem>}
                            {!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                                <span className="link" onClick={this.handleTabChange(Tab.AddResults)}>Добавить
                                    результаты</span>
                            </ListGroupItem>}
                            <ListGroupItem>
                                <span className="link" onClick={this.handleTabChange(Tab.Options)}>Настройки</span>
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
                return <News contestId={this.contestId} />;
            default:
                return <AddResult contestId={this.contestId} />;
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