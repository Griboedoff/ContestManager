import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../store/Participants';

export default function WithParticipants(WrappedComponent) {
    return connect(
        state => ({ participants: state.participants.list, fetchingParticipants: state.participants.fetching }),
        dispatch => bindActionCreators(actionCreators, dispatch)
    )(WrappedComponent);
}
