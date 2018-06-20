import React from 'react';
import {Button, Navbar, Modal, Form} from 'react-bootstrap';
import DateFormGroupWithTooltip from '../FormFields/DateFormGroupWithTooltip';
import NumberFormGroupWithTooltip from '../FormFields/NumberFormGroupWithTooltip';
import StringFormGroupWithTooltip from '../FormFields/StringFormGroupWithTooltip';

class ParticipateModal extends React.Component {
    constructor(props) {
        super(props);

        const val = [];
        for (let _ in props.contest.Fields) {
            val.push('');
        }

        this.state = {
            values: val,
        };
    }

    handleChange = (i) => {
        return function(e) {
            console.log(123213)
            const newValues = this.state.values.slice();
            newValues[i] = e.target.valueOf();
            this.setState({values: newValues});
        };
    };

    createFields(fields) {
        const res = [];
        for (let i = 0; i < fields.length; i++) {
            const f = fields[i];
            let comp;
            let handleChange = this.handleChange(i);
            switch (f.FieldType) {
                case "String":
                    comp = <StringFormGroupWithTooltip onChange={handleChange}
                                                       value={this.state.values[i]}
                                                       label={f.Title}
                                                       overlay=""
                                                       controlId={`f${i}`}

                    />;
                    break;
                case "Number":
                    comp = <NumberFormGroupWithTooltip onChange={handleChange}
                                                       value={this.state.values[i]}
                                                       label={f.Title}
                                                       overlay=""
                                                       controlId={`f${i}`} />;
                    break;
                case "Date":
                    comp = <DateFormGroupWithTooltip onChange={handleChange}
                                                     value={this.state.values[i]}
                                                     label={f.Title}
                                                     overlay=""
                                                     controlId={`f${i}`} />;
                    break;
            }

            res.push(comp);
        }

        return res;
    }

    send = () => {

    };

    render() {
        return (
            <Modal show={this.props.show} onHide={this.props.handleHide}>
                <Modal.Header closeButton>
                    <Modal.Title>Заполните форму регистрации</Modal.Title>
                </Modal.Header>
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