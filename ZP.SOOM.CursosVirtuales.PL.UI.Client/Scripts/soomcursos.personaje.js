function Personaje(mapa, intervaloMovimiento, posX, posY) {
    this.x = posX;
    this.y = posY;    
    this.width = 2;
    this.height = 3;
    this.bgX = 0;
    this.bgY = -this.height * mapa.cellHeight * 2;
    this.genero = 'M';
    this.mapa = mapa;
    this.movimiento = 0;
    this.intervaloMovimiento = intervaloMovimiento;

    let html = '<div class="personaje"></div>';
    $('#mapa').append(html);
    $('.personaje').css('width', this.width * mapa.cellWidth);
    $('.personaje').css('height', this.height * mapa.cellHeight);
    $('.personaje').css('left', ($(window).width() / 2) - (mapa.cellWidth * this.width) / 2);
    $('.personaje').css('top', ($(window).height() / 2) + (mapa.cellHeight / 2) - (mapa.cellHeight * this.height) + (mapa.cellHeight * 3));
    $('.personaje').css('z-index', this.y);
    this.pintar();

    var personaje = this;

    setInterval(function () {
        if (personaje.movimiento > 0) {
            personaje.bgX -= mapa.cellWidth * personaje.width;
            if (personaje.bgX <= -mapa.cellWidth * personaje.width * 9)
                personaje.bgX = -mapa.cellWidth * personaje.width;
        }
        else
            personaje.bgX = 0;

        personaje.animar();
    }, 80);
}

Personaje.prototype.mover = function (direccion) {
    var puedeMoverse = true;

    switch (direccion) {
        case 'L':
            if (this.x - 1 < 0)
                puedeMoverse = false;
            else if (mapa.grid[this.y][this.x - 1] == '#')
                puedeMoverse = false;
            else
                this.x--;
            this.bgY = -this.height * mapa.cellHeight * 1;
            break;
        case 'U':
            if (this.y - 1 < 0)
                puedeMoverse = false;
            else if (mapa.grid[this.y - 1][this.x] == '#')
                puedeMoverse = false;
            else
                this.y--;
            this.bgY = 0;
            break;
        case 'R':
            if (this.x + 1 >= mapa.width)
                puedeMoverse = false;
            else if (mapa.grid[this.y][this.x + 1] == '#')
                puedeMoverse = false;
            else
                this.x++;
            this.bgY = -this.height * mapa.cellHeight * 3;
            break;
        case 'D':
            if (this.y + 1 >= mapa.height)
                puedeMoverse = false;
            else if (mapa.grid[this.y + 1][this.x] == '#')
                puedeMoverse = false;
            else
                this.y++;
            this.bgY = -this.height * mapa.cellHeight * 2;
            break;

        default: return;
    }

    var personaje = this;

    if (puedeMoverse) {
        this.mapa.mover(direccion);
        $('.personaje').css('z-index', this.y);
        this.movimiento++;
        setTimeout(function () {
            personaje.movimiento--;
        }, this.intervaloMovimiento);
    }

    this.animar();

    return mapa.grid[this.y][this.x];
}

Personaje.prototype.pintar = function () {
    if (this.genero == 'H') {
        $('.personaje').css('background-image', 'url(\'' + getRootUrl() + 'Images/Juego/sprite_hombre.svg\')');
    }
    else if (this.genero == 'M') {
        $('.personaje').css('background-image', 'url(\'' + getRootUrl() + 'Images/Juego/sprite_mujer.svg\')');
    }
    $('.personaje').css('background-size', (this.width * mapa.cellWidth * 9) + 'px ' + (this.height * mapa.cellHeight * 4) + 'px');
    this.animar();
}

Personaje.prototype.animar = function () {
    $('.personaje').css('background-position', this.bgX + 'px ' + this.bgY + 'px');
}


Personaje.prototype.setGenero = function (genero) {
    this.genero = genero;
    this.pintar();
}