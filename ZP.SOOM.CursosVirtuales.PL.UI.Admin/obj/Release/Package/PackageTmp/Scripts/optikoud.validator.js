$(document).ready(function () {
    $('.only-numbers').keydown(function (e) {
        if ($.inArray(e.keyCode, [8, 9, 27, 13, 110]) !== -1 ||
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                (e.keyCode >= 35 && e.keyCode <= 40)) {
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $('.only-decimal-numbers').keydown(function (e) {
        if ($.inArray(e.keyCode, [8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                (e.keyCode >= 35 && e.keyCode <= 40)) {
            if ($(this).val().indexOf('.') !== -1 && e.keyCode == 190)
                e.preventDefault();
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
});

function validateNotEmpty(className, requiredBorderColor) {
    var complete = true;
    $('.' + className).each(function (i, item) {
        $(item).val($(item).val().trim());
        if (!$(item).val()) {
            complete = false;
            $(item).css('border-color', requiredBorderColor);
        }
        else
            $(item).css('border-color', '');
    });
    return complete;
}

function validateEmailFormat(className, invalidBorderColor) {
    var valid = true;

    $('.' + className).each(function (i, item) {
        $(item).val($(item).val().trim());
        var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
        if (!pattern.test($(item).val())) {
            valid = false;
            $(item).css('border-color', invalidBorderColor);
        }
        else
            $(item).css('border-color', '');
    });
    return valid;
}

function validatePasswordFormat(className, invalidBorderColor) {
    var valid = true;

    $('.' + className).each(function (i, item) {
        $(item).val($(item).val());
        var pattern = /(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/;
        if (!pattern.test($(item).val())) {
            valid = false;
            $(item).css('border-color', invalidBorderColor);
        }
        else
            $(item).css('border-color', '');
    });
    return valid;
}