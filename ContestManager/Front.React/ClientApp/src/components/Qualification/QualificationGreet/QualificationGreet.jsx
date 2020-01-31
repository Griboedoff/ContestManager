import React from 'react';
import { Jumbotron, Button } from 'reactstrap';
import { post } from '../../../Proxy';

export const QualificationGreet = ({ contestId, setError, onStart }) => {
    return (
        <div>
            <Jumbotron>
                <h4>Отборочный тур</h4>
                <p>
                    Вам будет предложено 8 задачек на 3 часа.
                    <br />
                    Ответом на каждую задачу является число, как разделитель для десятичных дробей используйте точку.
                    <br />
                    Успехов!
                </p>
                <p className="lead">
                    <Button color="success" onClick={async () => {
                        const resp = await post(`qualification/${contestId}/start`);
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

