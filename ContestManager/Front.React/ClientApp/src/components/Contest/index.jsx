import React from 'react';
import { Nav, NavItem, NavLink } from 'reactstrap';
import Spinner from 'reactstrap/es/Spinner';
import { ContestState } from '../../Enums/ContestsState';
import { UserRole } from '../../Enums/UserRole';
import WithContest from '../HOC/WithContest';
import WithUser from '../HOC/WithUser';
import News from './News';

class Contest extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            activeTab: 1,
        };

    }

    componentDidMount() {
        const contestId = this.props.match.params.id;

        if (this.props.contest && this.props.contest.id === contestId) {
            return;
        }

        this.props.getContest(contestId);
    }


    handleTabChange = n => _ => {
        this.setState({ activeTab: n });
    };

    render() {
        if (!this.props.contest || this.props.fetching)
            return <Spinner style={{ width: '3rem', height: '3rem' }} />;

        return <>
            <h2 className="mb-3">{this.props.contest.title}</h2>
            <Nav tabs>
                <NavItem>
                    <NavLink href="#"
                             active={this.state.activeTab === 1}
                             onClick={this.handleTabChange(1)}>Информация</NavLink>
                </NavItem>
                {this.props.contest.state === ContestState.Finished && <NavItem>
                    <NavLink href="#"
                             active={this.state.activeTab === 2}
                             onClick={this.handleTabChange(2)}>Результаты</NavLink>
                </NavItem>}
                {this.props.user && this.props.user.role === UserRole.Admin && <NavItem>
                    <NavLink href="#"
                             active={this.state.activeTab === 3}
                             onClick={this.handleTabChange(3)}>Настройки</NavLink>
                </NavItem>}
            </Nav>
            {this.renderTab()}
        </>;
    }

    renderTab() {
        switch (this.state.activeTab) {
            case 1:
                return <News contestId={this.props.contest.id} />;

            default:
                return null;
        }
    }
}

export default WithUser(WithContest(Contest));
