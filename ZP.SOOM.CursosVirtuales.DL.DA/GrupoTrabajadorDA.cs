using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class GrupoTrabajadorDA
    {
        public IQueryable<GrupoTrabajador> ListarGrupo(int IdGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoTrabajador.Where(g => g.IdGrupo == IdGrupo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarFechaDesasignacion(int IdGrupoTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                GrupoTrabajador objGrupoTrabajador = objModel.GrupoTrabajador.FirstOrDefault(g => g.IdGrupoTrabajador == IdGrupoTrabajador);
                objGrupoTrabajador.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime;

                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AsignarGrupoATrabajador(int IdTrabajador, int[] IdGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Trabajador objTrabajador = objModel.Trabajador.FirstOrDefault(t => t.IdTrabajador == IdTrabajador);
                List<GrupoTrabajador> lstGrupoTrabajador = objModel.GrupoTrabajador.Where(gt => gt.IdTrabajador == IdTrabajador && gt.Grupo.Nombre != "Todos" && gt.FechaHoraDesasignacion == null).ToList();

                if (IdGrupo != null)
                {
                    foreach (GrupoTrabajador objGrupoTrabajador in lstGrupoTrabajador)
                    {
                        if (!IdGrupo.Contains(objGrupoTrabajador.IdGrupo))
                        {
                            objGrupoTrabajador.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime;
                        }
                    }
                    foreach (int Grupos in IdGrupo.Where(g => lstGrupoTrabajador.All(s => s.IdGrupo != g)))
                    {
                        GrupoTrabajador objGrupoTrabajador = new GrupoTrabajador();
                        objGrupoTrabajador.IdTrabajador = IdTrabajador;
                        objGrupoTrabajador.IdGrupo = Grupos;
                        objGrupoTrabajador.FechaHoraRegistro = DateTimeHelper.PeruDateTime;

                        objModel.GrupoTrabajador.Add(objGrupoTrabajador);
                    }
                }
                else
                {
                    foreach (GrupoTrabajador objGrupoTrabajador in lstGrupoTrabajador)
                    {
                        objGrupoTrabajador.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime;
                    }
                }
                objModel.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
