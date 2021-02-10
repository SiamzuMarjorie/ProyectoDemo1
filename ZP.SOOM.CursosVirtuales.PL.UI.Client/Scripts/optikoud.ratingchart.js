
function RatingChart(id, valor, colorClass, quantity) {

    $('#' + id).addClass('star');
    $('#' + id).addClass('star-dashboard');
    $('#' + id).addClass('small');
    $('#' + id).addClass(colorClass);

    if (quantity == null)
        quantity = 5;

    for (var i = 0; i < quantity; i++)
        $('#' + id).append('<div class="estrella"></div>');

    for (var i = 1; i <= quantity; i++) {
        var claseStar = '';
        if (valor > i)
            claseStar = 'completa';
        else {
            var fraccion = valor - (i - 1);

            if (fraccion < 0.25)
                claseStar = '';
            else if (fraccion < 0.5)
                claseStar = 'un-cuarto';
            else if (fraccion < 0.75)
                claseStar = 'mitad';
            else if (fraccion < 1)
                claseStar = 'tres-cuartos';
            else
                claseStar = 'completa';
        }

        setTimeout(function (claseStar, i) {
            $('#' + id + ' div:nth-child(' + i + ')').addClass(claseStar);
        }, 200 * i, claseStar, i);
    }
}