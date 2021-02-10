using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class GrupoOcupacionalDA
    {
        public int InsertarGrupoOcupacional(GrupoOcupacional objGrupoOcupacional)
        {

            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.GrupoOcupacional.Add(objGrupoOcupacional);
                objModel.SaveChanges();
                return objGrupoOcupacional.IdGrupoOcupacional;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IQueryable<GrupoOcupacional> ListarGrupoOcupacional()
        {

            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoOcupacional;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<string> ListarGrupoOcupacionales()
        {

            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<string> lstGrupoOcupacional = objModel.GrupoOcupacional.Where(go => go.Nombre != null).Select(go => go.Nombre).Distinct();
                return lstGrupoOcupacional.OrderBy(go => go).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public GrupoOcupacional ObtenerGrupoOcupacional(int IdGrupoOcupacional)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoOcupacional.SingleOrDefault(t => t.IdGrupoOcupacional == IdGrupoOcupacional);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public GrupoOcupacional ObtenerGrupoOcupacional(string Nombre)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoOcupacional.SingleOrDefault(t => t.Nombre == Nombre);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public GrupoOcupacional ObtenerNombreGrupoOcupacional(string NombreGrupoOcupacional)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoOcupacional.FirstOrDefault(t => t.Nombre == NombreGrupoOcupacional);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void EliminarGrupoOcupacional(int IdGrupoOcupacional)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                GrupoOcupacional objGrupoOcupacional = objModel.GrupoOcupacional.SingleOrDefault(t => t.IdGrupoOcupacional == IdGrupoOcupacional);
                objGrupoOcupacional.Trabajador.Clear();
                objModel.GrupoOcupacional.Remove(objGrupoOcupacional);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
