import React from 'react';
import { Alert, Button, Col, Container, Form, FormGroup, Input, Label, Row, Spinner } from 'reactstrap';
import Textarea from 'react-textarea-autosize';
import { post } from '../../../Proxy';
import NewsArticle from './NewsArticle';

export default class AddNews extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            content: '',
            title: '',
            fetching: false,
            message: '',
            isError: false,
        };

        this.createNews = this.createNews.bind(this);
    }

    handleChange = async event => {
        const { target } = event;
        const { name } = target;
        await this.setState({
            [name]: target.value,
        });
    };

    async createNews() {
        this.setState({
            fetching: true,
            message: '',
            isError: false
        });

        await post('news', {
            content: this.state.content,
            title: this.state.title,
            contestId: this.props.contestId
        }).then(resp => {
            if (resp.ok)
                return resp;

            throw Error();
        }).then(_ => this.setState({
            message: 'Новость добавлена'
        })).catch(_ => this.setState({
            message: 'Произошла ошибка',
            error: true
        })).finally(() => this.setState({
            fetching: false
        }));
    }

    render() {
        return <Container>
            <Row>
                <Col>
                    <h4 className="mt-3">Добавить новость</h4>

                    <Alert color="light">
                        Новости пишутся на языке разметки markdown. <br />
                        <a target="_blank"
                           rel="noopener noreferrer"
                           href="https://rukeba.com/by-the-way/markdown-sintaksis-po-russki/">Синтаксис</a>
                    </Alert>
                    {this.state.error && <Alert color="danger">{this.state.message}</Alert>}
                    {!this.state.error && this.state.message && <Alert color="success">{this.state.message}</Alert>}
                </Col>
            </Row>
            <Row>
                <Col sm={6}>
                    <Form>
                        <FormGroup>
                            <Input type="text" name="title" onChange={this.handleChange} placeholder="Заголовок" />
                        </FormGroup>
                        <hr />
                        <FormGroup>
                            <Textarea className="form-control"
                                      minRows={5}
                                      maxRows={50}
                                      name="content"
                                      onChange={this.handleChange}
                                      placeholder="Содержание" />
                        </FormGroup>

                        <FormGroup>
                            <Button onClick={this.createNews}>
                                {this.props.fetching && <Spinner color="secondary" />}
                                Создать</Button>
                        </FormGroup>
                    </Form>
                </Col>
                <Col sm={6}>
                    <NewsArticle article={this.state} />
                </Col>
            </Row>
        </Container>;
    }
}