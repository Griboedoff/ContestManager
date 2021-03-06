import * as React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { contestActionsCreators } from '../../store/Actions';
import { getDisplayName } from '../../utils';

export default function WithContests(WrappedComponent) {
    class EnhancedComponent extends React.Component {
        componentDidMount() {
            this.props.getContests();
        }

        render() {
            if (this.props.fetching || !this.props.contests) {
                return null;
            }

            return <WrappedComponent {...this.props} />;
        }
    }
    EnhancedComponent.displayName = `WithContests(${getDisplayName(WrappedComponent)})`;

    return connect(
        state => state.contests,
        dispatch => bindActionCreators(contestActionsCreators, dispatch)
    )(EnhancedComponent);
}
