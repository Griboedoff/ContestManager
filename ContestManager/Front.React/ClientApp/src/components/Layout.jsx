import React from 'react';
import { connect } from 'react-redux';
import { Alert, Container, Modal, ModalBody, ModalHeader } from 'reactstrap';
import { bindActionCreators } from 'redux';
import { errorActionsCreators } from '../store/Actions';
import { Footer } from './Footer';
import WithUser from './HOC/WithUser';
import { NavMenu } from './NavMenu';

const Layout = ({ children, showError, errorMessage, invalidateError }) => {
    return <div className="d-flex flex-column sticky-footer-wrapper">
        <NavMenu />
        <Container className="flex-fill">
            <Modal isOpen={showError} toggle={() => invalidateError()}>
                <ModalHeader toggle={() => invalidateError()}>Ошибка</ModalHeader>
                <ModalBody>
                    <Alert color="danger">{errorMessage}</Alert>
                </ModalBody>
            </Modal>
            {children}
        </Container>
        <Footer />
    </div>;
};
Layout.displayName = 'Layout';

const withUser = WithUser(Layout);
export default connect(
    state => ({ errorMessage: state.error.message, showError: state.error.show, canClose: state.error }),
    dispatch => bindActionCreators(errorActionsCreators, dispatch)
)(withUser);
