const setUser = 'SET_USER';
const logout = 'LOGOUT';
const initialState = { user: null };

export const actionCreators = {
    setUser: user => (dispatch, getState) => {
        dispatch({ type: setUser, user });
    },
    logout: (dispatch, getState) => {
        dispatch({ type: logout });
    }
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
