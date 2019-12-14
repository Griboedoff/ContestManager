import { connectRouter } from "connected-react-router";
import { combineReducers } from 'redux';
import * as Contest from './Contest';
import * as News from './News';
import * as Participants from './Participants';
import * as User from './User';

export function createRootReducer(history) {
    const reducers = {
        user: User.reducer,
        contests: Contest.reducer,
        news: News.reducer,
        participants: Participants.reducer
    };

    return combineReducers({
        router: connectRouter(history),
        ...reducers
    });
}

