using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class ContenidoCursoDA
    {
        public IQueryable<ContenidoCurso> ListarContenidoCurso()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.ContenidoCurso;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<ContenidoCurso> ListarContenidoCurso(int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.ContenidoCurso.Where(x => x.IdCurso == IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ContenidoCurso ObtenerContenidoCurso(int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.ContenidoCurso.SingleOrDefault(x => x.IdCurso == IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarContenidoCurso(ContenidoCurso objContenidoCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.ContenidoCurso.Add(objContenidoCurso);
                objModel.SaveChanges();
                return objContenidoCurso.IdContenidoCurso;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarContenidoCurso(ContenidoCurso objContenidoCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                ContenidoCurso objContenidoCursoBD = objModel.ContenidoCurso.SingleOrDefault(x => x.IdContenidoCurso == objContenidoCurso.IdContenidoCurso);
                objContenidoCursoBD.IdContenidoCurso = objContenidoCurso.IdContenidoCurso;
                objContenidoCursoBD.IdCurso = objContenidoCurso.IdCurso;
                if (objContenidoCurso.Archivo != null)
                    objContenidoCursoBD.Archivo = objContenidoCurso.Archivo;
                objContenidoCursoBD.TipoArchivo = objContenidoCurso.TipoArchivo;
                objContenidoCursoBD.Nombre = objContenidoCurso.Nombre;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void EliminarContenidoCurso(int IdContenidoCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                ContenidoCurso objContenidoCursoBD = objModel.ContenidoCurso.SingleOrDefault(x => x.IdContenidoCurso == IdContenidoCurso);
                objModel.ContenidoCursoInscripcion.RemoveRange(objContenidoCursoBD.ContenidoCursoInscripcion);
                objModel.ContenidoCurso.Remove(objContenidoCursoBD);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ContenidoCurso ObtenerContenidoCursoPorIdContenidoCurso(int IdContenidoCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                ContenidoCurso objContenidoCursoBD = objModel.ContenidoCurso.FirstOrDefault(x => x.IdContenidoCurso == IdContenidoCurso);
                return objContenidoCursoBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
