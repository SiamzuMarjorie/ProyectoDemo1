function ProgressBar(id, value, decimal) {

    this.id = id;
    this.value = value;
    if (decimal == null)
        decimal = 2;

    $('#' + id).addClass('barra-resultado');
    var html = '<div class="barra-progreso"></div>' +
               '<div class="ic-buen-porcentaje img-detail"></div>' +
               '<label class="label-detail">' + parseFloat(Math.round(value * 100) / 100).toFixed(decimal) + '%</label>';
    $('#' + id).html(html);

    var progressBar = this;
    this.setSize();
    $(window).resize(function () {
        progressBar.setSize();
    });
}

ProgressBar.prototype.setSize = function () {
    var width = $('#' + this.id).outerWidth();
    var total = this.value / 100.0;
    var calculatedWidth = Math.round(total * width);

    $('#' + this.id + ' .barra-progreso').css('width', calculatedWidth);
}