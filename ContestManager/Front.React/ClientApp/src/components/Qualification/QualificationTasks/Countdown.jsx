import React from 'react';
import Countdown from 'react-countdown';

const renderer = ({ hours, minutes, seconds, completed }) => {
    if (completed) {
        return <span>Время закончилось</span>;
    } else {
        return <span>До конца {hours}:{`${minutes}`.padStart(2, '0')}:{`${seconds}`.padStart(2, '0')}</span>;
    }
};

export const CountdownWrapper = ({ seconds }) => <Countdown
    date={Date.now() + seconds * 1000}
    renderer={renderer}
/>;
