import React from 'react';
import { Container } from 'reactstrap';
import withUser from './HOC/WithUser';
import NavMenu from './NavMenu';

class Layout extends React.Component {
    componentDidMount() {
        this.props.setUserFromCookie();
    }

    render() {
        return <div>
            <NavMenu />
            <Container>
                {this.props.children}
            </Container>
        </div>;
    }
}

export default withUser(Layout);
