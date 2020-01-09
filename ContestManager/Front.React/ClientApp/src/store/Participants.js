const FETCH_PARTICIPANTS_BEGIN = 'FETCH_PARTICIPANTS_BEGIN';
const FETCH_PARTICIPANTS_SUCCESS = 'FETCH_PARTICIPANTS_SUCCESS';
const FETCH_PARTICIPANTS_FAILURE = 'FETCH_PARTICIPANTS_FAILURE';
const UPDATE_PARTICIPANT = 'UPDATE_PARTICIPANT';
const initialState = { list: [], fetching: false };

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

export const reducer = (state = initialState, action) => {
    switch (action.type) {
        case FETCH_PARTICIPANTS_BEGIN:
            return { ...state, fetching: true };
        case FETCH_PARTICIPANTS_SUCCESS:
            return { ...state, list: action.participants, fetching: false };
        case FETCH_PARTICIPANTS_FAILURE:
            return { ...state, fetching: false };
        case UPDATE_PARTICIPANT:
            let list = [...state.list];
            for (let i = 0; i < list.length; ++i)
                if (list[i].id === action.participant.id) {
                    list[i] = { ...list[i], ...action.participant };
                }
            return { ...state, list };
        default:
            return state;
    }
};
