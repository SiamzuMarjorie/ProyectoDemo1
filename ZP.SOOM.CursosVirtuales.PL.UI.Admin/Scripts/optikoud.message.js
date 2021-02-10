function showMessage(message, callback) {
    var popupMessage = new Popup('popupMessage');
    $('#popupMessage h1').html(message);
    $('#popupMessage a').click(function () {
        if (callback != null)
            callback();

        popupMessage.hide();
        $('#popupMessage a').off('click');
    });
    popupMessage.show();
}

function showConfirmation(message, okText, okColorClass, callback) {
    var popupMessage = new Popup('popupConfirmation');
    $('#popupConfirmation h1').html(message);
    $('#popupConfirmation a:last-child').attr('class', 'button ' + okColorClass + ' btn-icon-left btn-icon-ok');
    $('#popupConfirmation a:last-child').html(okText);

    $('#popupConfirmation a:first-child').click(function () {
        popupMessage.hide();
        $('#popupConfirmation a').off('click');
    });
    $('#popupConfirmation a:last-child').click(function () {
        if (callback != null)
            callback();
        $('#popupConfirmation a').off('click');
        popupMessage.hide();
    });
    popupMessage.show();
}