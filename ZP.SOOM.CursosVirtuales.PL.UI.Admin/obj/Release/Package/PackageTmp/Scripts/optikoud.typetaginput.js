function TypeTagInput(inputId, inputName) {
    this.inputId = inputId;
    this.inputName = inputName;
    this.index = 0;
    this.tags = [];

    $('#' + inputId).addClass('tag-input');
    $('#' + inputId).append('<div class="contents"><ul></ul><textarea></textarea></div>');
    $('#' + inputId).append('<div class="suggest"></div>');

    var typeTagInput = this;
    setTimeout(function () {
        typeTagInput.resize();
    }, 300);

    this.setSuggestion();
    this.setInteractions();
}

TypeTagInput.prototype.setSuggestion = function () {
    var textarea = $('#' + this.inputId + ' textarea');
    var typeTagInput = this;
    textarea.keydown(function (e) {
        if (e.keyCode == 8) {
            if (textarea.val().length == 0)
                typeTagInput.removeTag($('#' + typeTagInput.inputId + ' ul li').length - 1);
        }
    });
    textarea.keyup(function (e) {
        if (e.keyCode == 13 || e.keyCode == 188) {
            typeTagInput.addTag();
        }
    });
}
    TypeTagInput.prototype.setInteractions = function () {
        var typeTagInput = this;
        $('#' + this.inputId + ' ul li .delete').click(function () {
            typeTagInput.removeTag($(this.parentElement).index());
        });
    }

    TypeTagInput.prototype.addTag = function (value) {

        if (value == null) {
            value = $('#' + this.inputId + ' textarea').val().replace(',', '').replace(';', '').trim();
        }
        $('#' + this.inputId + ' textarea').val('');
        $('#' + this.inputId + ' textarea').focus();

        $('#' + this.inputId + ' ul').append('<li id="tag-' + this.index + '"><label>' + value + '</label><a class="ic-eliminar-gray delete"></a></li>');
        this.tags.push(value);

        this.resize();
        this.setInteractions();

        this.index++;
    }
    TypeTagInput.prototype.removeTag = function (index) {
        $('#' + this.inputId + ' ul li:nth-child(' + (index + 1) + ')').remove();
        this.tags.splice(index, 1);
        this.resize();
    }

    TypeTagInput.prototype.resize = function () {
        var lastTag = $('#' + this.inputId + ' ul li:last-child');
        if (lastTag.length > 0)
            $('#' + this.inputId + ' textarea').css('text-indent', lastTag.position().left + lastTag.outerWidth());
        else
            $('#' + this.inputId + ' textarea').css('text-indent', 0);
    }

    TypeTagInput.prototype.getTags = function () {
        var tagString = '';
        for (var i = 0; i < this.tags.length; i++) {
            if (i > 0)
                tagString += ',';

            tagString += this.tags[i];
        }
        return tagString;
    }
 