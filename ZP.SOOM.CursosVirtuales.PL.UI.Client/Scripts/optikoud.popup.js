function Popup(popupId) {
    this.popupId = popupId;

    var popup = this;
    $('#' + popupId + ' .close').click(function () {
        popup.hide();
    });


    $(document).keyup(function (e) {
        if (e.which == 27) {
            popup.hide();
        }
    });
}

Popup.prototype.show = function () {
    $('#' + this.popupId).addClass('active');
}

Popup.prototype.hide = function () {
    $('#' + this.popupId).removeClass('active');
}