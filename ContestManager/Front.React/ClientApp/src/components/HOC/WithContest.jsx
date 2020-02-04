import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { contestActionsCreators } from '../../store/Actions';

export default function WithContest(WrappedComponent) {
    return connect(
        state => ({ ...state.contests, fetchingContests: state.contests.fetching }),
        dispatch => bindActionCreators(contestActionsCreators, dispatch)
    )(WrappedComponent);
}
