import React from 'react';
import { Link } from 'react-router-dom';
import { Alert, Button, Col, Container, ListGroup, ListGroupItem, Row } from 'reactstrap';
import { ContestOptions } from '../../Enums/ContestOptions';
import { UserRole } from '../../Enums/UserRole';
import { post } from '../../Proxy';
import { hasFlag } from '../../utils';
import { CenterSpinner } from '../CenterSpinner';
import WithContest from '../HOC/WithContest';
import WithUser from '../HOC/WithUser';
import News from './News';
import AddNews from './News/AddNews';
import Options from './Options';

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

    componentDidMount() {
        if (this.props.contest && this.props.contest.id === this.contestId) {
            return;
        }

        this.props.getContest(this.contestId);
    }

    handleTabChange = n => _ => {
        this.setState({ activeTab: n });
    };

    fetching() {
        return this.props.fetchingContests || this.props.fetchingUser;
    }

    render() {
        if (!this.props.contest || this.fetching())
            return <CenterSpinner />;

        return <Container>
            <Row><h2 className="mb-3">{this.props.contest.title}</h2></Row>
            <Row>
                <Col sm={9}>
                    {hasFlag(this.props.contest.options, ContestOptions.RegistrationOpen) &&
                    this.props.user && this.props.user.role === UserRole.Participant &&
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
                        {hasFlag(this.props.contest.options, ContestOptions.ResultsOpen) && <ListGroupItem>
                            <a className="link" onClick={this.handleTabChange(Tab.Results)}>Результаты</a>
                        </ListGroupItem>}
                        {this.props.user && this.props.user.role === UserRole.Admin && <>
                            <ListGroupItem>
                                <a className="link" onClick={this.handleTabChange(Tab.AddNews)}>Добавить новость</a>
                            </ListGroupItem>
                            <ListGroupItem>
                                <a className="link" onClick={this.handleTabChange(Tab.Options)}>Настройки</a>
                            </ListGroupItem>
                        </>}
                    </ListGroup>
                </Col>
            </Row>
        </Container>;
    }

    renderTab() {
        switch (this.state.activeTab) {
            case Tab.AddNews:
                return <AddNews contestId={this.contestId} />;
            case Tab.Options:
                return <Options />;
            case Tab.News:
            default:
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
}

const Tab = {
    News: 1,
    Results: 2,
    AddNews: 3,
    Options: 4,
};

export default WithUser(WithContest(Contest));
