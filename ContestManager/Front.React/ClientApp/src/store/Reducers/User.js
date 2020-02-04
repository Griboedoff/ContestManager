import { FETCHING_USER, LOGOUT, NO_USER, SET_USER } from '../Actions/UserActions';

const initialState = { user: null, fetching: false, checked: false };

export default (state = initialState, action) => {
    switch (action.type) {
        case SET_USER:
            return { ...state, user: action.user, fetching: false, checked: true };
        case FETCHING_USER:
            return { ...state, fetching: true };
        case NO_USER:
            return { ...state, fetching: false, checked: true };
        case LOGOUT:
            return { ...state, user: null };
        default:
            return state;
    }
};
