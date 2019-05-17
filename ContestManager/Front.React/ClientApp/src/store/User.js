import { get } from '../Proxy';

const setUser = 'SET_USER';
const logout = 'LOGOUT';
const initialState = { user: null };

export const actionCreators = {
    setUser: user => dispatch => dispatch({ type: setUser, user }),
    setUserFromCookie: t => dispatch =>
        get('users/check').then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            return null;
        }).then(user => {
            dispatch({ type: setUser, user });
        }),
    logout: dispatch => dispatch({ type: logout })
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === setUser) {
        return { ...state, user: action.user };
    }

    if (action.type === logout) {
        return { ...state, user: null };
    }

    return state;
};
