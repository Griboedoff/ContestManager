import { FETCH_NEWS_BEGIN, FETCH_NEWS_FAILURE, FETCH_NEWS_SUCCESS } from './Actions/NewsActions';

const initialState = { news: {}, fetching: false };

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
