using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class MaterialInscripcionDA
    {
        public IQueryable<MaterialInscripcion> ListarMaterialInscripcion()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.MaterialInscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public MaterialInscripcion ObtenerMaterialInscripcion(int IdMaterialInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                MaterialInscripcion objMaterialInscripcionBD = objModel.MaterialInscripcion.SingleOrDefault(x => x.IdMaterialInscripcion == IdMaterialInscripcion);
                return objMaterialInscripcionBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<MaterialInscripcion> ObtenerMaterialObligatorio(int IdInscripcion, int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.MaterialInscripcion.Where(v => v.Material.IdCurso == IdCurso && v.IdInscripcion == IdInscripcion && v.Visto != true && v.Obligatorio == true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public MaterialInscripcion ObtenerMaterialInscripcionPorIdMaterialYIdInscripcion(int IdMaterial, int IdInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                MaterialInscripcion objMaterialInscripcionBD = objModel.MaterialInscripcion.FirstOrDefault(x => x.IdMaterial == IdMaterial && x.IdInscripcion == IdInscripcion);
                return objMaterialInscripcionBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public MaterialInscripcion ObtenerMaterialInscripcionPorIdMaterialYIdCurso(int IdMaterial, int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                MaterialInscripcion objMaterialInscripcionBD = objModel.MaterialInscripcion.FirstOrDefault(x => x.IdMaterial == IdMaterial && x.IdCurso == IdCurso);
                return objMaterialInscripcionBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int IngresarMaterialInscripcion(MaterialInscripcion objMaterialInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                if (objMaterialInscripcion.IdMaterialInscripcion == 0)
                {
                    objModel.MaterialInscripcion.Add(objMaterialInscripcion);
                }
                else
                {
                    MaterialInscripcion objMaterialInscripcionBD = objModel.MaterialInscripcion.FirstOrDefault(c => c.IdMaterialInscripcion == objMaterialInscripcion.IdMaterialInscripcion);
                    objMaterialInscripcionBD.IdMaterial = objMaterialInscripcion.IdMaterial;
                    objMaterialInscripcionBD.IdInscripcion = objMaterialInscripcion.IdInscripcion;
                    objMaterialInscripcionBD.Visto = objMaterialInscripcion.Visto;
                    objMaterialInscripcionBD.IdCurso = objMaterialInscripcion.IdCurso;
                    objMaterialInscripcionBD.Nombre = objMaterialInscripcion.Nombre;
                    objMaterialInscripcionBD.TipoMaterial = objMaterialInscripcion.TipoMaterial;
                    objMaterialInscripcionBD.Archivo = objMaterialInscripcion.Archivo;
                    objMaterialInscripcionBD.Obligatorio = objMaterialInscripcion.Obligatorio;
                }
                objModel.SaveChanges();
                return objMaterialInscripcion.IdMaterialInscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualiazarVistoMaterialInscripcion(int IdMaterialInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                MaterialInscripcion objMaterialInscripcion = objModel.MaterialInscripcion.FirstOrDefault(m => m.IdMaterialInscripcion == IdMaterialInscripcion);
                if (objMaterialInscripcion.Visto == false)
                {
                    objMaterialInscripcion.Visto = true;
                }
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarMaterialInscripcion(MaterialInscripcion objMaterialInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                MaterialInscripcion objMaterialInscripcionBD = objModel.MaterialInscripcion.FirstOrDefault(x => x.IdMaterialInscripcion == objMaterialInscripcion.IdMaterialInscripcion);
                objMaterialInscripcionBD.IdMaterial = objMaterialInscripcion.IdMaterial;
                objMaterialInscripcionBD.IdInscripcion = objMaterialInscripcion.IdInscripcion;
                objMaterialInscripcionBD.Visto = objMaterialInscripcion.Visto;
                objMaterialInscripcionBD.IdCurso = objMaterialInscripcion.IdCurso;
                objMaterialInscripcionBD.Nombre = objMaterialInscripcion.Nombre;
                objMaterialInscripcionBD.TipoMaterial = objMaterialInscripcion.TipoMaterial;
                objMaterialInscripcionBD.Archivo = objMaterialInscripcion.Archivo;
                objMaterialInscripcionBD.Obligatorio = objMaterialInscripcion.Obligatorio;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarMaterialInscripcion(int IdMaterialInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                MaterialInscripcion objMaterialInscripcionBD = objModel.MaterialInscripcion.SingleOrDefault(x => x.IdMaterialInscripcion == IdMaterialInscripcion);
                objModel.MaterialInscripcion.Remove(objMaterialInscripcionBD);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarMaterialInscripcion(int IdMaterial, int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<MaterialInscripcion> listMaterialInscripcion = objModel.MaterialInscripcion.Where(x => x.IdMaterial == IdMaterial && x.IdCurso == IdCurso);
                objModel.MaterialInscripcion.RemoveRange(listMaterialInscripcion);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
