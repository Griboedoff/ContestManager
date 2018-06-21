import React from 'react';
import {Col, ControlLabel, FormControl, FormGroup, OverlayTrigger, Tooltip} from "react-bootstrap";

const tt = (val) => <Tooltip id={`tooltipHelp${Math.random()}`}>{val}</Tooltip>;

const getOverlay = (val, text) => {
    switch (val) {
        case 'error':
            return tt(text);
        case 'warning':
            return tt("Обязательное поле");
        case 'success':
        case null:
            return <div />;
    }
};

class FormGroupWithTooltip extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        const formControl = (
            <Col sm={this.props.size || 5}>
                <FormControl type={this.props.type}
                             value={this.props.value}
                             placeholder={this.props.ph}
                             onChange={this.props.onChange} />
                <FormControl.Feedback />
            </Col>);

        return <FormGroup controlId={this.props.controlId}
                          validationState={this.props.validationState}>
            <Col componentClass={ControlLabel} sm={4}>
                {this.props.label}
            </Col>
            <OverlayTrigger placement="right" overlay={getOverlay(this.props.validationState, this.props.overlay)}>
                {formControl}
            </OverlayTrigger>
        </FormGroup>;
    }
}

export default FormGroupWithTooltip;