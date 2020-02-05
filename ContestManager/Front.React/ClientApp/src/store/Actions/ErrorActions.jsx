import React from 'react';
import { Link } from 'react-router-dom';

export const SET_ERROR_MESSAGE = 'SET_ERROR_MESSAGE';
export const INVALIDATE_ERROR = 'INVALIDATE_ERROR';

export const actionCreators = {
    setError: message => dispatch => {
        dispatch({ type: SET_ERROR_MESSAGE, message });
    },
    setRequestError: (response, message = 'Произошла ошибка при выполнении запроса') => dispatch => {
        console.log(response);

        if (response.status === 401) {
            dispatch({
                type: SET_ERROR_MESSAGE,
                message: <>Вы не авторизованы.{' '}
                    <Link to="/login" onClick={() => dispatch({ type: INVALIDATE_ERROR })}>Войдите</Link> на сайт
                </>
            });
        } else {
            dispatch({ type: SET_ERROR_MESSAGE, message });
        }
    },
    invalidateError: _ => dispatch => {
        dispatch({ type: INVALIDATE_ERROR });
    }
};
