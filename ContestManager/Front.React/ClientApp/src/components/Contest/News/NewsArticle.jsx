import moment from 'moment';
import React, { useState } from 'react';
import Markdown from 'react-markdown';
import './index.css';
import { UserRole } from '../../../Enums/UserRole';
import WithUser from '../../HOC/WithUser';
import EditNews from './EditNews';

const NewsArticle = ({ article, user, editable }) => {
    const isAdmin = user && user.role === UserRole.Admin;
    const [edit, setEdit] = useState(false);
    return edit ?
        (<EditNews newsToUpdate={article} onClose={() => setEdit(false)}/>) :
        (<>
                <div className="d-flex align-items-baseline justify-content-between mb-4">
                    <h2 className="mb-0">{article.title}</h2>
                    <div>
                        {article.creationDate &&
                        <div className="time">{moment(article.creationDate).format('DD-MM-YYYY HH:mm')}</div>}
                        {editable && isAdmin && <div className="link" onClick={() => setEdit(true)}>редактировать</div>}
                    </div>
                </div>
                <Markdown source={article.content} />
            </>
        );
};

export default WithUser(NewsArticle);
