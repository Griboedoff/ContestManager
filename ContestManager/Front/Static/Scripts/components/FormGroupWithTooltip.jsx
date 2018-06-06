import React from 'react';
import {Col, ControlLabel, FormControl, FormGroup, OverlayTrigger} from "react-bootstrap";

class FormGroupWithTooltip extends React.Component {
    constructor(props) {
        super(props);
        this.fc = (<Col sm={5}>
            <FormControl type={this.props.type}
                         value={this.props.value}
                         placeholder={this.props.ph}
                         onChange={this.props.onChange} />
            <FormControl.Feedback />
        </Col>)
    }

    showOverlay = () => {
        switch (this.props.validationState) {
            case 'error':
            case 'warning':
                return true;
            case 'success':
            case null:
                return false;
        }
    };

    render() {
        const formControl = (
            <Col sm={5}>
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
            <OverlayTrigger placement="right" overlay={this.props.overlay}>
                {formControl}
            </OverlayTrigger>
        </FormGroup>
    }
}

export default FormGroupWithTooltip