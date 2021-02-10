function TagInput(inputId, inputName) {
    this.inputId = inputId;
    this.inputName = inputName;
    this.dataSource = [];
    this.suggestionList = [];
    this.selectedSuggestion = 0;

    $('#' + inputId).addClass('tag-input');
    $('#' + inputId).append('<div class="contents"><ul></ul><textarea></textarea></div>');
    $('#' + inputId).append('<div class="suggest"></div>');

    var tagInput = this;
    setTimeout(function () {
        tagInput.resize();
    }, 300);

    this.setSuggestion();
    this.setInteractions();
}

TagInput.prototype.addDataSourceItem = function (value, text) {
    var dataSourceItem = { value: value, text: text, selected: false };
    this.dataSource.push(dataSourceItem);
}

TagInput.prototype.resetDataSource = function () {
    this.dataSource = [];
}

TagInput.prototype.setSuggestion = function () {
    var textarea = $('#' + this.inputId + ' textarea');
    var tagInput = this;
    textarea.keydown(function (e) {
        if (e.keyCode == 8) {
            if (textarea.val().length == 0)
                tagInput.removeTag($('#' + tagInput.inputId + ' ul li').length - 1);
        }
    });
    textarea.keyup(function (e) {
        if (e.keyCode == 13) {
            tagInput.addTag();
        }
        else if (e.keyCode == 38) {
            if (tagInput.selectedSuggestion > 0)
                tagInput.selectSuggestion(tagInput.selectedSuggestion - 1);
        }
        else if (e.keyCode == 40) {
            if (tagInput.selectedSuggestion < tagInput.suggestionList.length - 1)
                tagInput.selectSuggestion(tagInput.selectedSuggestion + 1);
        }
        else {
            tagInput.loadSuggestions();
        }
    });
}

TagInput.prototype.setInteractions = function () {
    var tagInput = this;
    $('#' + this.inputId + ' ul li .delete').click(function () {
        tagInput.removeTag($(this.parentElement).index());
    });
}

TagInput.prototype.loadSuggestions = function () {
    var suggest = $('#' + this.inputId + ' .suggest');
    var textarea = $('#' + this.inputId + ' textarea');
    var tagInput = this;
    suggest.html('');
    if (textarea.val().length > 0) {
        suggest.addClass('active');
        this.suggestionList = [];
        this.selectedSuggestion = 0;
        for (var i = 0; i < this.dataSource.length; i++) {
            if (this.dataSource[i].text.toUpperCase().replace('Á', 'A').replace('Í', 'I').replace('Ú', 'U').replace('É', 'E').replace('Ó', 'O').indexOf(textarea.val().toUpperCase().replace('Á', 'A').replace('Í', 'I').replace('Ú', 'U').replace('É', 'E').replace('Ó', 'O')) != -1 && !this.dataSource[i].selected) {
                this.suggestionList.push(this.dataSource[i]);
            }
        }
        for (var i = 0; i < this.suggestionList.length; i++) {
            var html = '<a' + (i == 0 ? ' class="active"' : '') + ' id="suggest-' + i + '">' + this.suggestionList[i].text + '</a>';
            suggest.append(html);
        }

        $('#' + this.inputId + ' .suggest a').mouseenter(function () {
            tagInput.selectSuggestion($(this).index());
        });

        $('#' + this.inputId + ' .suggest a').click(function () {
            tagInput.addTag();
        });
    }
    else
        suggest.removeClass('active');
}

TagInput.prototype.addTag = function (value) {
    $('#' + this.inputId + ' textarea').val('');
    $('#' + this.inputId + ' textarea').focus();

    var dataSourceItemIndex = 0;

    if (value == null) {
        dataSourceItemIndex = this.dataSource.indexOf(this.suggestionList[this.selectedSuggestion]);
        this.suggestionList[this.selectedSuggestion].selected = true;
    }
    else {
        var selectedItem = this.dataSource.filter(function (item) {
            return item.value == value;
        })[0];
        selectedItem.selected = true;
        dataSourceItemIndex = this.dataSource.indexOf(selectedItem);
    }

    $('#' + this.inputId + ' ul').append('<li id="tag-' + dataSourceItemIndex + '"><input type="hidden" name="' + this.inputName + '" value="' + this.dataSource[dataSourceItemIndex].value + '" />' + this.dataSource[dataSourceItemIndex].text + '<a class="ic-eliminar-gray delete"></a></li>');

    this.resize();
    this.loadSuggestions();
    this.setInteractions();
}

TagInput.prototype.removeTag = function (index) {
    var dataSourceItemIndex = $('#' + this.inputId + ' ul li:nth-child(' + (index + 1) + ')').attr('id').replace('tag-', '');
    this.dataSource[dataSourceItemIndex].selected = false;

    $('#' + this.inputId + ' ul li:nth-child(' + (index + 1) + ')').remove();
    this.resize();
}

TagInput.prototype.selectSuggestion = function (selectedSuggestion) {
    this.selectedSuggestion = selectedSuggestion;
    $('#' + this.inputId + ' .suggest a').removeClass('active');
    $('#suggest-' + this.selectedSuggestion).addClass('active');
}

TagInput.prototype.resize = function () {
    var lastTag = $('#' + this.inputId + ' ul li:last-child');
    if (lastTag.length > 0)
        $('#' + this.inputId + ' textarea').css('text-indent', lastTag.position().left + lastTag.outerWidth());
    else
        $('#' + this.inputId + ' textarea').css('text-indent', 0);
}

TagInput.prototype.getSelectedTags = function () {
    return this.dataSource.filter(function (item, index) {
        return item.selected;
    });
}