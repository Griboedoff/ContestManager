import React from 'react';
import { Link, Route, Switch } from 'react-router-dom';
import { Button, Col, Container, ListGroup, ListGroupItem, Row } from 'reactstrap';
import { ContestOptions } from '../../Enums/ContestOptions';
import { UserRole } from '../../Enums/UserRole';
import { get } from '../../Proxy';
import { hasFlag } from '../../utils';
import { CenterSpinner } from '../CenterSpinner';
import WithContest from '../HOC/WithContest';
import WithParticipants from '../HOC/WithParticipants';
import WithUser from '../HOC/WithUser';
import { AddResult } from './AddResult';
import { EditNews, News } from './News';
import { Options } from './Options';
import { ParticipantsList } from './ParticipantsList';
import { ParticipateModal } from './ParticipateModal/ParticipateModal';
import { Seating } from './Seating';


class Contest extends React.Component {

    constructor(props) {
        super(props);
        this.contestId = this.props.match.params.id;
        this.CONTEST = `/contests/${this.contestId}`;

        this.toggleDropdown = this.toggleDropdown.bind(this);
        this.toggleParticipate = this.toggleParticipate.bind(this);
        this.state = {
            activeTab: 1,
            dropdownOpen: false,
            participateOpen: false
        };
    }

    toggleDropdown() {
        this.setState({
            dropdownOpen: !this.state.dropdownOpen
        });
    }

    async toggleParticipate() {
        if (this.state.participateOpen)
            await this.fetchParticipants();

        await this.setState({
            participateOpen: !this.state.participateOpen
        });
    }

    async componentDidMount() {
        await this.fetchContest();
        await this.fetchParticipants();
    }

    fetching() {
        return this.props.fetchingContests || this.props.fetchingUser || this.props.fetchingParticipants;
    }

    render() {
        if (!this.props.contest || this.fetching())
            return <CenterSpinner />;

        const { title, options } = this.props.contest;
        return <Container>
            <Row className="mb-3 justify-content-between">
                <h1>{title}</h1>
                {this.showParticipateButton() &&
                <Button color="success" onClick={this.toggleParticipate}>Принять участие</Button>}
                {this.state.participateOpen &&
                <ParticipateModal user={this.props.user}
                                  contest={this.props.contest}
                                  close={this.toggleParticipate}
                                  title="Принять участие"
                                  saveButtonTitle="Участвовать"
                />}
            </Row>
            <Row>
                <Col sm={9}>
                    <Switch>
                        <Route exact path={this.CONTEST}>
                            <News contestId={this.contestId} />
                        </Route>
                        <Route exact path={`${this.CONTEST}/participants`}>
                            <ParticipantsList contest={this.props.contest} participants={this.props.participants} />
                        </Route>

                        <Route exact path={`${this.CONTEST}/addNews`}>
                            <EditNews contestId={this.contestId} />
                        </Route>
                        <Route exact path={`${this.CONTEST}/options`}>
                            <Options />
                        </Route>
                        <Route exact path={`${this.CONTEST}/seating`}>
                            <Seating contestId={this.contestId} />
                        </Route>
                        <Route exact path={`${this.CONTEST}/addResult`}>
                            <AddResult contestId={this.contestId} />
                        </Route>
                    </Switch>
                </Col>
                <Col sm={3}>
                    <ListGroup>
                        <ListGroupItem>
                            <Link to={this.CONTEST}>Информация</Link>
                        </ListGroupItem>
                        <ListGroupItem>
                            <Link to={`${this.CONTEST}/participants`}>Участники</Link>
                        </ListGroupItem>
                        {hasFlag(options, ContestOptions.ResultsOpen) && <ListGroupItem>
                            <Link to={`${this.CONTEST}/results`}>Результаты</Link>
                        </ListGroupItem>}
                    </ListGroup>

                    {this.props.user && this.props.user.role === UserRole.Admin && <>
                        <h5 className="mt-3">Админка</h5>
                        <ListGroup>
                            <ListGroupItem>
                                <Link to={`${this.CONTEST}/addNews`}>Добавить новость</Link>
                            </ListGroupItem>
                            {!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                                <Link to={`${this.CONTEST}/seating`}>Сгенерировать рассадку</Link>
                            </ListGroupItem>}
                            {!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                                <Link to={`${this.CONTEST}/addResult`}>Добавить результаты</Link>
                            </ListGroupItem>}
                            <ListGroupItem>
                                <Link to={`${this.CONTEST}/options`}>Настройки</Link>
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

export default WithParticipants(WithUser(WithContest(Contest)));
