import React, { useState, useMemo } from 'react';
import { Link, withRouter } from 'react-router-dom';
import { Alert, Container, Nav, NavItem, NavLink, Table } from 'reactstrap';
import { Class } from '../../../Enums/Class';
import sortBy from 'lodash/sortBy';

const Results = ({ match, participants }) => {
    const classFromUrl = parseInt(match.params.class, 10);
    const defaultClass = Object.values(Class).includes(classFromUrl) ? classFromUrl : Class.Fifth;
    const [currentClass, setCurrentClass] = useState(defaultClass);
    const tasksCount = useMemo(() => Math.max(...participants.map(p => p.results.length)), [participants]);
    const filteredParticipants = useMemo(() => sortBy(
        participants.filter(p => p.userSnapshot.class === currentClass),
        [p => -p.results.map(r => parseInt(r)).reduce((a, b) => a + b, 0), p => p.userSnapshot.name]
        ),
        [participants, currentClass]);

    const hasParticipants = filteredParticipants.length !== 0;

    return <Container>
        <h4 className="mt-3">Результаты</h4>

        <Nav tabs>
            {Object.keys(Class).map(classKey => {
                const classValue = Class[classKey];

                return <NavItem key={classKey}>
                    <NavLink
                        tag={Link}
                        to={`${classValue}`}
                        onClick={() => setCurrentClass(classValue)}
                        active={currentClass === classValue}>{classValue} класс</NavLink>
                </NavItem>;
            })}
        </Nav>

        {hasParticipants ? <Table bordered size="sm">
            <thead>
            <tr>
                <th rowSpan="2">Участник</th>
                <th rowSpan="2" style={{ width: "20%" }}>Учебное заведение</th>
                <th colSpan={tasksCount}>Задача</th>
                <th rowSpan="2">Сумма</th>
                <th rowSpan="2">Место</th>
            </tr>
            <tr>
                {[...Array(tasksCount).keys()].map((_, n) => <td key={n}>{n + 1}</td>)}
            </tr>
            </thead>
            <tbody>
            {filteredParticipants.map(p => {
                const user = p.userSnapshot;

                return <tr key={p.id}>
                    <td>{user.name}</td>
                    <td>{`${user.school} (${user.city})`}</td>
                    {p.results.map((r, i) => <td key={i}>{r}</td>)}
                    <td>{p.results.reduce((a, b) => parseInt(b) + a, 0)}</td>
                    <td>{p.place || ""}</td>
                </tr>;
            })}
            </tbody>
        </Table>:
        <Alert color="secondary">Нет участников в {currentClass} классе</Alert>}
    </Container>;
};

export default withRouter(Results);
