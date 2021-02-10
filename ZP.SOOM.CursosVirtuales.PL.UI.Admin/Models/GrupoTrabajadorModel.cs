using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class GrupoTrabajadorModel
    {
        public int IdGrupoTrabajador { get; set; }
        public int IdGrupo { get; set; }
        public int IdTrabajador { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
        public DateTime? FechaHoraDesasignacion { get; set; }

        public static GrupoTrabajadorModel FromGrupoTrabajador(GrupoTrabajador objGrupoTrabajador)
        {
            try
            {
                GrupoTrabajadorModel objGrupoTrabajadorModel = new GrupoTrabajadorModel();
                objGrupoTrabajadorModel.IdGrupoTrabajador = objGrupoTrabajador.IdGrupoTrabajador;
                objGrupoTrabajadorModel.IdGrupo = objGrupoTrabajador.IdGrupo;
                objGrupoTrabajadorModel.IdTrabajador = objGrupoTrabajador.IdTrabajador;
                objGrupoTrabajadorModel.FechaHoraRegistro = objGrupoTrabajador.FechaHoraRegistro;
                objGrupoTrabajadorModel.FechaHoraDesasignacion = objGrupoTrabajador.FechaHoraDesasignacion;

                return objGrupoTrabajadorModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public GrupoTrabajador ToGrupoTrabajador(GrupoTrabajadorModel objGrupoTrabajadorModel)
        {
            try
            {
                GrupoTrabajador objGrupoTrabajador = new GrupoTrabajador();
                objGrupoTrabajador.IdGrupoTrabajador = objGrupoTrabajadorModel.IdGrupoTrabajador;
                objGrupoTrabajador.IdGrupo = objGrupoTrabajadorModel.IdGrupo;
                objGrupoTrabajador.IdTrabajador = objGrupoTrabajadorModel.IdTrabajador;
                objGrupoTrabajador.FechaHoraRegistro = objGrupoTrabajadorModel.FechaHoraRegistro;
                objGrupoTrabajador.FechaHoraDesasignacion = objGrupoTrabajadorModel.FechaHoraDesasignacion;

                return objGrupoTrabajador;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }

}