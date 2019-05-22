import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../store/News';

export default function WithNews(WrappedComponent) {
    return connect(
        state => ({news: state.news.news}),
        dispatch => bindActionCreators(actionCreators, dispatch)
    )(WrappedComponent);
}
