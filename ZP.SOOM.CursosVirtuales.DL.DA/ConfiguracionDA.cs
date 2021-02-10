using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class ConfiguracionDA
    {
        public IQueryable<Configuracion> ListarConfiguracion()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Configuracion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Configuracion ObtenerConfiguracion(int IdConfiguracion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Configuracion objConfiguracioBD = objModel.Configuracion.Where(x => x.IdConfiguracion == IdConfiguracion).FirstOrDefault();
                return objConfiguracioBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Configuracion ObtenerConfiguracionPorNombre(string Nombre)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Configuracion objConfiguracioBD = objModel.Configuracion.Where(x => x.Nombre == Nombre).FirstOrDefault();
                return objConfiguracioBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarConfiguracion(Configuracion objConfiguracion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.Configuracion.Add(objConfiguracion);
                objModel.SaveChanges();
                return objConfiguracion.IdConfiguracion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActulizarConfiguracion(Configuracion objConfiguracion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Configuracion objConfiguracionBD = objModel.Configuracion.Where(x => x.IdConfiguracion == objConfiguracion.IdConfiguracion).FirstOrDefault();
                objConfiguracionBD.IdConfiguracion = objConfiguracion.IdConfiguracion;
                objConfiguracionBD.Nombre = objConfiguracion.Nombre;
                objConfiguracionBD.Valor = objConfiguracion.Valor;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarConfiguracionPorCodigo(string Codigo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Configuracion objConfiguracion = objModel.Configuracion.Where(x => x.Nombre == Codigo).FirstOrDefault();
                if (objConfiguracion != null)
                {
                    objModel.Configuracion.Remove(objConfiguracion);
                    objModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Configuracion ObtenerConfiguracion(string Codigo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Configuracion objConfiguracion = objModel.Configuracion.Where(x => x.Nombre == Codigo).FirstOrDefault();
                return objConfiguracion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
