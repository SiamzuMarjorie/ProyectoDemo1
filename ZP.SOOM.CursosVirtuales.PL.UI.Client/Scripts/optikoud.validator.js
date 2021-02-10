function Validator(fields, errorClass) {

    this.fields = fields;
    this.errorClass = errorClass;

    for (var i = 0; i < this.fields.length; i++) {
        var selector = this.fields[i].selector;

        var type = this.fields[i].type;
        if (typeof type === 'undefined')
            type = 'T';

        var maxValue = this.fields[i].maxValue;
        if (typeof maxValue === 'undefined')
            maxValue = null;

        if (type == 'I')
            $(selector).keydown(function (e) {
                if ($.inArray(e.keyCode, [8, 9, 27, 13, 110]) !== -1 ||
                        (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                        (e.keyCode >= 35 && e.keyCode <= 40)) {
                    return;
                }
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
        else if (type == 'D')
            $(selector).keydown(function (e) {
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

        if ((type == 'D' || type == 'I') && maxValue !== null)
            $(selector).keypress({ selector: selector, maxValue: maxValue }, function (event) {
                if (parseFloat($(event.data.selector).val() + String.fromCharCode(event.which)) > event.data.maxValue)
                    event.preventDefault();
            });

    }
}

Validator.prototype.validate = function () {

    var message = '';
    $('select.error, textarea.error, input.error').removeClass('error');

    var validator = this;
    for (var i = 0; i < this.fields.length; i++) {
        //Asignación de campos
        var selector = this.fields[i].selector;

        $(selector).each(function (index, item) {
            if ($(item).attr('disabled'))
                return true;

            var name = validator.fields[i].name;
            if (typeof name === 'undefined')
                name = $(item).attr('name');

            var type = validator.fields[i].type;
            if (typeof type === 'undefined')
                type = 'T';

            var minLength = validator.fields[i].minLength;
            if (typeof minLength === 'undefined')
                minLength = 0;

            var required = validator.fields[i].required;
            if (typeof required === 'undefined')
                required = false;

            var maxValue = validator.fields[i].maxValue;
            if (typeof maxValue === 'undefined')
                maxValue = null;

            //Validaciones
            var messageRequired = false;
            if (required)
                if (!validator.validateNotEmpty(item)) {
                    message += 'El campo <span class="bold">' + name + '</span> es obligatorio.\n';
                    messageRequired = true;
                }

            if (!messageRequired && !validator.validateMinLength(item, minLength, required))
                message += 'El campo <span class="bold">' + name + '</span> debe contener al menos <span class="bold">' + minLength + '</span> caracteres.\n';

            if (type == 'E')
                if (!messageRequired && !validator.validateEmailFormat(item))
                    message += 'El campo <span class="bold">' + name + '</span> es inválido.\n';

            if ((type == 'D' || type == 'I') && maxValue !== null)
                if (!messageRequired && parseFloat($(item).val()) > maxValue)
                    message += 'El valor máximo para el campo <span class="bold">' + name + '</span> es <span class="bold">' + maxValue + '</span>.\n';

            $(item).keyup(function (event) {
                $(item).removeClass('error');
            });
        });
    }

    if (message.length == 0)
        return null;

    return message;
}

Validator.prototype.validateNotEmpty = function (item) {
    $(item).val($(item).val().trim());
    var length = $(item).val().length;
    if (length == 0) {
        $(item).addClass(this.errorClass);
        return false;
    }
    else {
        $(item).removeClass(this.errorClass);
        return true;
    }
}

Validator.prototype.validateMinLength = function (item, minLength, required) {
    $(item).val($(item).val().trim());
    var length = $(item).val().length;
    if ((required || length > 0) && length < minLength) {
        $(item).addClass(this.errorClass);
        return false;
    }
    else {
        $(item).removeClass(this.errorClass);
        return true;
    }
}

Validator.prototype.validateEmailFormat = function (item) {
    $(item).val($(item).val().trim());
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    if (!pattern.test($(item).val())) {
        $(item).addClass(this.errorClass);
        return false;
    }
    else {
        $(item).removeClass(this.errorClass);
        return true;
    }
}

Validator.prototype.validatePasswordFormat = function (item) {
    $(item).val($(item).val());
    var pattern = /(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$/;
    if (!pattern.test($(item).val())) {
        $(item).addClass(this.errorClass);
        return false;
    }
    else {
        $(item).removeClass(this.errorClass);
        return true;
    }
}