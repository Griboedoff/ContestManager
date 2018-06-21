import React from 'react';
import {ValidSymbolValidator} from '../../Common/Validators';
import FormGroupWithTooltip from './FormGroupWithTooltip';

class StringFormGroupWithTooltip extends React.Component {
    validateValue = (val) => val
        ? (ValidSymbolValidator.validate(val)
            ? null
            : 'error')
        : null;

    render() {
        return <FormGroupWithTooltip controlId={this.props.controlId}
                                     label={this.props.label}
                                     validationState={this.validateValue(this.props.value)}
                                     overlay="русский алфавит, цифры и пунктуация"
                                     value={this.props.value}
                                     onChange={this.props.onChange}
                                     ph=""
                                     size={7}
                                     type="input" />;
    }
}

export default StringFormGroupWithTooltip;