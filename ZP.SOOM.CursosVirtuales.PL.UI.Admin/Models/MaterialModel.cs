using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZP.SOOM.CursosVirtuales.DL.DM;


namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class MaterialModel
    {
        public int IdMaterial { get; set; }
        public int IdCurso { get; set; }
        public string Nombre { get; set; }
        public string TipoMaterial { get; set; }
        public string Archivo { get; set; }
        public bool? Obligatorio { get; set; }

        public static MaterialModel FromMaterial(Material objMaterial)
        {
            try
            {
                MaterialModel objMaterialModel = new MaterialModel();
                objMaterialModel.IdMaterial = objMaterial.IdMaterial;
                objMaterialModel.IdCurso = objMaterial.IdCurso;
                objMaterialModel.Nombre = objMaterial.Nombre;
                objMaterialModel.TipoMaterial = objMaterial.TipoMaterial;
                objMaterialModel.Archivo = objMaterial.Archivo;
                objMaterialModel.Obligatorio = objMaterial.Obligatorio;
                return objMaterialModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Material ToMaterial()
        {
            try
            {
                Material objMaterial = new Material();
                objMaterial.IdMaterial = IdMaterial;
                objMaterial.IdCurso = IdCurso;
                objMaterial.Nombre = Nombre;
                objMaterial.TipoMaterial = TipoMaterial;
                objMaterial.Archivo = Archivo;
                objMaterial.Obligatorio = Obligatorio;
                return objMaterial;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
