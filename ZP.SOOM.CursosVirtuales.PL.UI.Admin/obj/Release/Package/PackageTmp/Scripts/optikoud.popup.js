function Popup(popupId) {
    this.popupId = popupId;
    $('#' + popupId + ' .close').click(function () {
        $('#' + popupId).removeClass('active');
    });
    var popup = this;
    $(document).keyup(function (e) {
        if (e.which == 27) {
            popup.hide();
        }
    });

    $('#' + popupId).click(function (event) {
        if (!$(event.target).is('#' + popupId + ' *'))
            popup.hide();
    });
}

Popup.prototype.show = function () {
    $('#' + this.popupId).addClass('active');
}

Popup.prototype.hide = function () {
    $('#' + this.popupId).removeClass('active');
}