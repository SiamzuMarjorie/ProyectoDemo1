using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class MaterialDA
    {
        public IQueryable<Material> ListarMaterial()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Material;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Material ObtenerMaterial(int IdMaterial)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Material.SingleOrDefault(x => x.IdMaterial == IdMaterial);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarMaterial(Material objMaterial)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.Material.Add(objMaterial);
                objModel.SaveChanges();
                return objMaterial.IdMaterial;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarMaterial(Material objMaterial)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Material objMaterialBD = objModel.Material.Find(objMaterial.IdMaterial);
                objMaterialBD.IdMaterial = objMaterial.IdMaterial;
                objMaterialBD.IdCurso = objMaterial.IdCurso;
                objMaterialBD.Nombre = objMaterial.Nombre;
                objMaterialBD.TipoMaterial = objMaterial.TipoMaterial;
                objMaterialBD.Obligatorio = objMaterial.Obligatorio;
                if (objMaterial.Archivo != null)
                    objMaterialBD.Archivo = objMaterial.Archivo;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarMaterial(int IdMaterial)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Material objMaterialBD = objModel.Material.Find(IdMaterial);
                objModel.MaterialInscripcion.RemoveRange(objMaterialBD.MaterialInscripcion);
                objModel.Material.Remove(objMaterialBD);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
