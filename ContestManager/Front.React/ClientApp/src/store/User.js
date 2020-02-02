import { get, post } from '../Proxy';

const fetching = 'FETCHING_USER';
const noUser = 'NO_USER';
const setUser = 'SET_USER';
const logout = 'LOGOUT';
const initialState = { user: null, fetching: false, checked: false };

export const actionCreators = {
    setUser: user => dispatch => dispatch({ type: setUser, user }),
    setUserFromCookie: t => async dispatch => {
        dispatch({ type: fetching });
        const resp = await get('users/check');
        if (resp.ok) {
            const user = await resp.json();
            dispatch({ type: setUser, user });
        } else
            dispatch({ type: noUser });
    },
    logout: t => async dispatch => {
        await post('users/logout');
        dispatch({ type: logout });
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === setUser) {
        return { ...state, user: action.user, fetching: false, checked: true };
    }
    if (action.type === fetching) {
        return { ...state, fetching: true };
    }
    if (action.type === noUser) {
        return { ...state, fetching: false, checked: true };
    }
    if (action.type === logout) {
        return { ...state, user: null };
    }

    return state;
};
