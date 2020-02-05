import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { errorActionsCreators } from '../../store/Actions';

export const WithSetError = WrappedComponent => connect(
    null,
    dispatch => bindActionCreators(errorActionsCreators, dispatch)
)(WrappedComponent);

