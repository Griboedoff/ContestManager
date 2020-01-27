import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../store/Contest';

export default function WithContest(WrappedComponent) {
    return connect(
        state => ({ ...state.contests, fetchingContests: state.contests.fetching }),
        dispatch => bindActionCreators(actionCreators, dispatch)
    )(WrappedComponent);
}
