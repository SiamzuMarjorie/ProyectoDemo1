function SearchInput(inputId, callback, inputName) {
    this.inputId = inputId;
    this.dataSource = [];
    this.suggestionList = [];
    this.selectedSuggestion = 0;
    this.callback = callback;

    $('#' + inputId).addClass('search-input');
    $('#' + inputId).html('<input type="text" placeholder="Buscar" name="' + inputName + '" autocomplete="off"/><div class="suggest"></div>');

    var searchInput = this;
    this.setSuggestion();
}

SearchInput.prototype.addDataSourceItem = function (value, text) {
    var dataSourceItem = { value: value, text: text, selected: false };
    this.dataSource.push(dataSourceItem);
}

SearchInput.prototype.resetDataSource = function () {
    this.dataSource = [];
}

SearchInput.prototype.setSuggestion = function () {
    var textarea = $('#' + this.inputId + ' input');
    var searchInput = this;
    textarea.keyup(function (e) {
        if (e.keyCode == 13) {
            searchInput.selectItem();
        }
        else if (e.keyCode == 38) {
            if (searchInput.selectedSuggestion > 0)
                searchInput.selectSuggestion(searchInput.selectedSuggestion - 1);
        }
        else if (e.keyCode == 40) {
            if (searchInput.selectedSuggestion < searchInput.suggestionList.length - 1)
                searchInput.selectSuggestion(searchInput.selectedSuggestion + 1);
        }
        else {
            searchInput.loadSuggestions();
        }
    });
}

SearchInput.prototype.selectItem = function (value) {
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

SearchInput.prototype.loadSuggestions = function () {
    var suggest = $('#' + this.inputId + ' .suggest');
    var textarea = $('#' + this.inputId + ' input');
    var searchInput = this;
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
            searchInput.selectSuggestion($(this).index());
        });

        $('#' + this.inputId + ' .suggest a').click(function () {
            searchInput.selectItem();
        });
    }
    else
        suggest.removeClass('active');
}

SearchInput.prototype.selectSuggestion = function (selectedSuggestion) {
    this.selectedSuggestion = selectedSuggestion;
    $('#' + this.inputId + ' .suggest a').removeClass('active');
    $('#suggest-' + this.selectedSuggestion).addClass('active');
}