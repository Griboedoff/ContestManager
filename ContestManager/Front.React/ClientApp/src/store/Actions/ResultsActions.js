import { get } from '../../Proxy';

export const FETCH_RESULTS_BEGIN = 'FETCH_RESULTS_BEGIN';
export const FETCH_RESULTS_SUCCESS = 'FETCH_RESULTS_SUCCESS';
export const FETCH_RESULTS_FAILURE = 'FETCH_RESULTS_FAILURE';

export const actionCreators = {
    fetchResults: contestId => async dispatch => {
        dispatch({ type: FETCH_RESULTS_BEGIN });
        const resp = await get(`contests/${contestId}/results`);
        if (resp.ok) {
            const results = await resp.json();
            dispatch({ type: FETCH_RESULTS_SUCCESS, results });
        } else {
            dispatch({ type: FETCH_RESULTS_FAILURE });
        }
    }
};
