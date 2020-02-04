export const FETCH_PARTICIPANTS_BEGIN = 'FETCH_PARTICIPANTS_BEGIN';
export const FETCH_PARTICIPANTS_SUCCESS = 'FETCH_PARTICIPANTS_SUCCESS';
export const FETCH_PARTICIPANTS_FAILURE = 'FETCH_PARTICIPANTS_FAILURE';
export const UPDATE_PARTICIPANT = 'UPDATE_PARTICIPANT';

export const actionCreators = {
    storeParticipants: (participants) => dispatch => {
        dispatch({ type: FETCH_PARTICIPANTS_SUCCESS, participants });
    },
    updateParticipant: (participant) => dispatch => {
        dispatch({ type: UPDATE_PARTICIPANT, participant });
    },
    startFetchingParticipants: t => dispatch => dispatch({ type: FETCH_PARTICIPANTS_BEGIN }),
    fetchingError: t => dispatch => dispatch({ type: FETCH_PARTICIPANTS_FAILURE })
};
