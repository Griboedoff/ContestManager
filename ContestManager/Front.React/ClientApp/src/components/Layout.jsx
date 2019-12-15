import React from 'react';
import { Container } from 'reactstrap';
import { CenterSpinner } from './CenterSpinner';
import { Footer } from './Footer';
import WithUser from './HOC/WithUser';
import { NavMenu } from './NavMenu';

class Layout extends React.Component {
    async componentDidMount() {
        if (!this.props.user)
            await this.props.setUserFromCookie();
    }

    render() {
        return this.props.fetching
            ? <CenterSpinner />
            : <div className="d-flex flex-column sticky-footer-wrapper">
                <NavMenu />
                <Container className="flex-fill">
                    {this.props.children}
                </Container>
                <Footer />
            </div>;
    }
}

export default WithUser(Layout);
