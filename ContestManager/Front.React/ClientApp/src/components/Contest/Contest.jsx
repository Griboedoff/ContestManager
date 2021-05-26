import React from 'react';
import {Link, Route, Switch} from 'react-router-dom';
import {Button, Col, Container, Label, ListGroup, ListGroupItem, Row} from 'reactstrap';
import {ContestOptions} from '../../Enums/ContestOptions';
import {ContestType} from '../../Enums/ContestType';
import {UserRole} from '../../Enums/UserRole';
import {hasFlag} from '../../utils';
import {CenterSpinner} from '../CenterSpinner';
import WithContest from '../HOC/WithContest';
import WithParticipants from '../HOC/WithParticipants';
import WithResults from '../HOC/WithResults';
import WithUser from '../HOC/WithUser';
import {AddResult} from './AddResult';
import {EditNews, News} from './News';
import {NotVerifiedList} from './NotVerifiedList';
import {Options} from './Options';
import {ParticipantsList} from './ParticipantsList';
import {ParticipateModal} from './ParticipateModal';
import {Results} from './Results';
import {Seating} from './Seating';
import {get} from '../../Proxy';

class Contest extends React.Component {
    constructor(props) {
        super(props);
        this.contestId = this.props.match.params.id;
        this.CONTEST = this.props.match.url;

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
            await this.props.fetchParticipants();

        await this.setState({
            participateOpen: !this.state.participateOpen
        });
    }

    async componentDidMount() {
        await Promise.all(
            [
                this.fetchContest(),
                this.props.fetchParticipants(this.contestId),
                this.props.fetchResults(this.contestId),
            ]
        );
    }

    fetching() {
        return this.props.fetchingContests || this.props.fetchingUser || this.props.fetchingParticipants || this.props.fetchingResults;
    }

