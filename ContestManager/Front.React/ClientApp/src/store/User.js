import { get, post } from '../Proxy';

const fetching = 'FETCHING_USER';
const setUser = 'SET_USER';
const logout = 'LOGOUT';
const initialState = { user: null, fetching: false };

export const actionCreators = {
    setUser: user => dispatch => dispatch({ type: setUser, user }),
    setUserFromCookie: t => async dispatch => {
        dispatch({ type: fetching });
        await get('users/check').then(resp => {
            if (resp.ok) {
                return resp.json();
            }

            return null;
        }).then(user => {
            dispatch({ type: setUser, user });
        });
    },
    logout: t => async dispatch => {
        await post('users/logout');
        dispatch({ type: logout });
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === setUser) {
        return { ...state, user: action.user, fetching: false };
    }
    if (action.type === fetching) {
        return { ...state, fetching: true };
    }
    if (action.type === logout) {
        return { ...state, user: null };
    }

    return state;
};
