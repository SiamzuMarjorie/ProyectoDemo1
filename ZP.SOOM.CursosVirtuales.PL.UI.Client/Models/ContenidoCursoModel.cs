using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Models
{
    public class ContenidoCursoModel
    {

        public int IdContenidoCurso { get; set; }
        public int IdCurso { get; set; }
        public string Archivo { get; set; }
        public string TipoArchivo { get; set; }
        public string Nombre { get; set; }
        public string UrlArchivo { get; set; }
        public bool Visto { get; set; }

        private const String URL_ARCHIVO = "~/Cursos/{CODIGO_CURSO}/ContenidoCurso/{ARCHIVO}";
        private const String CODIGO_CURSO = "{CODIGO_CURSO}";
        private const String ARCHIVO = "{ARCHIVO}";

        public static ContenidoCursoModel FromContenidoCurso(ContenidoCurso objContenidoCurso)
        {
            try
            {
                ContenidoCursoModel objContenidoCursoModel = new ContenidoCursoModel();
                objContenidoCursoModel.IdContenidoCurso = objContenidoCurso.IdContenidoCurso;
                objContenidoCursoModel.IdCurso = objContenidoCurso.IdCurso;
                objContenidoCursoModel.Archivo = objContenidoCurso.Archivo;
                objContenidoCursoModel.TipoArchivo = objContenidoCurso.TipoArchivo;
                objContenidoCursoModel.Nombre = objContenidoCurso.Nombre;
                objContenidoCursoModel.UrlArchivo = URL_ARCHIVO.Replace(CODIGO_CURSO, objContenidoCurso.Curso.Codigo).Replace(ARCHIVO, objContenidoCurso.Archivo);

                return objContenidoCursoModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}