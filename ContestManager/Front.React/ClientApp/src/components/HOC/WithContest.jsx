import * as React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../store/Contest';

export default function WithContest(WrappedComponent) {
    class EnhancedComponent extends React.Component {
        render() {
            return <WrappedComponent {...this.props} />;
        }
    }

    return connect(
        state => ({ ...state.contests, fetchingContests: state.contests.fetching }),
        dispatch => bindActionCreators(actionCreators, dispatch)
    )(EnhancedComponent);
}
