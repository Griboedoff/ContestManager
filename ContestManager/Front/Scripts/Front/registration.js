$.validator.addMethod("alreadyUsedEmails", function (value, element, usedEmails) {
    return $.inArray(value, usedEmails) === -1;
}, "Email уже зарегистрирован");
$.validator.addMethod("wrongConfirmCodes", function (value, element, confirmCodes) {
    return $.inArray(value, confirmCodes) === -1;
}, "Ошибка в коде подтверждения");
$.validator.addMethod("alreadyUsedConfirmCodes", function (value, element, confirmCodes) {
    return $.inArray(value, confirmCodes) === -1;
}, "Код подтверждения уже использован");

function PageData() {
    this.mode = ko.observable("");
    this.step = ko.observable("");

    this.serverError = ko.observable("");
    
    this.userName = ko.observable("");
    this.userEmail = ko.observable("");
    this.userPassword = ko.observable("");
    this.userRepeatPassword = ko.observable("");

    this.confirmCode = ko.observable("");

    this.userVkId = ko.observable("");

    this.isSending = ko.observable(false);

    var self = this;
    this.isEmailMode = ko.computed(function () { return self.mode() === "Email" });
    this.isVkMode = ko.computed(function () { return self.mode() === "Vk" });

    this.isRegistrationStep = ko.computed(function () { return self.step() === "Registration" });
    this.isConfirmStep = ko.computed(function () { return self.step() === "Confirm" });
    this.isFinishStep = ko.computed(function () { return self.step() === "Finish" });

    this.hasServerError = ko.computed(function() { return self.serverError() !== "" });

    this.alreadyUsedEmails = [];
    this.wrongConfirmCodes = [];
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
            },
            userVkId: {
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
            },
            userVkId: {
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
    data.serverError("");

    $.ajax({
        async: true,
        method: "POST",
        url: "/Registration/CreateEmailRegistrationRequest",
        cache: false,
        data:
        {
            userEmail: data.userEmail()
        },
        success: function (status) {
            switch (status) {
                case "RequestCreated":
                    setStep("Confirm");
                    break;

                case "EmailAlreadyUsed":
                    $("#userEmail").rules("remove", "alreadyUsedEmails");

                    data.alreadyUsedEmails.push(data.userEmail());
                    $("#userEmail").rules("add", { "alreadyUsedEmails": data.alreadyUsedEmails });
                    $("#registrationForm").valid();
                    break;

                default:
                    data.serverError("Неизвестный ответ сервера");
                    break;
            }
        },
        error: function() {
            data.serverError("В данный момент сервер регистрации недоступен. Повторите попытку позже.");
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
        url: "/Registration/ConfirmEmailRegistrationRequest",
        cache: false,
        data:
        {
            userName: data.userName(),
            userEmail: data.userEmail(),
            userPassword: data.userPassword(),
            confirmationCode: data.confirmCode()
        },
        success: function (status) {
            switch (status) {
                case "Success": 
                    setStep("Finish");
                    break;

                case "RequestAlreadyUsed":
                    $("#userConfirm").rules("remove", "alreadyUsedConfirmCodes");

                    data.alreadyUsedConfirmCodes.push(data.confirmCode());
                    $("#userConfirm").rules("add", { "alreadyUsedConfirmCodes": data.alreadyUsedConfirmCodes });
                    $("#registrationForm").valid();
                    break;

                case "WrongConfirmationCode":
                    $("#userConfirm").rules("remove", "wrongConfirmCodes");

                    data.wrongConfirmCodes.push(data.confirmCode());
                    $("#userConfirm").rules("add", { "wrongConfirmCodes": data.wrongConfirmCodes });
                    $("#registrationForm").valid();
                    break;

                default:
                    data.serverError("Неизвестный ответ сервера");
                    break;
            }
        },
        error: function () {
            data.serverError("В данный момент сервер регистрации недоступен. Повторите попытку позже.");
        },
        complete: function () {
            data.isSending(false);
        }
    });
}

function setEmailMode() {
    setMode("Email");
}

function setVkMode() {
    data.serverError("");

    if (data.userVkId() !== "")
        setMode("Vk");
    else
        VK.Auth.login(vkCollback, 65536);
}

function vkCollback(obj) {
    if (obj.status !== "connected") {
        data.serverError("Для регистрации необходимо авторизироваться через ВКонтакте и разрешить доступ приложению");
        return;
    }
    setMode("Vk");

    var user = obj.session.user;

    data.userName(user.last_name + " " + user.first_name);
    data.userVkId(user.id);

    $("#registrationForm").valid();
}

function sendVkRegistrationRequest() {
    if (!$("#registrationForm").valid())
        return;

    data.isSending(true);
    data.serverError("");

    $.ajax({
        async: true,
        method: "POST",
        url: "/Registration/RegisterByVk",
        cache: false,
        data:
        {
            userName: data.userName(),
            userVkId: data.userVkId()
        },
        success: function (status) {
            switch (status) {
                case "Success":
                    setStep("Finish");
                    break;

                case "VkIdAlreadyUsed":
                    setStep("Finish");
                    break;

                default:
                    data.serverError("Неизвестный ответ сервера");
                    break;
                }
        },
        error: function () {
            data.serverError("В данный момент сервер регистрации недоступен. Повторите попытку позже.");
        },
        complete: function () {
            data.isSending(false);
        }
    });
}