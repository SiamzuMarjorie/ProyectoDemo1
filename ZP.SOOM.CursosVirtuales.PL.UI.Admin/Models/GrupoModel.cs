using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class GrupoModel
    {
        public int IdGrupo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool GrupoTrabajador { get; set; }
        public bool GrupoCurso { get; set; }

        public static GrupoModel FromGrupo(Grupo objGrupo, int? IdTrabajador = null, int? IdCurso = null)
        {
            try
            {
                GrupoModel objGrupoModel = new GrupoModel();
                objGrupoModel.IdGrupo = objGrupo.IdGrupo;
                objGrupoModel.Nombre = objGrupo.Nombre;
                objGrupoModel.Descripcion = objGrupo.Descripcion;
                if (IdTrabajador != null)
                {
                    GrupoTrabajador objGrupoTrabajador = objGrupo.GrupoTrabajador.FirstOrDefault(t => t.IdTrabajador == IdTrabajador && t.FechaHoraDesasignacion == null);
                    objGrupoModel.GrupoTrabajador = objGrupoTrabajador == null ? false : objGrupoTrabajador.FechaHoraDesasignacion == null ? true : false;
                }
                if (IdCurso != null)
                {
                    GrupoCurso objGrupoCurso = objGrupo.GrupoCurso.FirstOrDefault(c => c.IdCurso == IdCurso && c.FechaHoraDesasignacion == null);
                   objGrupoModel.GrupoCurso = objGrupoCurso == null ? false : objGrupoCurso.FechaHoraDesasignacion == null? true : false;
                }
                return objGrupoModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Grupo ToGrupo(GrupoModel objGrupoModel)
        {
            try
            {
                Grupo objGrupo = new Grupo();
                objGrupo.IdGrupo = objGrupoModel.IdGrupo;
                objGrupo.Nombre = objGrupoModel.Nombre;
                objGrupo.Descripcion = objGrupoModel.Descripcion;

                return objGrupo;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}