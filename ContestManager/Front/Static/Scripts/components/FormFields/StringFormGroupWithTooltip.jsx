import React from 'react';
import {ValidSymbolValidator} from '../../Common/Validators';
import FormGroupWithTooltip from './FormGroupWithTooltip';

class StringFormGroupWithTooltip extends React.Component {
    validateValue = (val) => val
        ? (ValidSymbolValidator.validate(val)
            ? 'success'
            : 'error')
        : null;

    render() {
        return <FormGroupWithTooltip controlId={this.props.controlId}
                                     label={this.props.label}
                                     validationState={this.validateValue(this.props.value)}
                                     overlay={this.props.overlay}
                                     value={this.props.value}
                                     onChange={this.onChange}
                                     ph=""
                                     type="input" />;
    }
}

export default StringFormGroupWithTooltip;