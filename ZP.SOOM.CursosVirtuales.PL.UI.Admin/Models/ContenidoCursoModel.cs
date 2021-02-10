using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.Util;

using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class ContenidoCursoModel
    {
        public int IdContenidoCurso { get; set; }
        public int IdCurso { get; set; }
        public string Nombre { get; set; }
        public string TipoArchivo { get; set; }
        public string Archivo { get; set; }
        public string CodigoCurso { get; set; }

        public static ContenidoCursoModel FromContenidoCurso(ContenidoCurso objContenidoCurso)
        {
            try
            {
                ContenidoCursoModel objContenidoCursoModel = new ContenidoCursoModel();
                objContenidoCursoModel.IdContenidoCurso = objContenidoCurso.IdContenidoCurso;
                objContenidoCursoModel.IdCurso = objContenidoCurso.IdCurso;
                objContenidoCursoModel.Nombre = objContenidoCurso.Nombre;
                objContenidoCursoModel.TipoArchivo = objContenidoCurso.TipoArchivo;
                objContenidoCursoModel.Archivo = objContenidoCurso.Archivo;
                objContenidoCursoModel.CodigoCurso = objContenidoCurso.Curso.Codigo;
                return objContenidoCursoModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ContenidoCurso ToContenidoCurso()
        {
            try
            {
                ContenidoCurso objContenidoCurso = new ContenidoCurso();
                objContenidoCurso.IdContenidoCurso = IdContenidoCurso;
                objContenidoCurso.IdCurso = IdCurso;
                objContenidoCurso.Nombre = Nombre;
                objContenidoCurso.TipoArchivo = TipoArchivo;
                objContenidoCurso.Archivo = Archivo;
                return objContenidoCurso;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}