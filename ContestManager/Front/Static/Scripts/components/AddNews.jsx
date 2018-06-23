require('codemirror/lib/codemirror.css');
require('codemirror/theme/neat.css');
require('codemirror/mode/markdown/markdown');
import React from 'react';
import {Button, ControlLabel, Form, FormControl, FormGroup, Panel} from 'react-bootstrap';
import Markdown from 'react-markdown';
import Axios from 'axios';

class AddNews extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            markdownSrc: 'Пишите тут',
            contestId: '',
        };
    }

    componentDidMount() {
        this.setState({contestId: this.props.location.pathname.split('/')[2]});
    }

    handleMarkdownChange = (e) => {
        this.setState({markdownSrc: e.target.value});
    };

    addNews = () => {

        Axios.post("/news", {
            mdContent: this.state.markdownSrc,
            contestId: this.state.contestId,
        })
            .then(() => this.props.history.push(`/contests/${this.state.contestId}`));
    };

    render() {
        return [
            <Markdown source={this.state.markdownSrc}
                      skipHtml={this.state.htmlMode === 'skip'}
                      escapeHtml={this.state.htmlMode === 'escape'} />,
            <Form>
                <FormGroup controlId="formControlsTextarea">
                    <ControlLabel>Новость</ControlLabel>
                    <FormControl componentClass="textarea"
                                 placeholder="textarea"
                                 value={this.state.markdownSrc}
                                 onChange={this.handleMarkdownChange}
                                 style={{height: '447px'}} />
                </FormGroup>
            </Form>,
            <Button onClick={this.addNews}>Добавить</Button>
        ];
    }
}

export default AddNews;