function Joystick() {

    this.seguir = false;
    var joystick = this;

    $('#joystick').mousedown(function () {
        joystick.seguir = true;
    });

    $('.joystick').mousemove(function () {
        var top = event.pageY - $(this).offset().top;
        var left = event.pageX - $(this).offset().left;
        $('#joystick').css('top', top);
        $('#joystick').css('left', left);
    });

    $('#joystick').mouseup(function () {
        joystick.seguir = false;
    });
}