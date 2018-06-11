import React from 'react';
import {Button, Glyphicon} from 'react-bootstrap';

const typeToStr = {
    "String": "Строка",
    "Number": "Число",
    "Date": "Дата",
};

class ContestField extends React.Component {
    render() {
        return (
            <tr>
                <td>{this.props.field.title}</td>
                <td>{typeToStr[this.props.field.fieldType]}</td>
                <td>
                    <Button onClick={() => this.props.removeItem(this.props.field)}>
                        <Glyphicon glyph="trash" />
                    </Button>
                </td>
            </tr>
        );
    };
}


export default ContestField;