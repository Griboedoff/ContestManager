import { get, post } from '../../Proxy';

export const FETCHING_USER = 'FETCHING_USER';
export const NO_USER = 'NO_USER';
export const SET_USER = 'SET_USER';
export const LOGOUT = 'LOGOUT';

export const actionCreators = {
    setUser: user => dispatch => dispatch({ type: SET_USER, user }),
    setUserFromCookie: t => async dispatch => {
        dispatch({ type: FETCHING_USER });
        const resp = await get('users/check');
        if (resp.ok) {
            const user = await resp.json();
            dispatch({ type: SET_USER, user });
        } else
            dispatch({ type: NO_USER });
    },
    logout: t => async dispatch => {
        await post('users/logout');
        dispatch({ type: LOGOUT });
    }
};
