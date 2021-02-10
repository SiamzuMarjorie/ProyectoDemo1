function DatePicker(id) {
    this.id = id;
    this.wrapperId = 'dtp-' + id;
    this.day = new Date().getDate();
    this.month = new Date().getMonth();
    this.year = new Date().getFullYear();

    this.loadContent();
    this.loadMonth();
}

DatePicker.prototype.loadContent = function () {
    $('#' + this.id).wrap('<div class="datepicker" id="' + this.wrapperId + '" tabindex="1"></div>');
    var html = '<div class="datepicker-calendar" tabindex="1">' +
               '<header><a class="ic-prev2-white prev-year"><a class="ic-prev-white prev"></a>' +
               '<label class="mes"></label>' +
               '<a class="ic-next-white next"><a class="ic-next2-white next-year"></a>' +
               '</header><table><thead><tr>' +
               '<th>D</th>' +
               '<th>L</th>' +
               '<th>M</th>' +
               '<th>M</th>' +
               '<th>J</th>' +
               '<th>V</th>' +
               '<th>S</th>' +
               '</tr></thead><tbody>';

    var count = 0;
    for (var i = 0; i < 6; i++) {
        html += '<tr>';
        for (var j = 0; j < 7; j++) {
            html += '<td class="pos-' + count + '"></td>';
            count++;
        }
        html += '</tr>';
    }
    html += '</tbody></table></div>';

    $('#' + this.wrapperId).append(html);
    $('#' + this.id).focus({ wrapperId: this.wrapperId }, function (event) {
        $('#' + event.data.wrapperId + ' .datepicker-calendar').addClass('active');
        $('#' + event.data.wrapperId + ' .datepicker-calendar').focus();
    });

    var calendar = this;
    $('#' + this.wrapperId + ' .prev').click(function () { calendar.prevMonth() });
    $('#' + this.wrapperId + ' .next').click(function () { calendar.nextMonth() });

    $('#' + this.wrapperId + ' .prev-year').click(function () { calendar.prevYear() });
    $('#' + this.wrapperId + ' .next-year').click(function () { calendar.nextYear() });
}

DatePicker.prototype.loadMonth = function () {

    var mes;
    var dias = new Date(this.year, this.month + 1, 0).getDate();
    var diaSemana = new Date(this.year, this.month, 1).getDay();

    switch (this.month) {
        case 0:
            mes = 'ENERO';
            break;
        case 1:
            mes = 'FEBRERO';
            break;
        case 2:
            mes = 'MARZO';
            break;
        case 3:
            mes = 'ABRIL';
            break;
        case 4:
            mes = 'MAYO';
            break;
        case 5:
            mes = 'JUNIO';
            break;
        case 6:
            mes = 'JULIO';
            break;
        case 7:
            mes = 'AGOSTO';
            break;
        case 8:
            mes = 'SETIEMBRE';
            break;
        case 9:
            mes = 'OCTUBRE';
            break;
        case 10:
            mes = 'NOVIEMBRE';
            break;
        case 11:
            mes = 'DICIEMBRE';
            break;
    }
    mes += ' ' + this.year;

    $('#' + this.wrapperId + ' .mes').html(mes);
    $('#' + this.wrapperId + ' td').html('');
    $('#' + this.wrapperId + ' td').removeClass('today');
    $('#' + this.wrapperId + ' td').removeClass('active');

    var calendar = this;

    for (var i = 1; i <= dias; i++) {
        var fecha = this.formatDate(this.year, this.month, i);
        $('#' + this.wrapperId + ' .pos-' + diaSemana).click({ diaSemana: diaSemana, day: i }, function (event) {
            calendar.day = event.data.day;
            $('#' + calendar.id).val(calendar.formatDate(calendar.year, calendar.month, event.data.day));
            $('#' + calendar.id).change();
            $('#' + calendar.wrapperId + ' td').removeClass('active');
            $('#' + calendar.wrapperId + ' .pos-' + event.data.diaSemana).addClass('active');
            $('#' + calendar.wrapperId + ' .datepicker-calendar').removeClass('active');
            $('#' + calendar.wrapperId).focus();
        });
        $('#' + this.wrapperId + ' .pos-' + diaSemana).html(i.toString());
        if (i == new Date().getDate() && this.month == new Date().getMonth() && this.year == new Date().getFullYear())
            $('#' + this.wrapperId + ' .pos-' + diaSemana).addClass('today');
        if (fecha == $('#' + this.id).val())
            $('#' + this.wrapperId + ' .pos-' + diaSemana).addClass('active');
        diaSemana++;
    }
}

DatePicker.prototype.prevMonth = function () {

    this.month--;
    if (this.month < 0) {
        this.month = 11;
        this.year--;
    }
    this.loadMonth();
}

DatePicker.prototype.nextMonth = function () {

    this.month++;
    if (this.month > 11) {
        this.month = 0;
        this.year++;
    }
    this.loadMonth();
}

DatePicker.prototype.prevYear = function () {

    this.year--;    
    this.loadMonth();
}

DatePicker.prototype.nextYear = function () {

    this.year++;    
    this.loadMonth();
}


DatePicker.prototype.formatDate = function(year, month, day) {
    return ('0' + day).slice(-2) + '/' + ('0' + (month + 1)).slice(-2) + '/' + year;
}

DatePicker.prototype.getDate = function () {
    return new Date(this.year, this.month, this.day);
}