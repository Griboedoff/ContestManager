import { INVALIDATE_ERROR, SET_ERROR_MESSAGE } from '../Actions/ErrorActions';

const initialState = { message: null, show: false };

export default (state = initialState, action) => {
    switch (action.type) {
        case SET_ERROR_MESSAGE:
            return { ...state, show: true, message: action.message };
        case INVALIDATE_ERROR:
            return { ...state, show: false };
        default:
            return state;
    }
};
