import * as React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../store/Contest';

export default function WithContests(WrappedComponent, formDataConfig) {
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

    return connect(
        state => state.contests,
        dispatch => bindActionCreators(actionCreators, dispatch)
    )(EnhancedComponent);
}
