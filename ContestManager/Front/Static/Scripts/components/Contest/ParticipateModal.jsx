import React from 'react';
import {Button, Modal, Form, Alert} from 'react-bootstrap';
import DateFormGroupWithTooltip from '../FormFields/DateFormGroupWithTooltip';
import NumberFormGroupWithTooltip from '../FormFields/NumberFormGroupWithTooltip';
import StringFormGroupWithTooltip from '../FormFields/StringFormGroupWithTooltip';
import Axios from 'axios';

class ParticipateModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {error: false};
        for (let f of props.contest.Fields)
            this.state[f.Title] = '';
    }

    handleChange = (k) => (e) => this.setState({
        error: false,
        [k]: e.target.value
    });

    createFields = (fields) => {
        const res = [];
        for (let i = 0; i < fields.length; i++) {
            const f = fields[i];
            let handleChange = this.handleChange(f.Title);

            let comp;
            switch (f.FieldType) {
                case "String":
                    comp = <StringFormGroupWithTooltip key={f.Title}
                                                       onChange={handleChange}
                                                       value={this.state[f.Title]}
                                                       label={f.Title}
                                                       overlay=""
                                                       controlId={`f${i}`} />;
                    break;
                case "Number":
                    comp = <NumberFormGroupWithTooltip key={f.Title}
                                                       onChange={handleChange}
                                                       value={this.state[f.Title]}
                                                       label={f.Title}
                                                       overlay=""
                                                       controlId={`f${i}`} />;
                    break;
                case "Date":
                    comp = <DateFormGroupWithTooltip key={f.Title}
                                                     onChange={handleChange}
                                                     value={this.state[f.Title]}
                                                     label={f.Title}
                                                     overlay=""
                                                     controlId={`f${i}`} />;
                    break;
            }

            res.push(comp);
        }

        return res;
    };

    send = () => {
        console.log(this.state);
        const fields = [];
        for (let f of this.props.contest.Fields)
            if (!this.state[f.Title])
                fields.push(f.Title);
        if (fields.length)
            this.setState({error: fields});
        else
            Axios
                .post(`/contests/${this.props.contest.Id}/participate`,
                    this.props.contest.Fields.map(f => {
                        return {
                            Title: f.Title,
                            value: this.state[f.Title]
                        };
                    })
                )
                .then(this.props.handleHide());
    };

    render() {
        return (
            <Modal key="Modal" show={this.props.show} onHide={this.props.handleHide}>
                <Modal.Header closeButton>
                    <Modal.Title>Заполните форму регистрации</Modal.Title>
                </Modal.Header>
                {this.state.error ? <Alert bsStyle='danger'>Заполните поля: {this.state.error.join(', ')}</Alert> : ''}
                <Modal.Body>
                    <Form style={{display: 'grid'}}>
                        {this.createFields(this.props.contest.Fields)}
                    </Form>
                </Modal.Body>

                <Modal.Footer>
                    <Button bsStyle="primary" onClick={this.send}>Участвовать</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}

export default ParticipateModal;