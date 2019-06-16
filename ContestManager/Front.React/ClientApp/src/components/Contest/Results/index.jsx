import React from 'react';
import {
    Container,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownToggle,
    Nav,
    NavItem,
    NavLink,
    Table
} from 'reactstrap';

export default class Results extends React.Component {
    render() {
        return <Container>
            <h4 className="mt-3">Результаты</h4>

            <Nav tabs>
                <NavItem>
                    <NavLink href="#">6 класс</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink href="#">7 класс</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink href="#">8 класс</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink href="#">9 класс</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink href="#">10 класс</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink href="#" active>11 класс</NavLink>
                </NavItem>
            </Nav>

            <Table bordered size="sm">
                <thead>
                <tr>
                    <th rowspan="2">Участник</th>
                    <th rowspan="2" style={{ width: "30%" }}>Учебное заведение</th>
                    <th colspan="5">Задача</th>
                    <th rowspan="2">Сумма</th>
                    <th rowspan="2">Место</th>
                </tr>
                <tr>
                    <td>1</td>
                    <td>2</td>
                    <td>3</td>
                    <td>4</td>
                    <td>5</td>
                </tr>
                </thead>
                <tr>
                    <td>Нальберский Дмитрий Вячеславович</td>
                    <td>МАОУ Политехническая гимназия (Нижний Тагил)</td>
                    <td>7</td>
                    <td>0</td>
                    <td>7</td>
                    <td>7</td>
                    <td>0</td>
                    <td>21</td>
                    <td>1</td>
                </tr>
                <tr>
                    <td>Черкасова Лада Вадимовна</td>
                    <td>Политехническая гимназия №82 (Нижний Тагил)</td>
                    <td>7</td>
                    <td>0</td>
                    <td>7</td>
                    <td>5</td>
                    <td>2</td>
                    <td>21</td>
                    <td>1</td>
                </tr>
                <tr>
                    <td>Лисицын Константин Эдуардович</td>
                    <td>МАОУ Политехническая гимназия (Нижний Тагил)</td>
                    <td>7</td>
                    <td>0</td>
                    <td>7</td>
                    <td>2</td>
                    <td>3</td>
                    <td>19</td>
                    <td>2</td>
                </tr>
                <tr>
                    <td>Гречишкин Кирилл Юрьевич</td>
                    <td>МАОУ Политехническая гимназия (Нижний Тагил)</td>
                    <td>7</td>
                    <td>0</td>
                    <td>0</td>
                    <td>7</td>
                    <td>4</td>
                    <td>18</td>
                    <td>2</td>
                </tr>
                <tr>
                    <td>Лоновенко Дарья Дмитриевна</td>
                    <td>МБОУ гимназия №5 (Екатеринбург)</td>
                    <td>7</td>
                    <td>0</td>
                    <td>1</td>
                    <td>4</td>
                    <td>3</td>
                    <td>15</td>
                    <td>3</td>
                </tr>
                <tr>
                    <td>Мосиевский Матвей Андреевич</td>
                    <td>Гимназия 5 (Екатеринбург)</td>
                    <td>7</td>
                    <td>1</td>
                    <td>3</td>
                    <td>2</td>
                    <td>2</td>
                    <td>15</td>
                    <td>3</td>
                </tr>
                <tr>
                    <td>частиков александр дмитриевич</td>
                    <td>ЦО №1 (Нижний Тагил )</td>
                    <td>7</td>
                    <td>2</td>
                    <td>0</td>
                    <td>2</td>
                    <td>1</td>
                    <td>12</td>
                    <td>3</td>
                </tr>
                <tr>
                    <td>Никифоров Александр Михайлович</td>
                    <td>Лицей №10 (Каменск-Уральский)</td>
                    <td>3</td>
                    <td>0</td>
                    <td>0</td>
                    <td>4</td>
                    <td>1</td>
                    <td>8</td>
                    <td>4</td>
                </tr>
                <tr>
                    <td>Смышляев Андрей Игоревич</td>
                    <td>МБОУ Лицей (Нижний Тагил)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>7</td>
                    <td>0</td>
                    <td>7</td>
                    <td>4</td>
                </tr>
                <tr>
                    <td>Шубат Марк Игоревич</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>2</td>
                    <td>3</td>
                    <td>1</td>
                    <td>6</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Иваненко Григорий Алексеевич</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>2</td>
                    <td>2</td>
                    <td>0</td>
                    <td>4</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Кочнева Екатерина Михайловна</td>
                    <td>Школа №68 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>2</td>
                    <td>0</td>
                    <td>2</td>
                    <td>4</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Охотников Михаил Александрович</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Екатеринбург)</td>
                    <td>2</td>
                    <td>0</td>
                    <td>0</td>
                    <td>1</td>
                    <td>1</td>
                    <td>4</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Чемякин Андрей Александрович</td>
                    <td> (Каменск-Уральский)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>4</td>
                    <td>0</td>
                    <td>4</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Шелганова Валерия Романовна</td>
                    <td>Гимназия №40 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>2</td>
                    <td>1</td>
                    <td>0</td>
                    <td>3</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Казакова Дарья Сергеевна</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>2</td>
                    <td>0</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Русинов Виталий Дмитриевич</td>
                    <td>МБОУСШ2 (Красноуфимск)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>2</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Щербаков Данил Вячеславович</td>
                    <td>МБОУ СШ 2 (Красноуфимск)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>1</td>
                    <td>1</td>
                    <td>0</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Кадников Дмитрий Андреевич</td>
                    <td>МАОУ "СОШ №10" (Ревда )</td>
                    <td>1</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Лаврова Ксения Денисовна</td>
                    <td>Школа №2 (Красноуфимск)</td>
                    <td>1</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Молявина Юлия Олеговна</td>
                    <td>Лицей №1 (Ревда)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>1</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Попко Никита Дмитриевич</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Нижний Тагил)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>1</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Береснев Виктор Сергеевич</td>
                    <td>МБОУ Лицей (Нижний Тагил)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Болотов Михаил Игоревич</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Лесной)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Ветренко Александр Олегович</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Гаряева Амина Эдуардовна</td>
                    <td>Школа №2 (Красноуфимск)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Дунаев Яков Дмитриевич</td>
                    <td>СУНЦ УрФУ (Екатеринбург )</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Ершов Вадим Дмитриевич</td>
                    <td>Гимназия №39 (Уфа)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Зайцев Роман Алексеевич</td>
                    <td>Школа №16 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Иванович Иван Петров</td>
                    <td>Школа 5 (Первоуральск)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Истратов Илья Андреевич</td>
                    <td>Школа №16 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Кирьянова Анна Дмитриевна</td>
                    <td>МАОУ Гимназия №9 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Кривочкин Илья Сергеевич</td>
                    <td>МАОУ Лицей №4 ТМОЛ (Таганрог)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Кушнерюк Сергей Сергеевич</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Митрофанов Дмитрий Александрович</td>
                    <td>МАОУ "СОШ №32" (Краснотурьинск)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Морозов Ярослав Олегович</td>
                    <td>Гимназия №9 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Никитин Никита Олегович</td>
                    <td>ГБПОУ "СОМК" (Ревда)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Петровская Александра Евгеньевна</td>
                    <td>МАОУ гимназия 9 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Плюхин Игорь Алексеевич</td>
                    <td>МАОУ СОШ №68 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Попова Валерия</td>
                    <td>Лицей №13 (Троицк)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Попова Валерия Александровна</td>
                    <td>Лицей №13 (Челябинск)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Усманов Илья Евгеньевич</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Фатеев Андрей Андреевич</td>
                    <td>Еврогимназия (Ревда)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Фролов Юрий Максимович</td>
                    <td>Специализированный учебно-научный центр (СУНЦ) УрФУ (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Холоденко Мария Дмитриевна</td>
                    <td>МАОУ лицей №3 (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Черных Максим Вячеславович</td>
                    <td>МАОУ НГО СОШ №4 (Новая Ляля)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Шестаков Кирилл Николаевич</td>
                    <td>МБОУ "СОШ №7" (Лысьва)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Янчина Анна Андреевна</td>
                    <td>СУНЦ УрФУ (Екатеринбург)</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td>0</td>
                    <td></td>
                </tr>
            </Table>
        </Container>;
    }
}