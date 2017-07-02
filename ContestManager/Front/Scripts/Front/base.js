function changeClass(element, classToRemove, classToAdd) {
    element.removeClass(classToRemove);
    element.addClass(classToAdd);
}

function errorPlacement (error, element) {
    error.addClass("col-xs-4");
    error.children().addClass("help-block");
    error.appendTo(element.parent("div").parent("div"));
}

function highlight (element) {
    var container = $(element).parent("div").parent("div");
    changeClass(container, "has-success", "has-error");

    var glyphicon = $(element).next("span");
    changeClass(glyphicon, "glyphicon-ok", "glyphicon-remove");
}

function unhighlight (element) {
    var container = $(element).parent("div").parent("div");
    changeClass(container, "has-error", "has-success");

    var glyphicon = $(element).next("span");
    changeClass(glyphicon, "glyphicon-remove", "glyphicon-ok");
}