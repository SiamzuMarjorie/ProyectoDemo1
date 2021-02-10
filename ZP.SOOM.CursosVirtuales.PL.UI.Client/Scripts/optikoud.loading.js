var preventLoadingActive = false;

$(document).ready(function () {
    var htmlLoading = '<div class="loading"><div class="loading-container"><label class="progress"></label><svg viewBox="0 0 38 38"><path d="M19 3.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" fill="none" /><path d="M19 3.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" fill="none" /></svg></div></div>';
    $('body').append(htmlLoading);

    $('a[href][href!=""]').click(function () {
        setTimeout(function () {
            startLoading();
        }, 350);
    });
});

function preventLoading() {
    preventLoadingActive = true;
}

function startLoading() {
    if (preventLoadingActive)
        preventLoadingActive = false;
    else {
        $('.loading').addClass('active');
        $('.loading .progress').html('');
    }
}

function stopLoading() {
    $('.loading').removeClass('active');
}

function setLoadingProgress(progress) {
    $('.loading .progress').html(progress + '%');
}

function getLoadingProgress(progress) {
    return parseInt($('.loading .progress').html().replace('%', ''));
}