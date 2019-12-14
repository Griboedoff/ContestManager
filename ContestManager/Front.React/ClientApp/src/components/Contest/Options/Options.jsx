import * as React from 'react';
import { CustomInput, Form, FormGroup } from 'reactstrap';
import { ContestOptions } from '../../../Enums/ContestOptions';
import { patch } from '../../../Proxy';
import { hasFlag, triggerFlag } from '../../../utils';
import WithContest from '../../HOC/WithContest';

class Options extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            options: this.props.contest.options
        };
    }

    onChange = async (value, flag) => {
        await this.setState({ options: triggerFlag(value, flag) });
        await patch(`contests/${this.props.contest.id}/options`, this.state.options);
    };

    render() {
        return <>
            <h4 className="mb-3">Настройки соревнования</h4>
            <Form>
                <FormGroup>
                    <Switch id="registration"
                            value={this.state.options}
                            label="Открыта регистрация"
                            optType={ContestOptions.RegistrationOpen}
                            onChange={this.onChange}
                    />
                    <Switch id="preresults"
                            value={this.state.options}
                            label="Открыты предварительные результаты"
                            optType={ContestOptions.PreResultsOpen}
                            onChange={this.onChange}
                    />
                    <Switch id="results"
                            value={this.state.options}
                            label="Открыты результаты"
                            optType={ContestOptions.ResultsOpen}
                            onChange={this.onChange}
                    />
                    <Switch id="finished"
                            value={this.state.options}
                            label="Соревнование закончено"
                            optType={ContestOptions.Finished}
                            onChange={this.onChange}
                    />
                </FormGroup>
            </Form>
        </>;
    }
}

function Switch({ id, optType, value, label, onChange }) {
    return <CustomInput type="switch"
                        name="customSwitch"
                        checked={hasFlag(value, optType)}
                        label={label} id={id}
                        onChange={() => onChange(value, optType)} />;
}

export default WithContest(Options);