    render() {
        if (!this.props.contest || this.fetching())
            return <CenterSpinner/>;

        const {user, setUser, contest} = this.props;
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
                              setUser={setUser}/>}
        </Container>;
    }

    renderBody = () => (<Switch>
        <Route exact path={this.contestUrl()}>
            <News contestId={this.contestId}/>
        </Route>
        <Route exact path={this.contestUrl('participants')}>
            <ParticipantsList contest={this.props.contest} participants={this.props.participants}/>
        </Route>

        <Route exact path={this.contestUrl('addNews')}>
            <EditNews contestId={this.contestId}/>
        </Route>
        <Route exact path={this.contestUrl('options')}>
            <Options contest={this.props.contest}/>
        </Route>
        <Route exact path={this.contestUrl('seating')}>
            <Seating contestId={this.contestId}/>
        </Route>
        <Route exact path={this.contestUrl('addResult')}>
            <AddResult contestId={this.contestId}/>
        </Route>
        <Route path={this.contestUrl('results/:class')}>
            <Results contest={this.props.contest} results={this.props.results}/>
        </Route>
        <Route exact path={this.contestUrl('notVerified')}>
            <NotVerifiedList contestId={this.contestId}/>
        </Route>
        {/*<Route exact path={this.contestUrl('addTasks')}>*/}
        {/*    <AddResult contestId={this.contestId} />*/}
        {/*</Route>*/}
    </Switch>);

    
    renderMenu = () => {
        const {user, contest} = this.props;
        return <>
            {this.showParticipateButton() &&
            <Button className="mb-4" color="success" onClick={this.toggleParticipate}>Принять участие</Button>}

            {this.showQualificationButton() &&
            <Button className="mb-4" color="success" tag={Link} to={`/contest/${this.contestId}/qualification`}>
                Отборочный тур
            </Button>}

            {this.showBadgesButton()
            && <div>
                <Button className="mb-4" color="success" onClick={async () => {
                    const response = await get(`badges/print?contestId=${this.contestId}`);

                    if (response.ok) {
                        const blob = await response.blob();
                        const url = window.URL.createObjectURL(new Blob([blob]));
                        const link = document.createElement('a');
                        link.href = url;
                        link.setAttribute('download', `${user.name}.pdf`);
                        document.body.appendChild(link);
                        link.click();
                        link.parentNode.removeChild(link);
                    }
                }}>
                    Напечатать бейдж
                </Button>
            </div>}

            {this.showUserLogin() &&
            <div>
                <Label className="mb-4">Ваш код: {this.props.participants.find(p => p.userId === user?.id)?.login}
                </Label>
            </div>}

            <ListGroup>
                <ListGroupItem>
                    <Link to={this.contestUrl()}>Информация</Link>
                </ListGroupItem>
                <ListGroupItem>
                    <Link to={this.contestUrl('participants')}>Участники</Link>
                </ListGroupItem>
                {(hasFlag(contest.options, ContestOptions.ResultsOpen) || hasFlag(contest.options, ContestOptions.PreResultsOpen) || this.props.user?.role === UserRole.Admin) &&
                <ListGroupItem>
                    <Link to={this.contestUrl('results/5')}>Результаты</Link>
                </ListGroupItem>}
            </ListGroup>

            {user?.role === UserRole.Admin && this.renderAdminPanel(contest.options)}
        </>;
    };

    showParticipateButton() {
        const {user, contest, participants} = this.props;
        return hasFlag(contest.options, ContestOptions.RegistrationOpen) &&
            user?.role === UserRole.Participant &&
            !participants.some(p => p.userId === user.id);
    }

    showQualificationButton() {
        const {user, contest, participants} = this.props;
        return hasFlag(contest.options, ContestOptions.QualificationOpen) &&
            user?.role === UserRole.Participant &&
            participants.some(p => p.userId === user.id);
    }

    showUserLogin() {
        const {user, contest} = this.props;
        return hasFlag(contest.options, ContestOptions.FilterVerified) &&
            user?.role === UserRole.Participant
    }

    showBadgesButton() {
        const {user, contest, participants} = this.props;
        const part = participants.find(p => p.userId === user?.id);
        return hasFlag(contest.options, ContestOptions.FilterVerified) &&
            user?.role === UserRole.Participant &&
            part?.verified;
    }

    renderAdminPanel = () => {
        const {options, type} = this.props.contest;
        const isQualification = type === ContestType.Qualification;
        return <>
            <h5 className="mt-3">Админка</h5>
            <ListGroup>
                {/*{isQualification && <ListGroupItem>*/}
                {/*    <Link to={this.contestUrl('addTasks')}>Добавить задания</Link>*/}
                {/*</ListGroupItem>}*/}
                <ListGroupItem>
                    <Link to={this.contestUrl('addNews')}>Добавить новость</Link>
                </ListGroupItem>
                {/*{!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>*/}
                {/*    <Link to={this.contestUrl('seating')}>Сгенерировать рассадку</Link>*/}
                {/*</ListGroupItem>}*/}
                {!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>
                    <Link to={this.contestUrl('addResult')}>Добавить результаты</Link>
                </ListGroupItem>}
                {/*{!hasFlag(options, ContestOptions.RegistrationOpen) && <ListGroupItem>*/}
                {/*    <Link to={this.contestUrl('addResult')}>Добавить результаты</Link>*/}
                {/*</ListGroupItem>}*/}
                {!isQualification && <ListGroupItem>
                    <Link to={this.contestUrl('notVerified')}>Без подтверждения</Link>
                </ListGroupItem>}
                <ListGroupItem>
                    <Link to={this.contestUrl('options')}>Настройки</Link>
                </ListGroupItem>
            </ListGroup>
        </>;
    };

    async fetchContest() {
        if (this.props.contest?.id === this.contestId) {
            return;
        }

        this.props.getContest(this.contestId);
    }
}

export default WithResults(WithParticipants(WithUser(WithContest(Contest))));
