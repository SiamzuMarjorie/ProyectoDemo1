$(document).ready(function () {
    var htmlMessage = '<div class="popup" id="popupMessage"><div class="popup-box"><label class="text"></label><div class="form-actions"><a class="button btn-small btn-color02">ACEPTAR</a></div></div></div>';
    var htmlConfirmation = '<div class="popup" id="popupConfirmation"><div class="popup-box"><label class="text"></label><div class="form-actions"><a class="button btn-small">CANCELAR</a><a class="button btn-small btn-color01 btn-icon ic-ok-white">ACEPTAR</a></div></div></div>';
    var htmlAlert = '<div class="alerts" id="alerts"></div>';
    $('body').append(htmlMessage);
    $('body').append(htmlConfirmation);
    $('body').append(htmlAlert);
});

function showMessage(message, callback) {

    var popupMessage = new Popup('popupMessage');
    $('#popupMessage .text').html(message);
    $('#popupMessage a').click(function () {
        if (callback !== null)
            callback();

        popupMessage.hide();
    });
    popupMessage.show();
}

function showConfirmation(message, okText, okColorClass, okIconClass, callback) {

    var popupMessage = new Popup('popupConfirmation');
    $('#popupConfirmation .text').html(message);
    $('#popupConfirmation a:last-child').attr('class', 'button btn-small ' + okColorClass + ' btn-icon ' + okIconClass);
    $('#popupConfirmation a:last-child').html(okText.toUpperCase());

    $('#popupConfirmation a:first-child').unbind();
    $('#popupConfirmation a:first-child').click(function () {
        popupMessage.hide();
        $('#popupConfirmation a').off('click');
    });

    $('#popupConfirmation a:last-child').unbind();
    $('#popupConfirmation a:last-child').click(function () {
        if (callback != null)
            callback();
        $('#popupConfirmation a').off('click');
        popupMessage.hide();
    });
    popupMessage.show();
}

function showAlert(iconClass, text, miliseconds) {
    if (typeof miliseconds == 'undefined' || miliseconds == null)
        miliseconds = 3000;

    var html = '<div class="alert"><span class="' + iconClass + '"></span><label>' + text + '</label></div>';
    $('#alerts').append(html);

    var alertBox = $('#alerts .alert:last-child');

    setTimeout(function () {
        alertBox.addClass('active');
    }, 30)

    setTimeout(function () {
        alertBox.removeClass('active');

        setTimeout(function () {
            alertBox.remove();
        }, 300)
    }, miliseconds);
}