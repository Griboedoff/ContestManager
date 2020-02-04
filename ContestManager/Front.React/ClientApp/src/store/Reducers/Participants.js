import {
    FETCH_PARTICIPANTS_BEGIN,
    FETCH_PARTICIPANTS_FAILURE,
    FETCH_PARTICIPANTS_SUCCESS,
    UPDATE_PARTICIPANT
} from '../Actions/ParticipantActions';

const initialState = { list: [], fetching: false };

export default (state = initialState, action) => {
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
