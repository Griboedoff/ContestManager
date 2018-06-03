function allContainerClasses() {
    return ["has-success", "has-warning", "has-error"];
}

function allGlyphiconClasses() {
    return ["glyphicon-ok", "glyphicon-warning-sign", "glyphicon-remove"];
}

function changeClass(element, classesToRemove, classToAdd) {
    $.each(classesToRemove, function(i, classToRemove) { 
        element.removeClass(classToRemove);
    });
    element.addClass(classToAdd);
}

function errorPlacement (error, element) {
    error.addClass("col-xs-4");
    error.children().addClass("help-block");
    error.appendTo(element.parent("div").parent());
}

function highlight (element) {
    var mustBeWarn = $(element).hasClass("validation-warning");
    $(element).removeClass("validation-warning");
    
    var container = $(element).parent("div").parent();
    changeClass(container, allContainerClasses(), mustBeWarn? "has-warning" : "has-error");

    var glyphicon = $(element).next("span");
    changeClass(glyphicon, allGlyphiconClasses(), mustBeWarn? "glyphicon-warning-sign" : "glyphicon-remove");
}

function unhighlight (element) {
    var container = $(element).parent("div").parent();
    changeClass(container, allContainerClasses(), "has-success");

    var glyphicon = $(element).next("span");
    changeClass(glyphicon, allGlyphiconClasses(), "glyphicon-ok");
}