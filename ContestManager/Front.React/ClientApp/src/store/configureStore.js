import { applyMiddleware, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { createBrowserHistory } from 'history';
import { routerMiddleware } from 'connected-react-router';
import { createRootReducer } from './Reducers/rootReducer';

export const history = createBrowserHistory();

export default function configureStore(history, initialState) {
    const middleware = [thunk, routerMiddleware(history)];

    // In development, use the browser's Redux dev tools extension if installed
    const enhancers = [];
    const isDevelopment = process.env.NODE_ENV === 'development';
    if (isDevelopment && typeof window !== 'undefined' && window.__REDUX_DEVTOOLS_EXTENSION__) {
        enhancers.push(window.__REDUX_DEVTOOLS_EXTENSION__());
    }

    return createStore(
        createRootReducer(history), // root reducer with router state
        initialState,
        compose(applyMiddleware(...middleware), ...enhancers),
    );
}
