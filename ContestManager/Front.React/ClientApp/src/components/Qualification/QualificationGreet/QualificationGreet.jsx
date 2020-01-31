import React from 'react';
import { Jumbotron, Button } from 'reactstrap';
import { post } from '../../../Proxy';

export const QualificationGreet = ({ contestId, setError, onStart }) => {
    return (
        <div>
            <Jumbotron>
                <h3>Отборочный тур</h3>
                <p>
                    Вам будет предложено 8 задачек на 3 часа.
                    <br />
                    Ответ на каждую задачу — это число. Десятичные дроби записывайте через точку.
                    <br />
                    Пока не закончилось время вы в любой момент можете изменить ответ на задачу, не забывайте нажимать
                    на кнопку "Сохранить ответ" — она активна если есть несохраненные данные.

                    <br />
                    Успехов!
                </p>
                <p className="lead">
                    <Button color="success" onClick={async () => {
                        const resp = await post(`qualification/start?contestId=${contestId}`);
                        if (!resp.ok)
                            setError('Ошибка при старте отборочного тура');
                        else
                            onStart();
                    }}>Начать решать</Button>
                </p>
            </Jumbotron>
        </div>
    );
};

