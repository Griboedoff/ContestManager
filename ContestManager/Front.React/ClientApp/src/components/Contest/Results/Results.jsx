import React, {useState, useMemo} from 'react';
import {Link, withRouter} from 'react-router-dom';
import {Alert, Container, Nav, NavItem, NavLink, Table} from 'reactstrap';
import {Class} from '../../../Enums/Class';
import WithUser from '../../HOC/WithUser';

const Results = ({user, results, match}) => {
    const classFromUrl = parseInt(match.params.class, 10);
    const defaultClass = Object.values(Class).includes(classFromUrl) ? classFromUrl : Class.Fifth;
    const [currentClass, setCurrentClass] = useState(defaultClass);
    const filteredResults = results[currentClass];
    const tasksCount = useMemo(() => Math.max(0, Math.max(...filteredResults.map(p => p.results.length))), [filteredResults]);
    const hasResults = filteredResults?.length !== 0;
    console.log(user);
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

        {hasResults ? <Table bordered size="sm">
                <thead>
                <tr>
                    <th rowSpan="2">Участник</th>
                    <th rowSpan="2" style={{width: "20%"}}>Учебное заведение</th>
                    <th colSpan={tasksCount}>Задача</th>
                    <th rowSpan="2">Сумма</th>
                    <th rowSpan="2">Место</th>
                </tr>
                <tr>
                    {[...Array(tasksCount).keys()].map((_, n) => <td key={n}>{n + 1}</td>)}
                </tr>
                </thead>
                <tbody>
                {filteredResults.map(r => {
                    console.log(r)
                    const bgColor = r.id === user?.id ? "lightyellow" : 'white';
                    return <tr key={r.id}
                               style={{backgroundColor: bgColor}}>
                        <td>{r.name}</td>
                        <td>{r.schoolWithCity}</td>
                        {r.results.map((r, i) => <td key={i}>{r}</td>)}
                        <td>{r.sum}</td>
                        <td>{r.place || ""}</td>
                    </tr>;
                })}
                </tbody>
            </Table> :
            <Alert color="secondary">Нет участников в {currentClass} классе</Alert>}
    </Container>;
};

export default WithUser(withRouter(Results));
