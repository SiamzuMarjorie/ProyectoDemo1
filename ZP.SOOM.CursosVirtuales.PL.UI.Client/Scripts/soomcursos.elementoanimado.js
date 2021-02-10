var idElemento = 1;

function ElementoAnimado(x, y, width, height, src, mapa, cantidadElementos, intervaloAnimacion, delay) {
    this.x = x;
    this.y = y;
    this.bgX = 0;
    this.bgY = 0;
    this.width = width;
    this.height = height;
    this.src = src;
    this.mapa = mapa;
    this.intervaloAnimacion = intervaloAnimacion;
    this.delay = delay;
    this.cantidadElementos = cantidadElementos;
    this.id = idElemento;

    this.pintar();

    var elementoAnimado = this;

    setTimeout(function () {

        setInterval(function () {
            elementoAnimado.bgX -= mapa.cellWidth * elementoAnimado.width;
            if (elementoAnimado.bgX <= -mapa.cellWidth * elementoAnimado.width * elementoAnimado.cantidadElementos)
                elementoAnimado.bgX = 0;

            elementoAnimado.animar();
        }, intervaloAnimacion);
    }, delay)

    idElemento++;
}
ElementoAnimado.prototype.pintar = function () {
    let html = '<div class="elemento" id="elemento-' + idElemento + '"></div>';
    $('#mapa').append(html);
    $('#elemento-' + this.id).css('width', this.width * mapa.cellWidth);
    $('#elemento-' + this.id).css('height', this.height * mapa.cellHeight);
    $('#elemento-' + this.id).css('left', this.x * mapa.cellWidth);
    $('#elemento-' + this.id).css('top', (this.y - this.height + 1) * mapa.cellHeight);
    $('#elemento-' + this.id).css('z-index', this.y);
    $('#elemento-' + this.id).css('background-image', 'url(\'' + getRootUrl() + 'Images/Juego/' + this.src + '\')');
    $('#elemento-' + this.id).css('background-size', (this.width * mapa.cellWidth * this.cantidadElementos) + 'px ' + (this.height * mapa.cellHeight) + 'px');
    this.animar();
}

ElementoAnimado.prototype.animar = function () {
    $('#elemento-' + this.id).css('background-position', this.bgX + 'px ' + this.bgY + 'px');
}