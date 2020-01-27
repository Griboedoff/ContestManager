import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../store/User';
import { getDisplayName } from '../../utils';
import { CenterSpinner } from '../CenterSpinner';
import React from 'react';

const getCookie = (name) => {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
};

export default function WithUser(WrappedComponent) {
    class EnhancedComponent extends React.Component {
        displayName = `WithUser(${getDisplayName(WrappedComponent)})`;

        async componentDidMount() {
            if (!this.props.user && getCookie('sid') && getCookie('User'))
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
