function Mapa(grid, screenWidth, screenHeight, x, y, intervaloMovimiento) {
    this.grid = grid;

    this.width = grid[0].length;
    this.height = grid.length;

    this.cellWidth = 25;
    this.cellHeight = this.cellWidth;

    this.top = (screenHeight / 2) - (y * this.cellHeight) - this.cellHeight / 2 + (this.cellHeight * 3);
    this.left = (screenWidth / 2) - (x * this.cellWidth) - this.cellWidth / 2;

    $('#mapa').css("width", this.width * this.cellWidth);
    $('#mapa').css("height", this.height * this.cellHeight);
    $('#mapa').css('left', this.left);
    $('#mapa').css('top', this.top);
    $('#mapa').css("transition-duration", intervaloMovimiento + 'ms');
    $('#mapa').css("-webkit-transition-duration", intervaloMovimiento + 'ms');
    $('#mapa').css("-moz-transition-duration", intervaloMovimiento + 'ms');
    $('#mapa').css("-o-transition-duration", intervaloMovimiento + 'ms');
}

Mapa.prototype.mover = function (direccion) {
    switch (direccion) {
        case 'L': // left
            this.left += this.cellWidth;
            break;

        case 'U': // up
            this.top += this.cellHeight;
            break;

        case 'R': // right
            this.left -= this.cellWidth;
            break;

        case 'D': // down
            this.top -= this.cellHeight;
            break;

        default: return; // exit this handler for other keys
    }

    $('#mapa').css('left', this.left);
    $('#mapa').css('top', this.top);
}


