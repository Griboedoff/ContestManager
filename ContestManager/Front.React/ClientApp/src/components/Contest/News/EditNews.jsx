import React from 'react';
import Textarea from 'react-textarea-autosize';
import { Alert, Button, Col, Container, Form, FormGroup, Input, Row, Spinner } from 'reactstrap';
import { patch, post } from '../../../Proxy';
import NewsArticle from './NewsArticle';

export default class EditNews extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            content: '',
            title: '',
            fetching: false,
            message: '',
            isError: false,

            ...this.props.newsToUpdate
        };

        this.createNews = this.createNews.bind(this);
        this.updateNews = this.updateNews.bind(this);
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

    async updateNews() {
        this.setState({
            fetching: true,
            message: '',
            isError: false
        });

        const news = {
            content: this.state.content,
            title: this.state.title
        };
        await patch(`news/${this.props.newsToUpdate.id}`, {
            ...news,
            contestId: this.props.contestId
        }).then(resp => {
            if (resp.ok)
                return resp;

            throw Error();
        }).then(_ => this.setState({
            message: 'Новость обновлена '
        })).then(_ => {
                document.location.reload();
                this.props.onClose();
            }
        ).catch(_ => this.setState({
            message: 'Произошла ошибка',
            error: true
        })).finally(() => this.setState({
            fetching: false
        }));
    }

    render() {
        let isUpdate = this.props.newsToUpdate;
        const { error, title, content, message } = this.state;
        return <Container>
            <Row>
                <Col>
                    {!isUpdate && <h4 className="mt-3">Добавить новость</h4>}

                    <Alert color="light">
                        Новости пишутся на языке разметки markdown. <br />
                        <a target="_blank"
                           rel="noopener noreferrer"
                           href="https://rukeba.com/by-the-way/markdown-sintaksis-po-russki/">Синтаксис</a>
                    </Alert>
                    {error && <Alert color="danger">{message}</Alert>}
                    {!error && message && <Alert color="success">{message}</Alert>}
                </Col>
            </Row>
            <Row>
                <Col sm={6}>
                    <Form>
                        <FormGroup>
                            <Input type="text"
                                   name="title"
                                   onChange={this.handleChange}
                                   placeholder="Заголовок"
                                   value={title} />
                        </FormGroup>
                        <hr />
                        <FormGroup>
                            <Textarea className="form-control"
                                      minRows={5}
                                      maxRows={50}
                                      name="content"
                                      onChange={this.handleChange}
                                      placeholder="Содержание"
                                      value={content} />
                        </FormGroup>

                        <FormGroup>
                            <Button onClick={isUpdate ? this.updateNews : this.createNews}>
                                {this.props.fetching && <Spinner color="secondary" />}
                                Добавить</Button>
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
