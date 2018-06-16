require('codemirror/lib/codemirror.css');
require('codemirror/theme/neat.css');
require('codemirror/mode/markdown/markdown');
import React from 'react';
import {Button, Panel} from 'react-bootstrap';
import {UnControlled as CodeMirror} from 'react-codemirror2';
import Markdown from 'react-markdown';
import Axios from 'axios';

class ContestNews extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            markdownSrc: 'asdasd',
            htmlMode: 'raw',
        };
    }

    handleMarkdownChange = (e, d, v) => {
        this.setState({markdownSrc: v});
    };

    addNews = () => {
        Axios.post("/news", {
            news: this.state.markdownSrc,
            contestId: this.props.constet.Id,
        });
    };

    render() {
        const options = {
            mode: "markdown",
            // readOnly: false,
        };

        return [
            <Markdown source={this.state.markdownSrc}
                      skipHtml={this.state.htmlMode === 'skip'}
                      escapeHtml={this.state.htmlMode === 'escape'} />,
            <Panel key="p">
                <Panel.Body>
                    <CodeMirror
                        value={this.state.markdownSrc}
                        options={options}
                        onChange={this.handleMarkdownChange}
                    />
                </Panel.Body>
            </Panel>,
            <Button onClick={this.addNews}>Добавить</Button>
        ];
    }
}

export default ContestNews;