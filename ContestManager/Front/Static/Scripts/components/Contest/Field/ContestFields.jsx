import React from 'react';
import {Table} from 'react-bootstrap';
import ContestField from './ContestField';
import ContestFieldHeader from './ContestFieldHeader';

class ContestCreate extends React.Component {
    render() {
        return <Table striped condensed hover>
                <ContestFieldHeader />
                <tbody>
                {this.props.fields.map(f => <ContestField key={f.item} field={f} removeItem={this.props.removeItem} />)}
                </tbody>
            </Table>;
    };
}

export default ContestCreate;