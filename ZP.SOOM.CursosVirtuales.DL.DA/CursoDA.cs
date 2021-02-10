using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.Util;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class CursoDA
    {
        public IQueryable<Curso> ListarCurso()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Curso;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Curso> ListarCursosNoAsignados()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();

                return objModel.Curso.Where(a => a.Estado == Constants.Cursos.Estado.ACTIVO && a.CursoSlot.All(e => e.Estado != Constants.Cursos.Estado.ACTIVO && e.Estado != Constants.Cursos.Estado.INACTIVO));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Curso> FiltarCurso(string Tipo, string Valor, string estado)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<Curso> lstCursoFiltrado = objModel.Curso;
                if (estado == Constants.Cursos.Estado.ACTIVO)
                {
                    lstCursoFiltrado = lstCursoFiltrado.Where(c => c.Estado == Constants.Cursos.Estado.ACTIVO);
                }
                else
                {
                    lstCursoFiltrado = lstCursoFiltrado.Where(c => c.Estado == Constants.Cursos.Estado.ARCHIVADO);
                }
                if (!string.IsNullOrEmpty(Tipo) && Tipo == Constants.Cursos.Filtro.NOMBRE && !string.IsNullOrEmpty(Valor))
                {
                    lstCursoFiltrado = lstCursoFiltrado.Where(x => x.Titulo.Contains(Valor));
                }
                if (!string.IsNullOrEmpty(Tipo) && Tipo == Constants.Cursos.Filtro.CODIGO && !string.IsNullOrEmpty(Valor))
                {
                    lstCursoFiltrado = lstCursoFiltrado.Where(x => x.Codigo.Contains(Valor));
                }
                return lstCursoFiltrado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Curso ObtenerCurso(int? IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Curso.SingleOrDefault(x => x.IdCurso == IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public Curso ObtenerCursoAsignado(int? IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Curso.FirstOrDefault(x => x.IdCurso == IdCurso && x.CursoSlot.All(e => e.Estado == Constants.Cursos.Estado.ACTIVO && e.Estado == Constants.Cursos.Estado.INACTIVO));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarCurso(Curso objCurso)
        {
            SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
            using (var transaction = objModel.Database.BeginTransaction())
            {
                try
                {
                    objModel.Curso.Add(objCurso);
                    objModel.SaveChanges();
                    transaction.Commit();
                    return objCurso.IdCurso;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public void ActualizarCurso(Curso objCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Curso objCursoBD = objModel.Curso.Find(objCurso.IdCurso);
                objCursoBD.IdCurso = objCurso.IdCurso;
                objCursoBD.Horas = objCurso.Horas;
                objCursoBD.Codigo = objCurso.Codigo;
                objCursoBD.Puntos = objCurso.Puntos;
                objCursoBD.Titulo = objCurso.Titulo;
                if (objCurso.UrlExamen != null)
                    objCursoBD.UrlExamen = objCurso.UrlExamen;
                if (objCurso.Certificado != null)
                    objCursoBD.Certificado = objCurso.Certificado;
                objCursoBD.Descripcion = objCurso.Descripcion;
                objCursoBD.Estado = objCurso.Estado;
                objCursoBD.EmiteCertificado = objCurso.EmiteCertificado;
                objCursoBD.TieneExamen = objCurso.TieneExamen;
                if (objCurso.Imagen != null)
                    objCursoBD.Imagen = objCurso.Imagen;
                objCursoBD.Eliminado = false;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void EliminarCurso(int IdCurso)
        {
            SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
            using (var transaction = objModel.Database.BeginTransaction())
            {
                try
                {
                    Curso objCursoBD = objModel.Curso.SingleOrDefault(x => x.IdCurso == IdCurso);
                    foreach (GrupoCurso objGrupoCurso in objCursoBD.GrupoCurso)
                    {
                        objGrupoCurso.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime; 
                    }
                    objCursoBD.Estado = Constants.Cursos.Estado.HISTORIAL;
                    objCursoBD.Eliminado = true;
                    objModel.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Curso ObtenerCursoPorCodigo(string Codigo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Curso.FirstOrDefault(x => x.Codigo == Codigo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<int> ListarIdsCursoXTrabajador(int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Trabajador objTrabajador = objModel.Trabajador.FirstOrDefault(t => t.IdTrabajador == IdTrabajador);
                return objTrabajador.GrupoTrabajador.Where(gt => gt.FechaHoraDesasignacion == null).SelectMany(g => g.Grupo.GrupoCurso.Where(gc=>gc.FechaHoraDesasignacion == null).Select(gc => gc.IdCurso));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
