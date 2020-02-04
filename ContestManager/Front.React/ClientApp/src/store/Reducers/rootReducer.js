import { connectRouter } from "connected-react-router";
import { combineReducers } from 'redux';
import contests from './Contest';
import news from './News';
import participants from './Participants';
import user from './User';

export function createRootReducer(history) {
    const reducers = {
        user,
        contests,
        news,
        participants
    };

    return combineReducers({
        router: connectRouter(history),
        ...reducers
    });
}

