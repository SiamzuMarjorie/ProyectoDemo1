using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class RespuestaModel
    {
        public int IdRespuesta { get; set; }
        public int IdPregunta { get; set; }
        public int IdInscripcion { get; set; }
        public string Contenido { get; set; }
        public bool? Correcta { get; set; }

        public static RespuestaModel FromRespuesta(Respuesta objRespuesta)
        {
            try
            {
                RespuestaModel objRespuestaModel = new RespuestaModel()
                {
                    Contenido = objRespuesta.Contenido,
                    Correcta = objRespuesta.Correcta,
                    IdInscripcion = objRespuesta.Intento.IdInscripcion,
                    IdPregunta = objRespuesta.IdPregunta,
                    IdRespuesta = objRespuesta.IdRespuesta
                };
                return objRespuestaModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}