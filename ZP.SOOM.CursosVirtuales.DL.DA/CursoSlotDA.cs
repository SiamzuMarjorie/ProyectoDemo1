using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class CursoSlotDA
    {
        public IQueryable<CursoSlot> ListarCursoSlot()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListarCursoSlotPorAno(int? Ano)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.Where(cs => cs.Ano == Ano && cs.Curso != null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<int> ListarAnos()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.Select(x => x.Ano).Distinct().OrderBy(x => x).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListarCursoSlotIngresados(int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.Where(c => c.Estado == Constants.Cursos.Estado.ACTIVO && c.FechaHoraFin > DateTimeHelper.PeruDateTime);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListarCursoSlotActivo()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.Where(a => a.Estado == Constants.Cursos.Estado.ACTIVO && a.FechaHoraFin > DateTimeHelper.PeruDateTime && a.IdCurso != null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlot(int IdCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.Where(x => x.IdCursoSlot == IdCursoSlot).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotXIdSlot(int IdSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.Where(x => x.Slot.Codigo == IdSlot && x.Estado != Constants.Cursos.Estado.HISTORIAL && x.IdCurso != null && x.FechaHoraFin > DateTimeHelper.PeruDateTime).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public IQueryable<CursoSlot> ObtenerListaCursoSlotXIdSlot(int IdSlot, int[] IdsCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();

                return objModel.CursoSlot.Where(x => x.Slot.IdSlot == IdSlot && x.Estado == Constants.Cursos.Estado.ACTIVO && x.IdCurso != null && x.FechaHoraFin > DateTimeHelper.PeruDateTime && x.FechaHoraInicio < DateTimeHelper.PeruDateTime && IdsCurso.Any(g => g == x.IdCurso));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListaCursoSlotXIdSlot(int IdSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.Where(x => x.Slot.IdSlot == IdSlot && x.Estado == Constants.Cursos.Estado.ACTIVO && x.IdCurso != null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public IQueryable<CursoSlot> ListarCursoSlotAsignado(int IdSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.Where(c => c.IdSlot == IdSlot && c.Estado != Constants.Cursos.Estado.HISTORIAL);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerOrdenCursoSlot(int Orden, int IdSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.CursoSlot.FirstOrDefault(x => x.IdSlot == IdSlot && x.Orden == Orden);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void ActualizarCursoSlotDependiente(int IdSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Slot objSlot = objModel.Slot.FirstOrDefault(s => s.IdSlot == IdSlot);
                List<CursoSlot> lstCursoSlot = objSlot.CursoSlot.Where(cs => cs.Estado != Constants.Cursos.Estado.HISTORIAL).OrderByDescending(cs => cs.Orden).ToList();
                if (objSlot.CursosDependientes == true)
                {
                    CursoSlot objUltimoCursoSlot = lstCursoSlot.FirstOrDefault();
                    if (objUltimoCursoSlot != null)
                    {
                        lstCursoSlot.Remove(objUltimoCursoSlot);
                        foreach (CursoSlot objCursoSlot in lstCursoSlot)
                        {
                            objCursoSlot.IdCursoSlotDependiente = objUltimoCursoSlot.IdCursoSlot;
                        }
                        objUltimoCursoSlot.IdCursoSlotDependiente = null;
                    }
                    
                }
                else
                {
                    foreach (CursoSlot objCursoSlot in lstCursoSlot)
                    {
                        objCursoSlot.IdCursoSlotDependiente = null;
                    }
                }
                objModel.SaveChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarOrdenCursoSlot(int IdCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlot = objModel.CursoSlot.FirstOrDefault(x => x.IdCursoSlot == IdCursoSlot);
                if (objCursoSlot != null)
                {
                    if (objCursoSlot.Orden == 1)
                    {
                        foreach(CursoSlot objCursoSlotOrden in objModel.CursoSlot.Where(cs => cs.IdCursoSlot != IdCursoSlot && cs.IdSlot == objCursoSlot.IdSlot)){
                            objCursoSlotOrden.Orden = objCursoSlotOrden.Orden - 1;
                        }
                    }
                    else if (objCursoSlot.Orden > 1)
                    {
                        foreach (CursoSlot objCursoSlotOrden in objModel.CursoSlot.Where(cs => cs.IdCursoSlot != IdCursoSlot && cs.IdSlot == objCursoSlot.IdSlot))
                        {
                            if (objCursoSlot.Orden < objCursoSlotOrden.Orden)
                                objCursoSlotOrden.Orden = objCursoSlotOrden.Orden - 1;
                        }
                    }

                    objModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarCursoSlot(CursoSlot objCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.CursoSlot.Add(objCursoSlot);
                objModel.SaveChanges();
                return objCursoSlot.IdCursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarCursoSlot(CursoSlot objCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlotBD = objModel.CursoSlot.SingleOrDefault(x => x.IdCursoSlot == objCursoSlot.IdCursoSlot);
                objCursoSlotBD.IdCurso = objCursoSlot.IdCurso;
                objCursoSlotBD.IdSlot = objCursoSlot.IdSlot;
                objCursoSlotBD.FechaHoraFin = objCursoSlot.FechaHoraFin;
                objCursoSlotBD.FechaHoraInicio = objCursoSlot.FechaHoraInicio;
                objCursoSlotBD.Intentos = objCursoSlot.Intentos;
                objCursoSlotBD.Visible = objCursoSlot.Visible;
                objCursoSlotBD.Estado = objCursoSlot.Estado;
                objCursoSlotBD.Titulo = objCursoSlot.Titulo;
                if (objCursoSlotBD.FechaHoraRegistro == null)
                    objCursoSlotBD.FechaHoraRegistro = objCursoSlot.FechaHoraRegistro;
                objCursoSlotBD.FechaHoraUltimaActualizacion = objCursoSlot.FechaHoraUltimaActualizacion;
                objCursoSlotBD.Orden = objCursoSlot.Orden;
                objCursoSlotBD.IdCursoSlotDependiente = objCursoSlot.IdCursoSlotDependiente;
                objCursoSlotBD.Ano = objCursoSlot.Ano;
                objCursoSlotBD.Horas = objCursoSlot.Horas;
                objCursoSlotBD.Puntos = objCursoSlot.Puntos;
                objCursoSlotBD.Codigo = objCursoSlot.Codigo;
                objCursoSlotBD.Descripcion = objCursoSlot.Descripcion;
                objCursoSlotBD.NombreSlot = objCursoSlot.NombreSlot;
                objModel.SaveChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarCursoSlot(int IdCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlotBD = objModel.CursoSlot.SingleOrDefault(x => x.IdCursoSlot == IdCursoSlot);
                objModel.CursoSlot.Remove(objCursoSlotBD);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotPorCursoActivo(int? IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlot = objModel.CursoSlot.Where(x => x.IdCurso == IdCurso && x.Estado == Constants.Cursos.Estado.ACTIVO && x.FechaHoraInicio <= DateTimeHelper.PeruDateTime && x.FechaHoraFin >= DateTimeHelper.PeruDateTime).FirstOrDefault();
                return objCursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotActivoPorCodigoCurso(string Codigo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlot = objModel.CursoSlot.Where(x => x.Curso.Codigo == Codigo && x.Estado == Constants.Cursos.Estado.ACTIVO && x.FechaHoraFin > DateTimeHelper.PeruDateTime).FirstOrDefault();
                return objCursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> FiltrarCursoSlot(string Tipo, string Valor, string Estado)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<CursoSlot> lstCursoSlotFiltrado = objModel.CursoSlot;
                if (!string.IsNullOrEmpty(Estado) && Estado == Constants.Cursos.Estado.HISTORIAL)
                {
                    lstCursoSlotFiltrado = objModel.CursoSlot.Where(x => x.Estado == Constants.Cursos.Estado.HISTORIAL);
                }
                if (!string.IsNullOrEmpty(Tipo) && Tipo == Constants.Cursos.Filtro.NOMBRE && !string.IsNullOrEmpty(Valor))
                {
                    lstCursoSlotFiltrado = lstCursoSlotFiltrado.Where(x => x.Titulo.Contains(Valor));
                }
                if (!string.IsNullOrEmpty(Tipo) && Tipo == Constants.Cursos.Filtro.CODIGO && !string.IsNullOrEmpty(Valor))
                {
                    lstCursoSlotFiltrado = lstCursoSlotFiltrado.Where(x => x.Codigo.Contains(Valor));
                }
                return lstCursoSlotFiltrado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotPorIdCurso(int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlot = objModel.CursoSlot.OrderByDescending(cs => cs.IdCursoSlot).FirstOrDefault(x => x.IdCurso == IdCurso);
                return objCursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotAsignadoPorIdCurso(int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlot = objModel.CursoSlot.FirstOrDefault(cs => cs.IdCurso == IdCurso && cs.Estado != Constants.Cursos.Estado.HISTORIAL);
                return objCursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarCursoSlotEstadoHistorial(int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<CursoSlot> lstCursoSlotBD = objModel.CursoSlot.Where(x => x.IdCurso == IdCurso && x.Estado == Constants.Cursos.Estado.ACTIVO);
                if (lstCursoSlotBD.Count() > 0)
                {
                    foreach (CursoSlot objCursoSlot in lstCursoSlotBD)
                    {
                        objCursoSlot.Estado = Constants.Cursos.Estado.HISTORIAL;
                    }
                }
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotActivo(int IdCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlot = objModel.CursoSlot.Where(x => x.IdCursoSlot == IdCursoSlot).FirstOrDefault();
                return objCursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotActivoPorIdCurso(int IdCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                CursoSlot objCursoSlot = objModel.CursoSlot.Where(x => x.IdCurso == IdCurso && x.Estado == Constants.Cursos.Estado.ACTIVO && x.FechaHoraFin >= DateTimeHelper.PeruDateTime && x.FechaHoraInicio <= DateTimeHelper.PeruDateTime).FirstOrDefault();
                return objCursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
