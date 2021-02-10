using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Models
{
    public class InscripcionModel
    {
        public int IdInscripcion { get; set; }
        public int IdCursoSlot { get; set; }
        public int IdTrabajador { get; set; }
        public string Certificado { get; set; }
        public int Nota { get; set; }
        public bool Aprobado { get; set; }
        public bool Terminado { get; set; }
        public string CodigoCurso { get; set; }


        public Inscripcion ToInscripcion()
        {
            try
            {
                Inscripcion objInscripcion = new Inscripcion();
                objInscripcion.IdInscripcion = IdInscripcion;
                objInscripcion.CursoSlot.Curso.Codigo = CodigoCurso;
                //objInscripcion.Nota = Nota;

                return objInscripcion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    

}