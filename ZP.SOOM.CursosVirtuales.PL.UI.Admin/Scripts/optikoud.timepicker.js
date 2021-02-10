function TimePicker(id) {

    this.inputId = id;
    this.id = 'timepicker-' + id;
    this.selecting = false;
    this.selected = false;
    this.hour = true;

    this.hours = 0;
    this.minutes = 0;
    this.pm = false;

    this.loadContent();
    this.setInteractions();
}

TimePicker.prototype.loadContent = function () {

    var timepicker = this;
    var angleAction = 360.0 / 60.0;

    var html = '<div class="timepicker" id="' + this.id + '"><div class="timepicker-box"><div class="time"><div class="numbers"><a class="hours">2</a><label>:</label><a class="minutes">00</a></div><div class="part-day"><a>AM</a><a>PM</a></div></div><div class="clock"><div class="marker"></div><div class="numbers">';

    for (var i = 1; i <= 12; i++)
        html += '<label>' + i + '</label>';

    html += '</div><div class="actions">';

    for (var i = 0; i < 60; i++) {
        html += '<a>' + i + '</a>';
    }

    html += '</div></div><div class="form-actions align-center"><a class="button btn-small cancel">CANCELAR</a><a class="button btn-small btn-green ok">ACEPTAR</a></div></div></div>';

    $('body').append(html);

    var spacing = $('#' + this.id + ' .numbers label:first-child').innerHeight() / 2;
    var radius = $('#' + this.id + ' .clock').innerHeight() / 2 - spacing;
    var angle = 360.0 / ($('#' + this.id + ' .numbers label').size() - 1);

    $('#' + this.id + ' .numbers label').each(function (i, item) {
        let top = radius - Math.cos(angle * i * Math.PI / 180.0) * radius + spacing;
        let left = radius + Math.sin(angle * i * Math.PI / 180.0) * radius + spacing;

        $(item).css('top', top);
        $(item).css('left', left);
    });

    $('#' + this.id + ' .actions a').each(function (i, item) {
        let top = radius - Math.cos(angleAction * i * Math.PI / 180) * radius + spacing;
        let left = radius + Math.sin(angleAction * i * Math.PI / 180) * radius + spacing;

        $(item).css('top', top);
        $(item).css('left', left);
        $(item).css('transform', 'translate(-50%, -50%) rotate(' + (angleAction * i) + 'deg)');
    });

    $('#' + this.id + ' .actions a').mousedown(function () {
        timepicker.setTime(this);
    });
    $('#' + this.id + ' .actions a').mouseover(function () {
        if (timepicker.selecting) {
            timepicker.setTime(this);
        }
    });

    $('#' + this.id + ' .time .numbers a:first-child').click(function () {
        timepicker.changeToHours();
    });
    $('#' + this.id + ' .time .numbers a:last-child').click(function () {
        timepicker.changeToMinutes();
    });

    $('#' + this.id + ' .time .part-day a:first-child').click(function () {
        timepicker.setPartOfDay(false);
    });
    $('#' + this.id + ' .time .part-day a:last-child').click(function () {
        timepicker.setPartOfDay(true);
    });

    $('#' + this.id).mousedown(function () {
        timepicker.selecting = true;
    });
    $('#' + this.id).mouseup(function () {
        timepicker.selecting = false;
        $('#' + this.id + ' .marker').removeClass('selecting');

        if (timepicker.selected)
            timepicker.changeToMinutes();
    });

    $('#' + this.id).on('touchstart', function () {
        timepicker.selecting = true;
    });
    $('#' + this.id).on('touchend', function () {
        timepicker.selecting = false;
        $('#' + this.id + ' .marker').removeClass('selecting');

        if (timepicker.selected)
            timepicker.changeToMinutes();
    });
    $('#' + this.id).on('touchmove', function () {
        event.preventDefault();
        if ($(event.target).is('#' + timepicker.id + ' .actions a')) {
            if (timepicker.selecting) {
                timepicker.setTime(event.target);
            }
        }
    });

    $('#' + this.inputId).focusin(function () {
        timepicker.show();
        this.blur();
    });
    $('#' + this.id + ' .cancel').click(function () {
        timepicker.hide();
    });
    $('#' + this.id + ' .ok').click(function () {
        timepicker.selectTime();
        timepicker.hide();
    });
    $('#' + this.id).click(function () {
        if (!$(event.target).is('.timepicker-box, .timepicker-box *'))
            timepicker.hide();
    });

    $(document).keyup(function (e) {
        if (e.which == 27) {
            timepicker.hide();
        }
    });

    this.loadTime();
}

TimePicker.prototype.changeToMinutes = function () {
    if (this.hour)
        this.hour = false;

    this.setInteractions();
}

TimePicker.prototype.changeToHours = function () {
    if (!this.hour)
        this.hour = true;

    this.setInteractions();
}

TimePicker.prototype.setPartOfDay = function (pm) {
    this.pm = pm;
    this.selectedTime = this.formatTime(this.hours, this.minutes, this.pm);
    this.setInteractions();
}

