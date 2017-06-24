function PageData() {
    this.mode = ko.observable("");

    this.userName = ko.observable("");
    this.userEmail = ko.observable("");
    this.userPassword = ko.observable("");
    this.userRepeatPassword = ko.observable("");

    var self = this;
    this.isEmailMode = ko.computed(function () { return self.mode() === "Email" });
    this.isVkMode = ko.computed(function () { return self.mode() === "Vk" });
}
var data = new PageData();

function setMode(mode) {
    data.mode(mode);
}

function setValidators() {
    $("#registrationForm").validate({
        rules: {
            userName: {
                required: true,
                maxlength: 100,
            },
            userEmail: {
                required: true,
                email: true,
            },
            userPassword: {
                required: true,
                rangelength: [6, 20]
            },
            repeatPassword: {
                required: true,
                equalTo: "#userPassword"
            }
        },
            
        messages: {
            userName: {
                required: "Поле обязательно для заполнения",
                maxlength: "ФИО не должны превышать 100 символов",
            },
            userEmail: {
                required: "Поле обязательно для заполнения",
                email: "Неверный формат электронной почты",
            },
            userPassword: {
                required: "Поле обязательно для заполнения",
                rangelength: "Пароль должен быть от 6 до 20 символов"
            },
            repeatPassword: {
                required: "Поле обязательно для заполнения",
                equalTo: "Поля не совпадают"
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
            container.removeClass("has-success");
            container.addClass("has-error");
        },

        unhighlight: function (element) {
            var container = $(element).parent("div").parent("div");
            container.removeClass("has-error");
            container.addClass("has-success");
        },
    });
}

function sendRegistrationRequest() {
    if (!$("#registrationForm").valid())
        return;

    alert("send");
}