function RingChart(id, colorClass, value) {
    var html = '<svg viewBox="0 0 38 38">' +
        '<path d="M19 3.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" fill="none" stroke-width="6" />' +
        '<path d="M19 3.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" fill="none" stroke-width="6" /></svg>' +
        '<label>' + value + '%</label>';

    $('#' + id).append(html);
    $('#' + id).addClass('ring-chart');
    $('#' + id).addClass(colorClass);
    setTimeout(function () {
        $('#' + id + ' svg path:last-child').css('stroke-dasharray', value + 'px 100px');
    }, 100);
}