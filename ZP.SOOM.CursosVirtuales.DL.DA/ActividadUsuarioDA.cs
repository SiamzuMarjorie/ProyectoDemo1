using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class ActividadUsuarioDA
    {
        public int RegistrarFechaHoraIngresoCurso(ActividadUsuario objActividadUsuario)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.ActividadUsuario.Add(objActividadUsuario);
                objModel.SaveChanges();

                return objActividadUsuario.IdActividadUsuario;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistrarVisita(ActividadUsuario objActividadUsuario)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.ActividadUsuario.Add(objActividadUsuario);
                objModel.SaveChanges();

                return objActividadUsuario.IdActividadUsuario;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<ActividadUsuario> ListarActividadUsuario()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.ActividadUsuario;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<ActividadUsuario> ListarActividadUsuarioCurso()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.ActividadUsuario.Where(a => a.TipoActividad == Constants.TipoActividad.CURSO);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
