export const FETCH_NEWS_BEGIN = 'FETCH_NEWS_BEGIN';
export const FETCH_NEWS_SUCCESS = 'FETCH_NEWS_SUCCESS';
export const FETCH_NEWS_FAILURE = 'FETCH_NEWS_FAILURE';

export const actionCreators = {
    storeNews: (news, contestId) => dispatch => {
        dispatch({ type: FETCH_NEWS_SUCCESS, news, contestId });
    },
    startFetchingNews: t => dispatch => dispatch({ type: FETCH_NEWS_BEGIN }),
    fetchingError: t => dispatch => dispatch({ type: FETCH_NEWS_FAILURE })
};
