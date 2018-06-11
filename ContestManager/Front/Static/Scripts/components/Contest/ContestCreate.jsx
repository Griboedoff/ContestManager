import Axios from 'axios/index';
import React from 'react';
import {Button, Col, ControlLabel, Form, FormControl, FormGroup} from 'react-bootstrap';
import ContestFields from './Field/ContestFields';
import CreateField from './Field/CreateField';

class ContestCreate extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            fields: [],
            name: '',
        };
    }

    onClick = field => {
        if (field.title && field.fieldType) {
            this.setState(prevState => ({
                fields: [...prevState.fields, field]
            }));
        }
    };

    removeItem = item => {
        this.setState(prevState => ({
            fields: prevState.fields.filter(i => i.name !== item.name)
        }));
    };

    handleNameChange = e => this.setState({name: e.target.value,});

    create = () => {
        Axios.post('contests/create', {
            name: this.state.name,
            fields: JSON.stringify(this.state.fields),
        })
            .then(() => {
                    this.props.history.push(`/`);
                    this.props.onLogIn();
                }
            ).catch(() => this.setState({error: true}))
    };

    render() {
        return (
            <div className="fields-table">
                <Form horizontal>
                    <FormGroup controlId="formInlineName">
                        <Col sm={5} componentClass={ControlLabel}>
                            Название соревнования
                        </Col>
                        <Col sm={5}>
                            <FormControl type="text" placeholder="Название" onChange={this.handleNameChange} />
                        </Col>
                    </FormGroup>
                </Form>
                <br />
                <ContestFields fields={this.state.fields} removeItem={this.removeItem} />
                <CreateField onClick={this.onClick} />
                <Button onClick={this.create}>Создать</Button>
            </div>
        );
    };
}

export default ContestCreate;