import React from 'react';
import FormGroupWithTooltip from './FormGroupWithTooltip';

class DateFormGroupWithTooltip extends React.Component {
    render() {
        return <FormGroupWithTooltip controlId={this.props.controlId}
                                     label={this.props.label}
                                     validationState={null}
                                     overlay={this.props.overlay}
                                     value={this.props.value}
                                     onChange={this.props.onChange}
                                     ph=""
                                     size={7}
                                     type="date" />;
    }
}

export default DateFormGroupWithTooltip;