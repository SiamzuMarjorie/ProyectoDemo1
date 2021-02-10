function Elemento(x, y, width, height, src, mapa) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
    this.src = src;
    this.mapa = mapa;
   
    let html = '<div class="elemento"></div>';
    $('#mapa').append(html);
    $('.elemento:last-child').css('width', this.width * mapa.cellWidth);
    $('.elemento:last-child').css('height', this.height * mapa.cellHeight);
    $('.elemento:last-child').css('left', this.x * mapa.cellWidth);
    $('.elemento:last-child').css('top', (this.y - this.height + 1) * mapa.cellHeight);
    $('.elemento:last-child').css('z-index', this.y);
    $('.elemento:last-child').css('background-image', 'url(\'' + getRootUrl() + 'Images/Juego/' + this.src + '\')');
}
