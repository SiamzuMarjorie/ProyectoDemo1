function DropDown(inputId, callback, inputName) {
    this.inputId = inputId;
    this.dataSource = [];
    this.suggestionList = [];
    this.selectedSuggestion = 0;
    this.callback = callback;

    $('#' + inputId).addClass('search-input');
    $('#' + inputId).html('<input type="text" placeholder="Buscar" name="' + inputName + '" autocomplete="off"/><div class="suggest"></div>');

    var DropDown = this;
    this.setSuggestion();
}

DropDown.prototype.addDataSourceItem = function (value, text) {
    var dataSourceItem = { value: value, text: text, selected: false };
    this.dataSource.push(dataSourceItem);
}

DropDown.prototype.resetDataSource = function () {
    this.dataSource = [];
}

DropDown.prototype.setSuggestion = function () {
    var textarea = $('#' + this.inputId + ' input');
    var DropDown = this;
    textarea.click(function (e) {
        if (e.keyCode == 13) {
            DropDown.selectItem();
        }
        else if (e.keyCode == 38) {
            if (DropDown.selectedSuggestion > 0)
                DropDown.selectSuggestion(DropDown.selectedSuggestion - 1);
        }
        else if (e.keyCode == 40) {
            if (DropDown.selectedSuggestion < DropDown.suggestionList.length - 1)
                DropDown.selectSuggestion(DropDown.selectedSuggestion + 1);
        }
        else {
            DropDown.loadSuggestions();
        }
    });
}

DropDown.prototype.selectItem = function (value) {
    var dataSourceItemIndex = 0;

    if (value == null) {
        dataSourceItemIndex = this.dataSource.indexOf(this.suggestionList[this.selectedSuggestion]);
    }
    else {
        var selectedItem = this.dataSource.filter(function (item) {
            return item.value == value;
        })[0];
        dataSourceItemIndex = this.dataSource.indexOf(selectedItem);
    }

    var id = this.dataSource[dataSourceItemIndex].value;
    var text = this.dataSource[dataSourceItemIndex].text;

    this.callback(id, text);

    this.loadSuggestions();
    $('#' + this.inputId + ' .suggest').removeClass('active');
}

DropDown.prototype.loadSuggestions = function () {
    var suggest = $('#' + this.inputId + ' .suggest');
    var textarea = $('#' + this.inputId + ' input');
    var DropDown = this;
    suggest.html('');
    if (textarea.val().length > 0) {
        suggest.addClass('active');
        this.suggestionList = [];
        this.selectedSuggestion = 0;
        for (var i = 0; i < this.dataSource.length; i++) {
            if (this.dataSource[i].text.toUpperCase().replace('Á', 'A').replace('Í', 'I').replace('Ú', 'U').replace('É', 'E').replace('Ó', 'O').indexOf(textarea.val().toUpperCase().replace('Á', 'A').replace('Í', 'I').replace('Ú', 'U').replace('É', 'E').replace('Ó', 'O')) != -1) {
                this.suggestionList.push(this.dataSource[i]);
            }
        }
        for (var i = 0; i < this.suggestionList.length; i++) {
            var html = '<a' + (i == 0 ? ' class="active"' : '') + ' id="suggest-' + i + '">' + this.suggestionList[i].text + '</a>';
            suggest.append(html);
        }

        $('#' + this.inputId + ' .suggest a').mouseenter(function () {
            DropDown.selectSuggestion($(this).index());
        });

        $('#' + this.inputId + ' .suggest a').click(function () {
            DropDown.selectItem();
        });
    }
    else
        suggest.removeClass('active');
}

DropDown.prototype.selectSuggestion = function (selectedSuggestion) {
    this.selectedSuggestion = selectedSuggestion;
    $('#' + this.inputId + ' .suggest a').removeClass('active');
    $('#suggest-' + this.selectedSuggestion).addClass('active');
}