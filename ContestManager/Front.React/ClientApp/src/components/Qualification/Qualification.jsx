import React from 'react';
import { Link } from 'react-router-dom';
import Alert from 'reactstrap/lib/Alert';
import { QualificationSolveState } from '../../Enums/QualificationSolveState';
import { get } from '../../Proxy';
import { CenterSpinner } from '../CenterSpinner';
import WithUser from '../HOC/WithUser';
import { QualificationGreet } from './QualificationGreet';
import { QualificationTasksViewWrapper } from './QualificationTasks';

class Qualification extends React.Component {
    constructor(props) {
        super(props);
        this.contestId = this.props.match.params.id;

        this.state = {
            fetching: true,
            error: false,
            qualificationState: QualificationSolveState.NotStarted
        };
    }

    async componentDidMount() {
        this.setState({ error: false });
        const resp = await get(`qualification/state?contestId=${this.contestId}`);
        if (!resp.ok) {
            if (resp.status === 403)
                this.setState({ error: 'Неавторизованный пользователь' });
            else if (resp.status === 400)
                this.setState({ error: 'Вы не являетесь участником этого соревнования' });
        } else {
            const state = await resp.json();
            this.setState({ qualificationState: state });
        }
        this.setState({ fetching: false });
    }

    render() {
        if (this.state.fetching)
            return <CenterSpinner />;

        if (!this.props.user)
            return <Alert color="danger"> <Link to="/login">Войдите</Link> в систему.</Alert>;

        return <>
            {this.state.error && <Alert color="danger">{this.state.error}</Alert>}
            {this.renderContent()}
        </>;
    }

    renderContent() {
        switch (this.state.qualificationState) {
            case QualificationSolveState.NotStarted:
                return <QualificationGreet
                    contestId={this.contestId}
                    onStart={() => this.setState({ qualificationState: QualificationSolveState.InProgress })}
                    setError={error => this.setState({ error })} />;
            case QualificationSolveState.InProgress:
                return <QualificationTasksViewWrapper contestId={this.contestId} />;
            default:
                return <Alert color="secondary">
                    Ваше время закончилось. Результаты будут доступны после завершения тура.
                </Alert>;
        }
    }
}

export default WithUser(Qualification);
