import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { participantActionsCreators } from '../../store/Actions';

export default function WithParticipants(WrappedComponent) {
    return connect(
        state => ({ participants: state.participants.list, fetchingParticipants: state.participants.fetching }),
        dispatch => bindActionCreators(participantActionsCreators, dispatch)
    )(WrappedComponent);
}
