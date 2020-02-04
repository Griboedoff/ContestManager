import { get } from '../../Proxy';

export const FETCH_CONTEST_BEGIN = 'FETCH_CONTEST_BEGIN';
export const FETCH_CONTEST_SUCCESS = 'FETCH_CONTEST_SUCCESS';
export const FETCH_CONTESTS_SUCCESS = 'FETCH_CONTESTS_SUCCESS';
export const FETCH_CONTEST_FAILURE = 'FETCH_CONTEST_FAILURE';

export const actionCreators = {
    getContests: () => async dispatch => {
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
