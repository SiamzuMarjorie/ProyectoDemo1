using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class SuscripcionNotificacionDA
    {
        public IQueryable<SuscripcionNotificacion> ListarSuscripcionesNotificacionUsuario(int IdUsuario)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.SuscripcionNotificacion.Where(s => s.IdUsuario == IdUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertarSuscripcionNotificacion(SuscripcionNotificacion objSuscripcionNotificacion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                SuscripcionNotificacion objSuscripcionNotificacionBd = objModel.SuscripcionNotificacion.FirstOrDefault(s => s.IdUsuario == objSuscripcionNotificacion.IdUsuario && s.Endpoint == objSuscripcionNotificacion.Endpoint && s.PS56DH == objSuscripcionNotificacion.PS56DH && s.Auth == objSuscripcionNotificacion.Auth);
                if (objSuscripcionNotificacionBd == null)
                {
                    objModel.SuscripcionNotificacion.Add(objSuscripcionNotificacion);
                    objModel.SaveChanges();
                    return objSuscripcionNotificacion.IdSuscripcionNotificacion;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarSuscripcionNotificacion(SuscripcionNotificacion objSuscripcionNotificacion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                SuscripcionNotificacion objSuscripcionNotificacionBd = objModel.SuscripcionNotificacion.FirstOrDefault(s => s.IdUsuario == objSuscripcionNotificacion.IdUsuario && s.Endpoint == objSuscripcionNotificacion.Endpoint && s.PS56DH == objSuscripcionNotificacion.PS56DH && s.Auth == objSuscripcionNotificacion.Auth);
                objModel.SuscripcionNotificacion.Remove(objSuscripcionNotificacionBd);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EliminarSuscripcionNotificacionUsuario(int IdUsuario)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IEnumerable<SuscripcionNotificacion> lstSuscripcionNotificacion = objModel.SuscripcionNotificacion.Where(s => s.IdUsuario == IdUsuario);
                objModel.SuscripcionNotificacion.RemoveRange(lstSuscripcionNotificacion);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
