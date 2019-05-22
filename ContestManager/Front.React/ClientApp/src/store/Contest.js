import { get } from '../Proxy';

const FETCH_CONTEST_BEGIN = 'FETCH_CONTEST_BEGIN';
const FETCH_CONTEST_SUCCESS = 'FETCH_CONTEST_SUCCESS';
const FETCH_CONTESTS_SUCCESS = 'FETCH_CONTESTS_SUCCESS';
const FETCH_CONTEST_FAILURE = 'FETCH_CONTEST_FAILURE';

const initialState = { contests: [], contest: null, fetching: false };

export const actionCreators = {
    getContests: t => async dispatch => {
        dispatch({ type: FETCH_CONTEST_BEGIN });
        await get('contests').then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            return null;
        }).then(contests => {
            dispatch({ type: FETCH_CONTESTS_SUCCESS, contests });
        });
    },
    getContest: id => async dispatch => {
        dispatch({ type: FETCH_CONTEST_BEGIN });
        await get(`contests/${id}`).then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            return null;
        }).then(contest => {
            dispatch({ type: FETCH_CONTEST_SUCCESS, contest });
        });
    },
    setContestFromStore: contest => dispatch => dispatch({ type: FETCH_CONTEST_SUCCESS, contest })
};

export const reducer = (state = initialState, action) => {
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
