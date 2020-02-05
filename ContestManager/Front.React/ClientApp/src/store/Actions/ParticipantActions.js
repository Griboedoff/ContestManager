import { get } from '../../Proxy';

export const FETCH_PARTICIPANTS_BEGIN = 'FETCH_PARTICIPANTS_BEGIN';
export const FETCH_PARTICIPANTS_SUCCESS = 'FETCH_PARTICIPANTS_SUCCESS';
export const FETCH_PARTICIPANTS_FAILURE = 'FETCH_PARTICIPANTS_FAILURE';
export const UPDATE_PARTICIPANT = 'UPDATE_PARTICIPANT';

export const actionCreators = {
    updateParticipant: (participant) => dispatch => {
        dispatch({ type: UPDATE_PARTICIPANT, participant });
    },
    fetchParticipants: contestId => async dispatch => {
        dispatch({ type: FETCH_PARTICIPANTS_BEGIN });
        const resp = await get(`contests/${contestId}/participants`);
        if (resp.ok) {
            const participants = await resp.json();
            dispatch({ type: FETCH_PARTICIPANTS_SUCCESS, participants });

        } else {
            dispatch({ type: FETCH_PARTICIPANTS_FAILURE });
        }
    }
};
