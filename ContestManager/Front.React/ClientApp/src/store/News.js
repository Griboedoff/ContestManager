const FETCH_NEWS_BEGIN = 'FETCH_NEWS_BEGIN';
const FETCH_NEWS_SUCCESS = 'FETCH_NEWS_SUCCESS';
const FETCH_NEWS_FAILURE = 'FETCH_NEWS_FAILURE';
const initialState = { news: {}, fetching: false };

export const actionCreators = {
    storeNews: (news, contestId) => dispatch => {
        dispatch({ type: FETCH_NEWS_SUCCESS, news, contestId });
    },
    fetchingNews: t => dispatch => dispatch({ type: FETCH_NEWS_BEGIN }),
    fetchingError: t => dispatch => dispatch({ type: FETCH_NEWS_FAILURE })
};

export const reducer = (state = initialState, action) => {
    switch (action.type) {
        case FETCH_NEWS_BEGIN:
            return { ...state, fetching: true };
        case FETCH_NEWS_SUCCESS:
            return { ...state, news: { ...state.news, [action.contestId]: action.news }, fetching: false };
        case FETCH_NEWS_FAILURE:
            return { ...state, fetching: false };
        default:
            return state;
    }
};
