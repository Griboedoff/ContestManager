import { FETCH_RESULTS_BEGIN, FETCH_RESULTS_FAILURE, FETCH_RESULTS_SUCCESS } from '../Actions/ResultsActions';

const initialState = { results: null, fetching: false };

export default (state = initialState, action) => {
    switch (action.type) {
        case FETCH_RESULTS_BEGIN:
            return { ...state, fetching: true };
        case FETCH_RESULTS_SUCCESS:
            return { ...state, results: action.results, fetching: false };
        case FETCH_RESULTS_FAILURE:
            return { ...state, fetching: false };
        default:
            return state;
    }
};
