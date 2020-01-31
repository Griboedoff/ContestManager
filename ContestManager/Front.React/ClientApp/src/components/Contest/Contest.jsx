import React from 'react';
import { Link, Route, Switch } from 'react-router-dom';
import { Button, Col, Container, ListGroup, ListGroupItem, Row } from 'reactstrap';
import { ContestOptions } from '../../Enums/ContestOptions';
import { ContestType } from '../../Enums/ContestType';
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
import { ParticipateModal } from './ParticipateModal';
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

    contestUrl = (url = '') => `${this.CONTEST}/${url}`;

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

        const { user, setUser, contest } = this.props;
        return <Container>
            <Row className="mb-3 justify-content-between"><h1>{contest.title}</h1></Row>
            <Row>
                <Col sm={9}>
                    {this.renderBody()}
                </Col>
                <Col sm={3}>
                    {this.renderMenu()}
                </Col>
            </Row>
            {this.state.participateOpen &&
            <ParticipateModal user={user}
                              contest={contest}
                              close={this.toggleParticipate}
                              title="Принять участие"
                              saveButtonTitle="Участвовать"
                              setUser={setUser} />}
        </Container>;
    }

    renderBody = () => (<Switch>
        <Route exact path={this.contestUrl()}>
            <News contestId={this.contestId} />
        </Route>
        <Route exact path={this.contestUrl('participants')}>
            <ParticipantsList contest={this.props.contest} participants={this.props.participants} />
        </Route>

        <Route exact path={this.contestUrl('addNews')}>
            <EditNews contestId={this.contestId} />
        </Route>
        <Route exact path={this.contestUrl('options')}>
            <Options />
        </Route>
        <Route exact path={this.contestUrl('seating')}>
            <Seating contestId={this.contestId} />
        </Route>
        <Route exact path={this.contestUrl('addResult')}>
            <AddResult contestId={this.contestId} />
        </Route>
        <Route exact path={this.contestUrl('addTasks')}>
            <AddResult contestId={this.contestId} />
        </Route>
    </Switch>);

    renderMenu = () => {
        const { user, contest } = this.props;
        return <>
            {this.showParticipateButton() &&
            <Button className="mb-4" color="success" onClick={this.toggleParticipate}>Принять участие</Button>}
            <ListGroup>
                <ListGroupItem>
                    <Link to={this.contestUrl()}>Информация</Link>
                </ListGroupItem>
                <ListGroupItem>
                    <Link to={this.contestUrl('participants')}>Участники</Link>
                </ListGroupItem>
                {hasFlag(contest.options, ContestOptions.ResultsOpen) && <ListGroupItem>
                    <Link to={this.contestUrl('results')}>Результаты</Link>
                </ListGroupItem>}
            </ListGroup>

            {user?.role === UserRole.Admin && this.renderAdminPanel(contest.options)}
        </>;
    };

    renderAdminPanel = () => {
        const { options, type } = this.props.contest;

        return <>
            <h5 className="mt-3">Админка</h5>
            <ListGroup>
                {type === ContestType.Qualification && <ListGroupItem>
                    <Link to={this.contestUrl('addTasks')}>Добавить задания</Link>
                </ListGroupItem>}
                <ListGroupItem>
                    <Link to={this.contestUrl('addNews')}>Добавить новость</Link>
                </ListGroupItem>
                {!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                    <Link to={this.contestUrl('seating')}>Сгенерировать рассадку</Link>
                </ListGroupItem>}
                {!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                    <Link to={this.contestUrl('addResult')}>Добавить результаты</Link>
                </ListGroupItem>}
                {!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                    <Link to={this.contestUrl('addResult')}>Добавить результаты</Link>
                </ListGroupItem>}
                <ListGroupItem>
                    <Link to={this.contestUrl('options')}>Настройки</Link>
                </ListGroupItem>
            </ListGroup>
        </>;
    };

    showParticipateButton() {
        const { user, contest, participants } = this.props;
        return hasFlag(contest.options, ContestOptions.RegistrationOpen) &&
            user?.role === UserRole.Participant &&
            !participants.some(p => p.userId === user.id);
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
        if (this.props.contest?.id === this.contestId) {
            return;
        }

        this.props.getContest(this.contestId);
    }
}

export default WithParticipants(WithUser(WithContest(Contest)));
