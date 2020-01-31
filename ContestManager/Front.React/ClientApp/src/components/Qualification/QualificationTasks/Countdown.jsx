import React from 'react';
import Countdown from 'react-countdown';

const renderer = ({ hours, minutes, seconds, completed }) => {
    if (completed) {
        return <span>Время закончилось</span>;
    } else {
        return <span>{hours}:{minutes}:{seconds}</span>;
    }
};

export const CountdownWrapper = ({ seconds }) => <Countdown
    date={Date.now() + seconds}
    renderer={renderer}
/>;
