var idTrabajadorLogueado = null;

function registrarResultadoCurso(codigo, nota, notaMaxima, aprobado) {
    $.post(getRootUrl() + 'APICursos/RegistrarResultadoCurso',
        {
            Codigo: codigo,
            Nota: nota,
            NotaMaxima: notaMaxima,
            Aprobado: aprobado,
            IdTrabajador: idTrabajadorLogueado
        }, function (data) {
            if (data.Code == 0) {
                $('#maximunScore').html(data.Data);
            }
        }
        );
}

function registrarRespuesta(codigo, pregunta, respuesta, correcto) {
    $.post(getRootUrl() + 'APICursos/RegistrarRespuestas',
        {
            Codigo: codigo,
            Pregunta: pregunta,
            Respuesta: respuesta,
            Correcto: correcto,
            IdTrabajador: idTrabajadorLogueado
        }, function (data) { }
        );
}

function reintentarExamen(codigo) {
    $.post(getRootUrl() + 'APICursos/ReintentarExamen',
        {
            Codigo: codigo,
            IdTrabajador: idTrabajadorLogueado
        }, function (data) { }
        );
}