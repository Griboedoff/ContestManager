export class RussianLettersValidator {
    static validate(value) {
        return /^[' а-яё-]*$/i.test(value);
    }
}

export class LengthValidator {
    constructor(min, max) {
        this.min = min;
        this.max = max;
    }

    validate(value) {
        if (this.min && this.max)
            return this.min <= value.length && value.length <= this.max;
        if (this.max)
            return value.length <= this.max;
        if (this.min)
            return this.min <= value.length;

        return false;
    }
}

export class EmailValidator {
    static validate(value) {
        return /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]*[a-zA-Z0-9!#$%&'*+\/=?^_`{|}~-]@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(value);
    }
}