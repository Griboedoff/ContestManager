import React from 'react';
import { Container } from 'reactstrap';
import { Footer } from './Footer';
import WithUser from './HOC/WithUser';
import { NavMenu } from './NavMenu';

const Layout = ({children}) => {
    return <div className="d-flex flex-column sticky-footer-wrapper">
        <NavMenu />
        <Container className="flex-fill">
            {children}
        </Container>
        <Footer />
    </div>;
};

export default WithUser(Layout);
