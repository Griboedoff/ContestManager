import { get, post } from '../../Proxy';

export const FETCHING_USER = 'FETCHING_USER';
export const NO_USER = 'NO_USER';
export const SET_USER = 'SET_USER';
export const LOGOUT = 'LOGOUT';

const setUser = user => dispatch => dispatch({ type: SET_USER, user });

const needCheckSession = state => !state.checked && !state.fetching;

const checkUserSession = () => async (dispatch, getState) => {
    if (needCheckSession(getState().user)) {
        dispatch({ type: FETCHING_USER });
        const resp = await get('users/check');
        if (resp.ok) {
            const user = await resp.json();
            dispatch({ type: SET_USER, user });
        } else
            dispatch({ type: NO_USER });
    }
};

const logout = () => async dispatch => {
    await post('users/logout');
    dispatch({ type: LOGOUT });
};

export const actionCreators = {
    setUser,
    checkUserSession,
    logout
};
