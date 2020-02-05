import React from 'react';
import Alert from 'reactstrap/lib/Alert';
import { QualificationSolveState } from '../../Enums/QualificationSolveState';
import { get } from '../../Proxy';
import { CenterSpinner } from '../CenterSpinner';
import { WithSetError } from '../HOC/WithSetError';
import WithUser from '../HOC/WithUser';
import { QualificationGreet } from './QualificationGreet';
import { QualificationTasksViewWrapper } from './QualificationTasks';

class Qualification extends React.Component {
    constructor(props) {
        super(props);
        this.contestId = this.props.match.params.id;

        this.state = {
            fetching: true,
            qualificationState: QualificationSolveState.NotStarted
        };
    }

    async componentDidMount() {
        this.setState({ error: false });
        const resp = await get(`qualification/state?contestId=${this.contestId}`);
        if (!resp.ok) {
            this.props.setRequestError(resp, 'Не удалось начать тур.');
        } else {
            const state = await resp.json();
            this.setState({ qualificationState: state });
        }
        this.setState({ fetching: false });
    }

    render() {
        if (this.state.fetching)
            return <CenterSpinner />;

        return <>
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

export default WithSetError(WithUser(Qualification));
