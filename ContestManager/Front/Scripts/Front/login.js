function PageData() {
    this.mode = ko.observable("");
    this.serverError = ko.observable("");

    this.userEmail = ko.observable("");
    this.userPassword = ko.observable("");

    this.isSending = ko.observable(false);

    var self = this;
    this.isEmailMode = ko.computed(function () { return self.mode() === "Email" });
    this.isVkMode = ko.computed(function () { return self.mode() === "Vk" });
    this.hasServerError = ko.computed(function () { return self.serverError() !== "" });
}
var data = new PageData();

function setMode(mode) {
    data.mode(mode);
}

function setEmailMode() {
    setMode("Email");
}

function setVkMode() {
    data.serverError("");

    VK.Auth.login(vkCallback, 65536);
}

function vkCallback(obj) {
    if (obj.status !== "connected") {
        data.serverError("Для авторизации необходимо авторизироваться через ВКонтакте и разрешить доступ приложению");
        return;
    }

    setMode("Vk");
}

function setValidators() {
    $("#loginForm").validate({
        rules: {
            userEmail: {
                required: true,
                email: true
            },
            userPassword: {
                required: true
            }
        },

        messages: {
            userEmail: {
                required: "Поле обязательно для заполнения",
                email: "Неверный формат электронной почты"
            },
            userPassword: {
                required: "Поле обязательно для заполнения",
            }
        },

        onfocusout: function (element) {
            this.element(element);
        },

        wrapper: "div",
        errorElement: "p",

        errorPlacement: errorPlacement,
        highlight: highlight,
        unhighlight: unhighlight
    });
}

function sendLoginRequest() {
    if (!$("#loginForm").valid())
        return;

    data.isSending(true);
    data.serverError("");

    $.ajax({
        async: true,
        method: "POST",
        url: "/Login/Login",
        cache: false,
        data:
        {
            userEmail: data.userEmail(),
            userPassword: data.userPassword()
        },
        success: function (status) {
            switch (status) {
                case "Success":
                    break;

                case "Fail":
                    data.serverError("Неправильный email или пароль");
                    data.userPassword("");

                    $("#loginForm").valid();
                    break;

                default:
                    data.serverError("Неизвестный ответ сервера");
                    break;
            }
        },
        error: function () {
            data.serverError("В данный момент сервер недоступен. Повторите попытку позже.");
        },
        complete: function () {
            data.isSending(false);
        }
    });
}