import { isEmpty } from 'lodash';
import React from 'react';
import Textarea from 'react-textarea-autosize';
import { Alert, Button, Col, Container, Row, Table } from 'reactstrap';
import { post } from '../../../Proxy';

export class Seating extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            auditoriums: '',
            auditoriumsData: {},

            message: '',
            isError: false,
        };
    }

    handleChange = event => {
        const { target } = event;
        const { name } = target;
        this.setState({
            [name]: target.value,
            auditoriumsData: this.parseAuditoriums(target.value),
        });
    };

    generate = _ => {
        this.setState({
            message: '',
            isError: false
        });

        post(`contestAdmin/${this.props.contestId}/generateSeating`, this.state.auditoriumsData).then(resp => {
            if (resp.ok)
                return resp;

            throw Error();
        }).then(_ => this.setState({
            message: 'Готово'
        })).catch(_ => this.setState({
            message: 'Произошла ошибка',
            error: true
        }));
    };

    render() {
        return <Container>
            <Row>
                <h4 className="mt-3">Сгенерировать рассадку</h4>

                <Alert color="secondary">Запиши данные про аудитории.<br />
                    По одной в строке, название, число мест и код из трех символов через <code>|</code>. <br />
                    Например <code>Демидовский зал|200|dmz</code> <br />
                    Если не указать код, туда подставится название.</Alert>

                {this.state.message &&
                <Alert color={this.state.error ? "success" : "danger"}>{this.state.message}</Alert>}
            </Row>
            <Row>
                <Col sm={6}>
                    <Textarea className="form-control mb-3" onChange={this.handleChange} name="auditoriums" />
                    <Button onClick={this.generate}>Сохранить и сгенерировать рассадку</Button>
                </Col>
                <Col sm={6}>
                    {!isEmpty(this.state.auditoriumsData) && <Table>
                        <thead>
                        <tr>
                            <th>Название</th>
                            <th>Код</th>
                            <th>Число мест</th>
                        </tr>
                        </thead>
                        <tbody>
                        {this.state.auditoriumsData.map(a => <tr>
                            <td>{a.name}</td>
                            <td>{a.code}</td>
                            <td>{a.capacity}</td>
                        </tr>)}
                        </tbody>
                    </Table>}
                </Col>
            </Row>
        </Container>;
    }

    parseAuditoriums = (text) => {
        return text.split('\n')
            .filter(s => !!s)
            .map(s => {
                const v = s.split('|');

                return { name: v[0], capacity: v[1], code: (v[2] ? v[2] : v[0]).substr(0, 3) };
            });
    };
}

