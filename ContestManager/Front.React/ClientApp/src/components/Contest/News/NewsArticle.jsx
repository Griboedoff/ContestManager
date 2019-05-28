import moment from 'moment';
import React from 'react';
import Markdown from 'react-markdown';
import './index.css';

export default function NewsArticle({ article }) {
    return <>
        <div className="d-flex align-items-baseline justify-content-between mb-4">
            <h2 className="mb-0">{article.title}</h2>
            {article.creationDate && <div className="time">{moment(article.creationDate).format('DD-MM-YYYY HH:mm')}</div>}
        </div>
        <Markdown source={article.content} />
    </>;
}