require('codemirror/lib/codemirror.css');
require('codemirror/theme/neat.css');
require('codemirror/mode/markdown/markdown');
import React from 'react';
import {Button, Panel} from 'react-bootstrap';
import {UnControlled as CodeMirror} from 'react-codemirror2';
import Markdown from 'react-markdown';
import Axios from 'axios';

class AddNews extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            markdownSrc: 'asdasd',
            htmlMode: 'raw',
            contestId: '',
        };
    }

    componentDidMount() {
        this.setState({contestId: this.props.location.pathname.split('/')[2]});
    }

    handleMarkdownChange = (e, d, v) => {
        this.setState({markdownSrc: v});
    };

    addNews = () => {

        Axios.post("/news", {
            mdContent: this.state.markdownSrc,
            contestId: this.state.contestId,
        })
            .then(() => this.props.history.push(`/contests/${this.state.contestId}`));
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

export default AddNews;