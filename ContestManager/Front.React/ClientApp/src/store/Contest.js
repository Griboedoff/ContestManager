import { get } from '../Proxy';

const fetching = 'FETCHING_CONTESTS';
const getContests = 'GET_CONTESTS';
const initialState = { contests: [], fetching: false };

export const actionCreators = {
    getContests: t => async dispatch => {
        dispatch({ type: fetching });
        await get('contests').then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            return null;
        }).then(contests => {
            dispatch({ type: getContests, contests });
        });
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === getContests) {
        return { ...state, contests: action.contests, fetching: false };
    }
    if (action.type === fetching) {
        return { ...state, fetching: true };
    }

    return state;
};
