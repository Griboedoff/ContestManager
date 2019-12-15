import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Button, Container, Jumbotron, ListGroup, ListGroupItem, Spinner } from 'reactstrap';
import { ContestOptions } from '../../Enums/ContestOptions';
import { getFromStorage, setToStorage } from '../../utils';
import WithContests from '../HOC/WithContests';

const jumboClosedKey = 'JumboClosed';

const ContestList = ({ fetching, contests }) => {
    const [jumboClosed, setJumboClosed] = useState(getFromStorage(jumboClosedKey));

    if (fetching) {
        return <Spinner style={{ width: '3rem', height: '3rem' }} />;
    }

    const ongoing = contests.filter(c => c.options !== ContestOptions.Finished);
    const finished = contests.filter(c => c.options === ContestOptions.Finished);

    const onClick = () => {
        setToStorage(jumboClosedKey, true);
        setJumboClosed(true);
    };

    const toListItem = c => (
        <ListGroupItem tag={Link} to={`contests/${c.id}`} id={c.id} key={c.id}>
            {c.title}
        </ListGroupItem>);
    return <>
        {!jumboClosed && <Jumbotron fluid>
            <Container>
                <h1 className="display-3">Обновление</h1>
                <p className="lead">
                    Мы обновили сайт. <br />
                    Все пользователи, кто был зарегистрирован с помощью ВКонтакте, могут входить как раньше. Те, кто
                    использовал email, должны зарегистрироваться еще раз. <br />
                    Скоро сюда перенесем результаты прошлых олимпиад, а пока мы открыли регистрацию на олимпиады в
                    2020
                    году.<br />
                    По всем вопросам пишите на адрес <a href="mailto:vuzak@acm.urfu.ru">vuzak@acm.urfu.ru</a><br />
                </p>
                <p className="lead">
                    <Button color="primary" onClick={onClick}>Все понятно</Button>
                </p>
            </Container>
        </Jumbotron>}
        {ongoing.length !== 0 && <>
            <h3>Текущие</h3>
            <ListGroup className="mb-3">{ongoing.map(toListItem)}</ListGroup>
        </>}
        {finished.length !== 0 && <>
            <h3>Прошедшие</h3>
            <ListGroup>{finished.map(toListItem)}</ListGroup>
        </>}
    </>;
};

export default WithContests(ContestList);
