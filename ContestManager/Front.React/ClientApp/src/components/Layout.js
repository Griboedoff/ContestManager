import React from 'react';
import { Container } from 'reactstrap';
import Spinner from 'reactstrap/es/Spinner';
import withUser from './HOC/WithUser';
import NavMenu from './NavMenu';

class Layout extends React.Component {
    async componentDidMount() {
        if (!this.props.user)
            await this.props.setUserFromCookie();
    }

    render() {
        return this.props.fetching
            ? <Spinner style={{ width: '3rem', height: '3rem' }} />
            : <div>
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
            </div>;
    }
}

export default withUser(Layout);
