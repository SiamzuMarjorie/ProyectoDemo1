using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class IntentoDA
    {

        public int GuardarIntento(Intento objIntento)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                if (objIntento.IdIntento == 0)
                {
                    objModel.Intento.Add(objIntento);
                }
                else
                {
                    Intento objIntentoBD = objModel.Intento.FirstOrDefault(i => i.IdIntento == objIntento.IdIntento);
                    objIntentoBD.Aprobado = objIntento.Aprobado;
                    objIntentoBD.Certificado = objIntento.Certificado;
                    objIntentoBD.FechaHoraAprobacion = objIntento.FechaHoraAprobacion;
                    if (objIntentoBD.FechaHoraRegistro == null)
                        objIntentoBD.FechaHoraRegistro = DateTimeHelper.PeruDateTime;
                    objIntentoBD.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                    objIntentoBD.Nota = objIntento.Nota;
                    objIntentoBD.Terminado = objIntento.Terminado;
                    objIntentoBD.NotaMaxima = objIntento.NotaMaxima;
                    objIntentoBD.Puntaje = objIntento.Puntaje;
                }
                objModel.SaveChanges();
                return objIntento.IdIntento;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void RegistrarDescargaCertificado(int IdIntento)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Intento objIntento = objModel.Intento.FirstOrDefault(x => x.IdIntento == IdIntento);
                objIntento.FechaHoraCertificadoDescargado = DateTimeHelper.PeruDateTime;
                objIntento.Inscripcion.FechaHoraDescargaCertificado = DateTimeHelper.PeruDateTime;
                objModel.SaveChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int ObtenerMayorIntento(int IdInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Intento.Where(i => i.IdInscripcion == IdInscripcion).Max(x => (int?) x.NumeroIntento) ?? 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int ObtenerMayorPuntaje(int IdInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Intento.Where(i => i.IdInscripcion == IdInscripcion && i.Puntaje != null).Max(x => (int?)x.Puntaje) ?? 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
