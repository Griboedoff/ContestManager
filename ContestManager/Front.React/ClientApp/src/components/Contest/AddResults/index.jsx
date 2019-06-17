import React from 'react';
import Textarea from 'react-textarea-autosize';
import { Alert, Button, Container, Row } from 'reactstrap';
import { get, post } from '../../../Proxy';

export default class AddResult extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            auditoriums: '',
            tasksData: {},

            tableLink: '',
            message: '',
            isError: false,
        };
    }

    componentDidMount() {
        get(`contests/${this.props.contestId}/resultsTable`).then(resp => {
            if (resp.ok)
                return resp.json();
        }).then(link => this.setState({
            tableLink: 'link'
        }));
    }

    handleChange = event => {
        const { target } = event;
        const { name } = target;
        this.setState({
            [name]: target.value,
            tasksData: this.parseClasses(target.value),
        });
    };

    generate = _ => {
        this.setState({
            message: '',
            isError: false
        });

        post(`contests/${this.props.contestId}/resultsTable`, this.state.tasksData).then(resp => {
            if (resp.ok)
                return resp.json();

            throw Error();
        }).then(tableLink => this.setState({
            tableLink
        })).catch(_ => this.setState({
            message: 'Произошла ошибка',
            error: true
        }));
    };

    render() {
        return <Container>
            {!this.state.tableLink && <>
                <Row>
                    <h4 className="mt-3">Сгенерировать таблицу для результатов</h4>

                    <Alert color="secondary">Запиши данные про задания.<br />
                        По одному классу строке, номер класса, двоеточие и список заданий через запятую.<br />
                        Если у задачки есть подзадания, указывай также через запятую.<br />
                        Например <code>10: 1, 2, 3, 4, 5.а, 5.б, 5.в, 5.г</code> <br />

                        После сохранения описания, создастся таблица на Гугл.Диске, в которую загрузится информация о
                        заданиях и шифры участников
                    </Alert>

                    {this.state.message &&
                    <Alert color={this.state.error ? "success" : "danger"}>{this.state.message}</Alert>}
                </Row>
                <Row>
                    <Textarea className="form-control mb-3" onChange={this.handleChange} name="auditoriums" />
                    <Button onClick={this.generate}>Создать табличку</Button>
                </Row>
            </>}
            {this.state.tableLink && <><Row className="mb-3">
                <a href={this.state.tableLink}>Таблица с разультатами</a>
            </Row>
                <Row>
                    <Button onClick={this.generate}>Синхронизировать данные</Button>
                </Row>
            </>}
        </Container>;
        ;
    }

    parseClasses = text => text.split('\n')
        .filter(s => !!s)
        .map(s => {
            const v = s.split(':');

            return { class: v[0], tasks: v[1], };
        });
}

