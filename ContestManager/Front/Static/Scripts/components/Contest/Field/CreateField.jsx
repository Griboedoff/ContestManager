import React from 'react';
import {Button, ControlLabel, Form, FormControl, FormGroup} from 'react-bootstrap';
import Col from 'react-bootstrap/es/Col';

class CreateField extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            title: '',
            fieldType: 'String',
        };
    }

    handleNameChange = e => this.setState({title: e.target.value,});
    handleTypeChange = e => this.setState({fieldType: e.target.value,});

    render() {
        return (
            <Col smOffset={2}>
                <Form inline>
                    <FormGroup controlId="formInlineName">
                        <ControlLabel>Название поля</ControlLabel>{'    '}
                        <FormControl type="text" placeholder="Название" onChange={this.handleNameChange} />
                    </FormGroup>{'    '}
                    <FormGroup controlId="formInlineType">
                        <ControlLabel>Тип</ControlLabel>{'    '}
                        <FormControl componentClass="select"
                                     placeholder="выбери тип"
                                     onChange={this.handleTypeChange}>
                            <option value="String">Строка</option>
                            <option value="Number">Число</option>
                            <option value="Date">Дата</option>
                        </FormControl>
                    </FormGroup>{'    '}
                    <Button onClick={() => this.props.onClick(this.state)}>Добавить</Button>
                </Form>
            </Col>);
    };
}

export default CreateField;