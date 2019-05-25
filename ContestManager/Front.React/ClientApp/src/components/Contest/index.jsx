import React from 'react';
import { Link } from 'react-router-dom';
import {
    Alert,
    Button,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownToggle,
    Nav,
    NavItem,
    NavLink
} from 'reactstrap';
import { ContestState } from '../../Enums/ContestsState';
import { UserRole } from '../../Enums/UserRole';
import { post } from '../../Proxy';
import { CenterSpinner } from '../CenterSpinner';
import WithContest from '../HOC/WithContest';
import WithUser from '../HOC/WithUser';
import News from './News';
import AddNews from './News/AddNews';

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

        return <>
            <h2 className="mb-3">{this.props.contest.title}</h2>
            {/*{this.props.user && this.props.user.role === UserRole.Participant &&*/}
            <Button className="mb-3" onClick={this.participate}>Принять участие</Button>
            {this.state.participateError && <Alert color="danger">{this.state.participateError}</Alert>}
            {this.state.participateSuccess &&
            <Alert color="success">Вы зарегистрированы. Не забудте <Link to="/user">обновить данные о
                себе</Link></Alert>}
            <Nav tabs>
                <NavItem>
                    <NavLink href="#"
                             active={this.state.activeTab === Tab.News}
                             onClick={this.handleTabChange(Tab.News)}>Информация</NavLink>
                </NavItem>
                {this.props.contest.state === ContestState.Finished && <NavItem>
                    <NavLink href="#"
                             active={this.state.activeTab === Tab.Results}
                             onClick={this.handleTabChange(Tab.Results)}>Результаты</NavLink>
                </NavItem>}
                {this.props.user && this.props.user.role === UserRole.Admin &&
                <Dropdown isOpen={this.state.dropdownOpen} toggle={this.toggleDropdown} className="ml-auto">
                    <DropdownToggle nav caret>
                        Админка
                    </DropdownToggle>
                    <DropdownMenu>
                        <DropdownItem onClick={this.handleTabChange(Tab.AddNews)}>Добавить новость</DropdownItem>
                        <DropdownItem onClick={this.handleTabChange(Tab.Settings)}>Настройки</DropdownItem>
                    </DropdownMenu>
                </Dropdown>}
            </Nav>
            {this.renderTab()}
        </>;
    }

    renderTab() {
        switch (this.state.activeTab) {
            case Tab.AddNews:
                return <AddNews contestId={this.contestId} />;
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
    Settings: 4,
};

export default WithUser(WithContest(Contest));
