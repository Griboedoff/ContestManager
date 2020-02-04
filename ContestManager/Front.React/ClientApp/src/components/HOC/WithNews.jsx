import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { newsActionsCreators } from '../../store/Actions';

export default function WithNews(WrappedComponent) {
    return connect(
        state => ({ news: state.news.news, fetchingNews: state.news.fetching }),
        dispatch => bindActionCreators(newsActionsCreators, dispatch)
    )(WrappedComponent);
}
