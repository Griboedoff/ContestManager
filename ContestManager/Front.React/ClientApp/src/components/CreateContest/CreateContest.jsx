import React from 'react';
import { Alert, Button, Col, Form, FormGroup, Input, Label } from 'reactstrap';
import { UserRole } from '../../Enums/UserRole';
import { post } from '../../Proxy';
import withUser from '../HOC/WithUser';

class CreateContest extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.createContest = this.createContest.bind(this);

        this.state = {
            title: '',
            type: 0,
            error: false,
        };
    }

    createContest() {
        if (!this.props.user)
            return;

        this.setState({ isProcessing: false, error: false });

        post('contests', {
            title: this.state.title,
            type: this.state.type,
        }).then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            throw Error();
        }).then(contest => {
            this.props.history.push(`/contests/${contest.id}`);
        }).catch(() => this.setState({ error: true }));
    };

    handleChange = async (event) => {
        const { target } = event;
        const { name } = target;
        await this.setState({
            [name]: target.value,
        });
    };

    render() {
        if (!this.props.user || this.props.user.role !== UserRole.Admin)
            return <Alert color="danger">Недостаточно прав </Alert>;

        return <div className="form-container">
            {
                this.state.error && <Alert color="danger">
                    Ой! Что-то пошло не так
                    <br />
                    Попробуйте позже
                </Alert>
            }
            <Form>
                <FormGroup row>
                    <Label sm={3}>Название</Label>
                    <Col sm={9}>
                        <Input type="text" name="title" onChange={this.handleChange} />
                    </Col>
                </FormGroup>
                <FormGroup row>
                    <Label sm={3}>Тип</Label>
                    <Col sm={9}>
                        <Input type="select" name="type" onChange={this.handleChange}>
                            <option value={0}>С подтверждением</option>
                            <option value={1}>Без подтверждения</option>
                        </Input>
                    </Col>
                </FormGroup>

                <FormGroup row>
                    <Col sm={{ size: 6, offset: 3 }}>
                        <Button onClick={this.createContest}>Создать</Button>
                    </Col>
                </FormGroup>
            </Form>
        </div>;
    }
}

export default withUser(CreateContest);