TimePicker.prototype.setInteractions = function () {
    var timepicker = this;

    $('#' + this.id + ' .actions a').removeClass('hidden');
    $('#' + this.id + ' .actions a').removeClass('hour');
    $('#' + this.id + ' .actions a').each(function (i, item) {
        if (timepicker.hour) {
            if (i % 5 !== 0)
                $(item).addClass('hidden');
            else
                $(item).addClass('hour');
        }
    });
    $('#' + this.id + ' .clock .numbers label').each(function (i, item) {
        if (timepicker.hour)
            $(item).html(i + 1);
        else {
            if (i < 11)
                $(item).html(('0' + ((i + 1) * 5)).slice(-2));
            else
                $(item).html('00');
        }
    });

    $('#' + this.id + ' .time a').removeClass('active');
    if (this.hour)
        $('#' + this.id + ' .time .numbers a:first-child').addClass('active');
    else
        $('#' + this.id + ' .time .numbers a:last-child').addClass('active');

    if (!this.pm)
        $('#' + this.id + ' .time .part-day a:first-child').addClass('active');
    else
        $('#' + this.id + ' .time .part-day a:last-child').addClass('active');

    this.loadTime();
    timepicker.selected = false;
}

TimePicker.prototype.setTime = function (item) {
    var index = $(item).index();

    if (this.hour)
        this.hours = index / 5;
    else
        this.minutes = index;

    $('#' + this.id + ' .marker').addClass('selecting');
    this.selected = true;

    this.selectedTime = this.formatTime(this.hours, this.minutes, this.pm);
    this.loadTime();
}

TimePicker.prototype.loadTime = function () {
    var angleAction = 360.0 / 60.0;
    var index;

    if (this.hour)
        index = this.hours * 5;
    else
        index = this.minutes;

    $('#' + this.id + ' .timepicker-box .clock .marker').css('transform', 'rotate(' + (angleAction * index) + 'deg)');

    $('#' + this.id + ' .clock .numbers label').removeClass('text-color-white');

    if (index >= 59 || index <= 1)
        $('#' + this.id + ' .clock .numbers label:nth-child(12)').addClass('text-color-white');
    else if (index >= 4 && index <= 6)
        $('#' + this.id + ' .clock .numbers label:nth-child(1)').addClass('text-color-white');
    else if (index >= 9 && index <= 11)
        $('#' + this.id + ' .clock .numbers label:nth-child(2)').addClass('text-color-white');
    else if (index >= 14 && index <= 16)
        $('#' + this.id + ' .clock .numbers label:nth-child(3)').addClass('text-color-white');
    else if (index >= 19 && index <= 21)
        $('#' + this.id + ' .clock .numbers label:nth-child(4)').addClass('text-color-white');
    else if (index >= 24 && index <= 26)
        $('#' + this.id + ' .clock .numbers label:nth-child(5)').addClass('text-color-white');
    else if (index >= 29 && index <= 31)
        $('#' + this.id + ' .clock .numbers label:nth-child(6)').addClass('text-color-white');
    else if (index >= 34 && index <= 36)
        $('#' + this.id + ' .clock .numbers label:nth-child(7)').addClass('text-color-white');
    else if (index >= 39 && index <= 41)
        $('#' + this.id + ' .clock .numbers label:nth-child(8)').addClass('text-color-white');
    else if (index >= 44 && index <= 46)
        $('#' + this.id + ' .clock .numbers label:nth-child(9)').addClass('text-color-white');
    else if (index >= 49 && index <= 51)
        $('#' + this.id + ' .clock .numbers label:nth-child(10)').addClass('text-color-white');
    else if (index >= 54 && index <= 56)
        $('#' + this.id + ' .clock .numbers label:nth-child(11)').addClass('text-color-white');

    var hoursText = this.hours;
    if (hoursText == 0)
        hoursText = 12;

    $('#' + this.id + ' .time .numbers .hours').html(hoursText);
    $('#' + this.id + ' .time .numbers .minutes').html(('0' + this.minutes).slice(-2));
}

TimePicker.prototype.formatTime = function (hours, minutes, pm) {

    if (pm)
        hours += 12;

    return ('0' + hours).slice(-2) + ':' + ('0' + minutes).slice(-2);
}

TimePicker.prototype.selectTime = function () {
    $('#' + this.inputId).val(this.selectedTime);
}

TimePicker.prototype.show = function () {

    this.selectedTime = $('#' + this.inputId).val();

    var time = null;

    if (this.selectedTime !== '' || !this.selectedTime == null)
        time = this.selectedTime.split(':');

    if (time == null) {
        this.hours = 11;
        this.minutes = 0;
    }
    else {
        this.hours = parseInt(time[0]);
        this.minutes = parseInt(time[1]);
    }

    if (this.hours >= 12) {
        this.hours -= 12;
        this.pm = true;
    }
    else
        this.pm = false;

    this.hour = true;

    this.setInteractions();

    $('#' + this.id).addClass('active');
}

TimePicker.prototype.hide = function () {
    $('#' + this.id).removeClass('active');
}