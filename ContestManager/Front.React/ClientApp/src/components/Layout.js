import React from 'react';
import { Container } from 'reactstrap';
import { CenterSpinner } from './CenterSpinner';
import WithUser from './HOC/WithUser';
import NavMenu from './NavMenu';

class Layout extends React.Component {
    async componentDidMount() {
        if (!this.props.user)
            await this.props.setUserFromCookie();
    }

    render() {
        return this.props.fetching
            ? <CenterSpinner />
            : <div>
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
            </div>;
    }
}

export default WithUser(Layout);
