import React from 'react';
import { Container, Row, Spinner } from 'reactstrap';

export const CenterSpinner = () => <Container>
    <Row>
        <Spinner className="m-auto flex-column" style={{ width: '3rem', height: '3rem' }} />
    </Row>
</Container>;