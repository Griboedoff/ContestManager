import {
    FETCH_CONTEST_BEGIN,
    FETCH_CONTEST_FAILURE,
    FETCH_CONTEST_SUCCESS,
    FETCH_CONTESTS_SUCCESS
} from '../Actions/ContestActions';

const initialState = { contests: [], contest: null, fetching: false };

export default (state = initialState, action) => {
    switch (action.type) {
        case FETCH_CONTEST_BEGIN:
            return { ...state, fetching: true, error: null };

        case FETCH_CONTEST_SUCCESS:
            return { ...state, fetching: false, contest: action.contest };
        case FETCH_CONTESTS_SUCCESS:
            return { ...state, fetching: false, contests: action.contests };

        case FETCH_CONTEST_FAILURE:
            return { ...state, fetching: false, error: action.error };

        default:
            return state;
    }
};
