import React from 'react';
import Alert from 'reactstrap/lib/Alert';
import { QualificationSolveState } from '../../Enums/QualificationSolveState';
import { get } from '../../Proxy';
import { CenterSpinner } from '../CenterSpinner';
import WithUser from '../HOC/WithUser';
import { QualificationGreet } from './QualificationGreet';
import { QualificationTasksView } from './QualificationTasks';


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
        const resp = await get(`qualification/${this.contestId}/start`);
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

        switch (this.state.qualificationState) {
            case QualificationSolveState.NotStarted:
                return <QualificationGreet
                    contestId={this.contestId}
                    onStart={() => this.setState({ qualificationState: QualificationSolveState.InProgress })}
                    setError={error => this.setState({ error })} />;
            case QualificationSolveState.InProgress:
                return <QualificationTasksView contestId={this.contestId} />;
            default:
                return <Alert color="secondary">Ваше время закончилось. Результаты будут доступны после завершения тура.</Alert>;
        }
    }
}

export default WithUser(Qualification);
