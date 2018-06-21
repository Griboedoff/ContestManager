import React from 'react';
import {NumberValidator} from '../../Common/Validators';
import FormGroupWithTooltip from './FormGroupWithTooltip';

class NumberFormGroupWithTooltip extends React.Component {
    validateValue = (val) => val
        ? (NumberValidator.validate(val)
            ? null
            : 'error')
        : null;

    render() {
        return <FormGroupWithTooltip controlId={this.props.controlId}
                                     label={this.props.label}
                                     validationState={this.validateValue(this.props.value)}
                                     overlay="только цифры"
                                     value={this.props.value}
                                     onChange={this.props.onChange}
                                     ph=""
                                     size={7}
                                     type="input" />;
    }
}

export default NumberFormGroupWithTooltip;