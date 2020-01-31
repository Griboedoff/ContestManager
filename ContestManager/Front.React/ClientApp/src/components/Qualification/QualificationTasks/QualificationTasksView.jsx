import update from 'immutability-helper';
import React from 'react';
import { Alert, Button, Col, Container, ListGroup, ListGroupItem, Row, Spinner } from 'reactstrap';
import { AvForm, AvGroup, AvInput, AvFeedback } from 'availity-reactstrap-validation';
import { get, post } from '../../../Proxy';
import { CenterSpinner } from '../../CenterSpinner';
import { CountdownWrapper } from './Countdown';

const ReactMarkdown = require('react-markdown/with-html');

export class QualificationTasksViewWrapper extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            fetching: false,
            tasks: [],
            answers: [],
            currentTask: 0,
            error: false,
            saved: true,
            timeLeft: 0,
        };
    }

    async componentDidMount() {
        this.setState({ fetching: true, error: false });
        const resp = await get(`qualification?contestId=${this.props.contestId}`);
        if (resp.ok) {
            const data = await resp.json();
            this.setState({ ...data });
        } else {
            this.setState({ error: true });
        }
        this.setState({ fetching: false });
    }

    render() {
        const { fetching, timeLeft, tasks, answers, title, error } = this.state;
        if (fetching)
            return <CenterSpinner />;

        if (error)
            return <Alert color="danger">Не удалось загрузить задания</Alert>;

        return <Container>
            <Row className="mb-2">
                <Col sm={9}><h1>{title}</h1></Col>
                <Col sm={3}>
                    <div className="mb-4">
                        <CountdownWrapper seconds={timeLeft} />
                    </div>
                </Col>
            </Row>
            <QualificationTasksView tasks={tasks} answers={answers} contestId={this.props.contestId} />
        </Container>;
    }
}

class QualificationTasksView extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            tasks: this.props.tasks,
            answers: this.props.answers,
            currentTask: 0,
            error: false,
            saved: true,
        };
    }

    setAnswer = answer => this.setState(state => {
        return {
            ...state,
            answers: update(state.answers, { [state.currentTask]: { $set: answer } }),
            saved: false
        };
    });

    saveAnswers = async () => {
        if (!this.state.saved) {
            this.setState({ error: false, saving: true });

            const [resp] = await Promise.all([
                post(`qualification/save?contestId=${this.props.contestId}`, this.state.answers),
                new Promise(resolve => setTimeout(resolve, 500))]);
            if (resp.ok) {
                this.setState({ saved: true });
            } else {
                this.setState({ error: 'Не удалось сохранить ответы' });
            }
            this.setState({ saving: false });
        }
    };

    changeTab = i => this.setState({ currentTask: i });

    render() {
        const { tasks, currentTask, answers, saved, saving, error } = this.state;

        return <>
            {error && <Alert color='danger'>{error}</Alert>}
            <Row>
                <Col sm={9}>
                    <ReactMarkdown className="mb-5" source={tasks[currentTask]} escapeHtml={false} />
                    <AvForm className="align-items-start" inline onSubmit={
                        async (event, errors) => {
                            if (!errors.length)
                                await this.saveAnswers();
                        }}>
                        <AvGroup className="mb-2 mr-sm-4 mb-sm-0">
                            <AvInput id="ans"
                                     name="ans"
                                     autoComplete="off"
                                     onChange={e => this.setAnswer(e.target.value)}
                                     value={answers[currentTask]}
                                     pattern={"^[-\\d\\.]+$"} />
                            <AvFeedback>Ответ — это число</AvFeedback>
                        </AvGroup>
                        <Button disabled={saved}>
                            {saving && <Spinner size="sm" color="light" />}{' '}
                            Сохранить ответ
                        </Button>
                    </AvForm>
                </Col>
                <Col sm={3}>
                    <ListGroup>
                        {tasks.map((t, i) => (
                            <ListGroupItem style={{ cursor: 'pointer' }} {...this.calcColor(i)}
                                           key={i}
                                           onClick={() => this.changeTab(i)}>
                            <span className="pseudo-link">
                                Задание {i + 1}
                            </span>
                            </ListGroupItem>))}
                    </ListGroup>
                </Col>
            </Row>
        </>;
    }

    calcColor = (i) => {
        if (i === this.state.currentTask)
            return { color: "info" };
        if (!!this.state.answers[i])
            return { color: "success" };
        return null;
    };
}

