$.validator.addMethod("alreadyUsedEmails", function (value, element, usedEmails) {
    return $.inArray(value, usedEmails) === -1;
}, "Email уже зарегистрирован");
$.validator.addMethod("alreadyUsedConfirmCodes", function (value, element, confirmCodes) {
    return $.inArray(value, confirmCodes) === -1;
}, "Ошибка в коде подтверждения");

function PageData() {
    this.mode = ko.observable("");
    this.step = ko.observable("");

    this.hasServerError = ko.observable(false);

    this.userName = ko.observable("");
    this.userEmail = ko.observable("");
    this.userPassword = ko.observable("");
    this.userRepeatPassword = ko.observable("");

    this.confirmCode = ko.observable("");

    this.isSending = ko.observable(false);

    var self = this;
    this.isEmailMode = ko.computed(function () { return self.mode() === "Email" });
    this.isVkMode = ko.computed(function () { return self.mode() === "Vk" });

    this.isRegistrationStep = ko.computed(function () { return self.step() === "Registration" });
    this.isConfirmStep = ko.computed(function () { return self.step() === "Confirm" });
    this.isFinishStep = ko.computed(function() { return self.step() === "Finish" });

    this.alreadyUsedEmails = [];
    this.alreadyUsedConfirmCodes = [];
}
var data = new PageData();

function setMode(mode) {
    data.mode(mode);
}

function setStep(step) {
    data.step(step);
}

function changeClass(element, classToRemove, classToAdd) {
    element.removeClass(classToRemove);
    element.addClass(classToAdd);
}

function setValidators() {
    $("#registrationForm").validate({
        rules: {
            userName: {
                required: true,
                maxlength: 100
            },
            userEmail: {
                required: true,
                email: true
            },
            userPassword: {
                required: true,
                rangelength: [6, 20]
            },
            repeatPassword: {
                required: true,
                equalTo: "#userPassword"
            },
            userConfirm: {
                required: true
            }
        },
            
        messages: {
            userName: {
                required: "Поле обязательно для заполнения",
                maxlength: "ФИО не должны превышать 100 символов"
            },
            userEmail: {
                required: "Поле обязательно для заполнения",
                email: "Неверный формат электронной почты"
            },
            userPassword: {
                required: "Поле обязательно для заполнения",
                rangelength: "Пароль должен быть от 6 до 20 символов"
            },
            repeatPassword: {
                required: "Поле обязательно для заполнения",
                equalTo: "Поля не совпадают"
            },
            userConfirm: {
                required: "Поле обязательно для заполнения"
            }
        },

        onfocusout: function (element) {
            this.element(element);
        },

        wrapper: "div",
        errorElement: "p",
        
        errorPlacement: function (error, element) {
            error.addClass("col-xs-4");
            error.children().addClass("help-block");
            error.appendTo(element.parent("div").parent("div"));
        },

        highlight: function (element) {
            var container = $(element).parent("div").parent("div");
            changeClass(container, "has-success", "has-error");

            var glyphicon = $(element).next("span");
            changeClass(glyphicon, "glyphicon-ok", "glyphicon-remove");
        },

        unhighlight: function (element) {
            var container = $(element).parent("div").parent("div");
            changeClass(container, "has-error", "has-success");

            var glyphicon = $(element).next("span");
            changeClass(glyphicon, "glyphicon-remove", "glyphicon-ok");
        },
    });
}

function sendEmailRegistrationRequest() {
    if (!$("#registrationForm").valid())
        return;

    data.isSending(true);
    data.hasServerError(false);

    $.ajax({
        async: true,
        method: "POST",
        url: "/Registration/AddEmailRegistrationRequest",
        cache: false,
        data:
        {
            userName: data.userName(),
            userEmail: data.userEmail(),
            userPassword: data.userPassword()
        },
        success: function (registrationRequestResult) {
            if (registrationRequestResult === "Success")
                setStep("Confirm");
            else {
                $("#userEmail").rules("remove", "alreadyUsedEmails");

                data.alreadyUsedEmails.push(data.userEmail());
                $("#userEmail").rules("add", { "alreadyUsedEmails": data.alreadyUsedEmails });
                $("#registrationForm").valid();
            }
        },
        error: function() {
            data.hasServerError(true);
        },
        complete: function () {
            data.isSending(false);
        }
    });
}

function sendEmailConfirm() {
    if (!$("#registrationForm").valid())
        return;

    data.isSending(true);
    data.hasServerError(false);

    $.ajax({
        async: true,
        method: "POST",
        url: "/Registration/ConfirmEmailRegistration",
        cache: false,
        data:
        {
            userEmail: data.userEmail(),
            userConfirmCode: data.confirmCode()
        },
        success: function (confirmRegistrationResult) {
            if (confirmRegistrationResult === "Success")
                setStep("Finish");
            else {
                $("#userConfirm").rules("remove", "alreadyUsedConfirmCodes");

                data.alreadyUsedConfirmCodes.push(data.confirmCode());
                $("#userConfirm").rules("add", { "alreadyUsedConfirmCodes": data.alreadyUsedConfirmCodes });
                $("#registrationForm").valid();
            }
        },
        error: function () {
            data.hasServerError(true);
        },
        complete: function () {
            data.isSending(false);
        }
    });
}