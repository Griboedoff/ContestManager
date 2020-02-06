import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { resultsActionsCreators } from '../../store/Actions';

export default function WithResults(WrappedComponent) {
    return connect(
        state => ({ results: state.results.results, fetchingResults: state.results.fetching }),
        dispatch => bindActionCreators(resultsActionsCreators, dispatch)
    )(WrappedComponent);
}
