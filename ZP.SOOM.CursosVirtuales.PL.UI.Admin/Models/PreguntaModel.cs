using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class PreguntaModel
    {
        public int IdPregunta { get; set; }
        public int IdCursoSlot { get; set; }
        public string Contenido { get; set; }

        public static PreguntaModel FromPregunta(Pregunta objPregunta)
        {
            try
            {
                PreguntaModel objPreguntaModel = new PreguntaModel
                {
                    Contenido = objPregunta.Contenido,
                    IdCursoSlot = objPregunta.IdCursoSlot,
                    IdPregunta = objPregunta.IdPregunta
                };

                return objPreguntaModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}