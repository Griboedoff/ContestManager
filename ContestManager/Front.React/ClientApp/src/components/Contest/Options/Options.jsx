import React, { useState } from 'react';
import { Button, CustomInput, Form, FormGroup, Spinner } from 'reactstrap';
import { ContestOptions } from '../../../Enums/ContestOptions';
import { ContestType } from '../../../Enums/ContestType';
import { patch, post } from '../../../Proxy';
import { hasFlag, triggerFlag } from '../../../utils';


const Options = ({ contest }) => {
    const [options, setOptions] = useState(contest.options);

    const onChange = async (value, flag) => {
        const newValue = triggerFlag(value, flag);
        setOptions(newValue);

        await patch(`contestAdmin/${contest.id}/options`, newValue);
    };


    const isQualification = contest.type === ContestType.Qualification;
    return <>
        <h4 className="mb-3">Настройки соревнования</h4>
        <Form>
            <FormGroup>
                <Switch id="registration"
                        value={options}
                        label="Открыта регистрация"
                        optType={ContestOptions.RegistrationOpen}
                        onChange={onChange}
                />
                {isQualification &&
                <Switch id="qualification"
                        value={options}
                        label="Открыт отбор"
                        optType={ContestOptions.QualificationOpen}
                        onChange={onChange}
                />}
                {!isQualification &&
                <Switch id="preresults"
                        value={options}
                        label="Открыты результаты с шифрами"
                        optType={ContestOptions.PreResultsOpen}
                        onChange={onChange}
                />
                }
                <Switch id="results"
                        value={options}
                        label="Открыты результаты с фио"
                        optType={ContestOptions.ResultsOpen}
                        onChange={onChange}
                />
                <Switch id="finished"
                        value={options}
                        label="Соревнование закончено"
                        optType={ContestOptions.Finished}
                        onChange={onChange}
                />
            </FormGroup>
        </Form>

        {isQualification && <QualificationSettings contestId={contest.id} />}
    </>;
};
Options.displayName = 'Options';


const QualificationSettings = ({ contestId }) => {
    const [fetchingCalcQualResults, setFetchingCalcQualResults] = useState(false);

    const refreshResults = async () => {
        setFetchingCalcQualResults(true);
        await Promise.all([
            await post(`contestAdmin/${contestId}/calcQualificationResults`),
            new Promise(resolve => setTimeout(resolve, 500))]);
        setFetchingCalcQualResults(false);
    };

    return <>
        <Button onClick={refreshResults}>
            {fetchingCalcQualResults && <Spinner size="sm" color="light" />}{' '}
            Обновить результаты пробного тура
        </Button>
    </>;
};

const Switch = ({ id, optType, value, label, onChange }) => (
    <CustomInput type="switch"
                 name="customSwitch"
                 checked={hasFlag(value, optType)}
                 label={label} id={id}
                 onChange={() => onChange(value, optType)} />);
Switch.displayName = 'Switch';

export default Options;
