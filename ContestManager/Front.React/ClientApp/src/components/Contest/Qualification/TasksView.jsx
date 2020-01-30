import React from 'react';
import Markdown from 'react-markdown';
import { Button, Col, Container, Form, FormGroup, Label, ListGroup, ListGroupItem, Row } from 'reactstrap';
import Input from 'reactstrap/lib/Input';
import { get, post } from '../../../Proxy';
import update from 'immutability-helper';
import { CenterSpinner } from '../../CenterSpinner';
import { Countdown } from './Countdown';

export class TasksView extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            fetching: false,
            tasks: [],
            answers: [],
            currentTask: 1,
            error: false,
            saved: true,
            timeLeft: 0,
        };
    }

    async componentDidMount() {
        this.setState({ fetching: true, error: false });
        const resp = await get(`tasks?contestId=${this.props.contestId}`);
        if (resp.ok) {
            const data = await resp.json();
            this.setState({ ...data });
        } else {
            this.setState({ error: 'Не удалось загрузить задания' });
        }
        this.setState({ fetching: false });
    }

    setAnswer = (answer) => {
        this.setState(state => {
            return {
                ...state,
                answers: update(state.answers, { [state.currentTask]: { $set: answer } }),
                saved: false
            };
        });
    };

    saveAnswers = async () => {
        if (!this.state.saved) {
            this.setState({ error: false });

            const resp = await post(`tasks/save?contestId=${this.props.contestId}`, { answers: this.state.answers });
            if (resp.ok) {
                this.setState({ saved: true });
            } else {
                this.setState({ error: 'Не удалось сохранить ответы' });
            }
        }
    };

    changeTab = (i) => this.setState({ currentTask: i });

    render() {
        if (this.state.fetching)
            return <CenterSpinner />;

        return <Container>
            <Row>
                <Col sm={9}>
                    <Markdown source={this.state.tasks[this.state.currentTask]} />
                    <Form inline>
                        <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                            <Label for="ans" className="mr-sm-2">Ответ</Label>
                            <Input id="ans" onChange={e => this.setAnswer(e.target.value)} />
                        </FormGroup>
                        <Button onClick={async () => await this.saveAnswers()}>Сохранить ответ</Button>
                    </Form>
                </Col>
                <Col sm={3}>
                    <Countdown seconds={this.state.timeLeft} />
                    <ListGroup>
                        {this.state.tasks.map((t, i) => {
                            return <ListGroupItem {this.calcColor(i)}>
                                <span className="pseudo-link" onClick={() => this.changeTab(i)}>
                                    Задание {i}
                                </span>
                            </ListGroupItem>;
                        })}
                    </ListGroup>
                </Col>
            </Row>
        </Container>;
    }

    calcColor = (i) => {
        if (i === this.state.currentTask)
            return { color: "warning" };
        if (!!this.state.answers[i])
            return { color: "success" };
        return null;
    };
}

