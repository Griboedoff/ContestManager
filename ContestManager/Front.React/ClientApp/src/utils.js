export function hasFlag(value, flag) {
    return (value & flag) === flag;
}

export function setFlag(value, flag) {
    return value | flag;
}

export function removeFlag(value, flag) {
    return value & ~flag;
}

export function triggerFlag(value, flag) {
    return hasFlag(value, flag) ? removeFlag(value, flag) : setFlag(value, flag);
}

export function getFromStorage(key) {
    return JSON.parse(window.localStorage.getItem(key));
}

export function setToStorage(key, data) {
    window.localStorage.setItem(key, JSON.stringify(data));
}
