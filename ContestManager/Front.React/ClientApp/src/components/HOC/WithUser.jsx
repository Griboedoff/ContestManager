import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { userActionsCreators } from '../../store/Actions';
import { getDisplayName } from '../../utils';
import { CenterSpinner } from '../CenterSpinner';
import React from 'react';

export default function WithUser(WrappedComponent) {
    class EnhancedComponent extends React.Component {
        displayName = `WithUser(${getDisplayName(WrappedComponent)})`;

        async componentDidMount() {
            await this.props.checkUserSession();
        }

        render() {
            if (this.props.fetching)
                return <CenterSpinner />;

            return <WrappedComponent {...this.props} />;
        }
    }

    return connect(
        state => ({ user: state.user.user, fetchingUser: state.user.fetching, userChecked: state.user.checked }),
        dispatch => bindActionCreators(userActionsCreators, dispatch)
    )(EnhancedComponent);
}
