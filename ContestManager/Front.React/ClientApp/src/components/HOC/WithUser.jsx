import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../store/User';
import { getDisplayName } from '../../utils';
import { CenterSpinner } from '../CenterSpinner';
import React from 'react';

export default function WithUser(WrappedComponent) {
    class EnhancedComponent extends React.Component {
        displayName = `WithUser(${getDisplayName(WrappedComponent)})`;

        async componentDidMount() {
            if (!this.props.user)
                await this.props.setUserFromCookie();
        }

        render() {
            if (this.props.fetching)
                return <CenterSpinner />;

            return <WrappedComponent {...this.props} />;
        }
    }

    return connect(
        state => ({ user: state.user.user, fetchingUser: state.user.fetching }),
        dispatch => bindActionCreators(actionCreators, dispatch)
    )(EnhancedComponent);
}
