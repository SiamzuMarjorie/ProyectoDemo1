function startLoading() {
    $('.loading').addClass('active');
    $('.loading .progress').html('');
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