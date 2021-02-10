using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class GrupoDA
    {
        public int GuardarGrupo(Grupo objGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                if (objGrupo.IdGrupo == 0)
                {
                    objModel.Grupo.Add(objGrupo);
                }
                else
                {
                    Grupo objGrupoBD = objModel.Grupo.FirstOrDefault(g => g.IdGrupo == objGrupo.IdGrupo);
                    objGrupoBD.Nombre = objGrupo.Nombre;
                    objGrupoBD.Descripcion = objGrupo.Descripcion;
                    objGrupoBD.Eliminado = false;
                }
                objModel.SaveChanges();
                return objGrupo.IdGrupo;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarGrupo(int IdGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Grupo objGrupo = objModel.Grupo.FirstOrDefault(g => g.IdGrupo == IdGrupo);
                objGrupo.Eliminado = true;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public void EliminarTrabajadoresGrupo(int IdGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                List<GrupoTrabajador> lstGrupoTrabajador = objModel.GrupoTrabajador.Where(g => g.IdGrupo == IdGrupo).ToList();
                foreach (GrupoTrabajador objGrupoTrabajador in lstGrupoTrabajador)
                {
                    objGrupoTrabajador.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime;
                }
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void EliminarTrabajadorGrupo(int IdGrupo, int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                GrupoTrabajador objGrupoTrabajador = objModel.GrupoTrabajador.FirstOrDefault(gt => gt.IdGrupo == IdGrupo && gt.IdTrabajador == IdTrabajador);
                objGrupoTrabajador.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQueryable<Grupo> ListarGrupo()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Grupo.Where(g => !g.Eliminado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> ListarNombreGrupo()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<string> lstGrupo = objModel.Grupo.Where(g => g.Nombre != null && !g.Eliminado).Select(g => g.Nombre).Distinct();
                return lstGrupo.OrderBy(g => g).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Grupo ObtenerGrupo(int IdGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Grupo.FirstOrDefault(g => g.IdGrupo == IdGrupo && !g.Eliminado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
