using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class GrupoOcupacionalModel
    {
        public int IdGrupoOcupacional { get; set; }
        public string Nombre { get; set; }

        public static GrupoOcupacionalModel FromGrupoOcupacional(GrupoOcupacional objGrupoOcupacional)
        {
            try
            {
                GrupoOcupacionalModel objGrupoOcupacionalModel = new GrupoOcupacionalModel();
                objGrupoOcupacionalModel.IdGrupoOcupacional = objGrupoOcupacional.IdGrupoOcupacional;
                objGrupoOcupacionalModel.Nombre = objGrupoOcupacional.Nombre;
                return objGrupoOcupacionalModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}