function ElementoNombre(X, Y, W, height, mapa, Titulo, ColorBackground, ColorText, EstadoCurso, idSlot) {
    this.x = X;
    this.y = Y;
    this.width = W;
    this.height = height
    this.mapa = mapa;
    this.nombre = Titulo;
    this.background = ColorBackground;
    this.textcolor = ColorText;

    let html = '<div class="elemento-name"><label class="nombre"></label><div id="bolita-'+idSlot+'" class="bolita"></div></div>';


    $('#mapa').append(html);
    $('.elemento-name:last-child .bolita').addClass(EstadoCurso);
    $('.elemento-name:last-child').css('width', this.width * mapa.cellWidth);
    $('.elemento-name:last-child').css('left', this.x * mapa.cellWidth);
    $('.elemento-name:last-child').css('top', (this.y - this.height + 1) * mapa.cellHeight);
    $('.elemento-name:last-child').css('z-index', 2000);
    $('.elemento-name:last-child .nombre').html(this.nombre);
    $('.elemento-name:last-child').css('background-color', this.textcolor);
    $('.elemento-name:last-child .nombre').css('color', this.background);
    $('.elemento-name:last-child').css('border', '3px solid #fff');
    $('.elemento-name:last-child').css('border-radius', '20px');
    $('.elemento-name:last-child .nombre').css({ 'padding': "10px 0px", "text-align": "center", "font-weight": "bold", "word-break": "break-word" });
    $('.elemento-name:last-child .bolita').css({ 'width': "15px", "height": "15px", "border-radius": "50%", "position": "absolute", "top": "-5px", "right": "-10px", "border": "3px solid #fff" });
}