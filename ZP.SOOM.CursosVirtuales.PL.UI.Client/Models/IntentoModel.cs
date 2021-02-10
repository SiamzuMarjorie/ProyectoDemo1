using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Models
{
    public class IntentoModel
    {
        public int IdIntento { get; set; }
        public int IdInscripcion { get; set; }
        public double? NotaMaxima { get; set; }
        public double? Nota { get; set; }
        public double? Puntaje { get; set; }
        public bool? Aprobado { get; set; }
        public string Certificado { get; set; }
        public DateTime? FechaHoraAprobacion { get; set; }

        public static IntentoModel FromSlot(Intento objIntento)
        {
            try
            {
                IntentoModel objIntentoModel = new IntentoModel();

                objIntentoModel.IdIntento = objIntento.IdIntento;
                objIntentoModel.IdInscripcion = objIntento.IdInscripcion;
                objIntentoModel.Nota = objIntento.Nota;
                objIntentoModel.NotaMaxima = objIntento.NotaMaxima;
                objIntentoModel.Aprobado = objIntento.Aprobado;
                objIntentoModel.Certificado = objIntento.Certificado;
                objIntentoModel.FechaHoraAprobacion = objIntento.FechaHoraAprobacion;

                return objIntentoModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}