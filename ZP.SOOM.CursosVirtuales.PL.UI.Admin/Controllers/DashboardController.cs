using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {

        public ActionResult Index()
        {
            CursoBC objCursoBC = new CursoBC();
            IQueryable<Curso> lstCurso = objCursoBC.ListarCurso().Where(c => !c.Eliminado);
            List<CursoModel> lstCursoModel = new List<CursoModel>();
            foreach (Curso objCurso in lstCurso)
            {
                lstCursoModel.Add(CursoModel.FromCurso(objCurso));
            }
            ViewBag.Cursos = lstCursoModel;
            return View();
        }

        [HttpPost]
        public ActionResult DashboardAvanceCurso(int? IdCurso, string Nivel, string Gerencia)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                List<DashboardModel> lstDashboardModel = new List<DashboardModel>();
                MapaBC objMapaBC = new MapaBC();
                UsuarioBC objUsuarioBC = new UsuarioBC();
                List<CursoSlot> lstCursoSlot = new List<CursoSlot>();
                CursoSlot objCursoSlot = new CursoSlot();
                if (IdCurso != null)
                {
                    objCursoSlot = objMapaBC.ObtenerCursoSlotPorIdCurso((int)IdCurso);
                }
                else
                {
                    lstCursoSlot = objMapaBC.ListarCursoSlotActivo().ToList();
                }
                IQueryable<Trabajador> lstTrabajador = objUsuarioBC.ListarTrabajadores();
                List<string> lstTrabajadorGerencia = lstTrabajador.Where(t => t.Gerencia != null).Select(t => t.Gerencia).Distinct().ToList();

                if (lstTrabajadorGerencia.Count > 0)
                {
                    if (Nivel == Constants.Dashboard.Nivel.GERENCIA)
                    {
                        foreach (string GerenciaBD in lstTrabajadorGerencia)
                        {
                            double? porcentajeGerencia;
                            if (IdCurso != null)
                            {
                                porcentajeGerencia = CalcularAvanceCurso(lstTrabajador.Where(t => t.Gerencia == GerenciaBD), objCursoSlot.IdCursoSlot);
                            }
                            else
                            {
                                porcentajeGerencia = CalcularAvanceCursoTotal(lstTrabajador.Where(t => t.Gerencia == GerenciaBD), lstCursoSlot);
                            }
                            if (porcentajeGerencia == null)
                                continue;
                            DashboardModel objDashboardModel = new DashboardModel();
                            objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                            objDashboardModel.Resultado = porcentajeGerencia.Value;
                            objDashboardModel.Nombre = GerenciaBD;
                            lstDashboardModel.Add(objDashboardModel);
                        }
                    }
                    else
                    {
                        double? porcentajeGerencia;
                        if (IdCurso != null)
                        {
                            porcentajeGerencia = CalcularAvanceCurso(lstTrabajador.Where(t => t.Gerencia == Gerencia), objCursoSlot.IdCursoSlot);
                        }
                        else
                        {
                            porcentajeGerencia = CalcularAvanceCursoTotal(lstTrabajador.Where(t => t.Gerencia == Gerencia), lstCursoSlot);
                        }
                        DashboardModel objDashboarGerenciadModel = new DashboardModel();
                        objDashboarGerenciadModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboarGerenciadModel.Resultado = porcentajeGerencia ?? 0;
                        objDashboarGerenciadModel.Nombre = Gerencia;
                        lstDashboardModel.Add(objDashboarGerenciadModel);

                        foreach (string Area in lstTrabajador.Where(t => t.Gerencia == Gerencia && t.Area != null).Select(t => t.Area).Distinct().ToList())
                        {
                            double? porcentajeArea;
                            if (IdCurso != null)
                            {
                                porcentajeArea = CalcularAvanceCurso(lstTrabajador.Where(t => t.Gerencia == Gerencia && t.Area == Area), objCursoSlot.IdCursoSlot);
                            }
                            else
                            {
                                porcentajeArea = CalcularAvanceCursoTotal(lstTrabajador.Where(t => t.Gerencia == Gerencia && t.Area == Area), lstCursoSlot);
                            }
                            if (porcentajeArea == null)
                                continue;
                            DashboardModel objDashboardModel = new DashboardModel();
                            objDashboardModel.Color = Constants.Dashboard.Color.CELESTE;
                            objDashboardModel.Resultado = porcentajeArea.Value;
                            objDashboardModel.Nombre = Area;
                            lstDashboardModel.Add(objDashboardModel);
                        }
                    }
                    objResultObject.Data = lstDashboardModel;
                    if (lstDashboardModel.Count > 0)
                    {
                        objResultObject.OK = true;
                    }
                    else
                    {
                        objResultObject.OK = false;
                        objResultObject.Message = "No hay data";
                    }
                }
                else
                {
                    objResultObject.OK = false;
                    objResultObject.Message = "No hay data";
                }
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.exMessage = "Ocurrio un error al cargar el dashboard de avance de cursos.";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult DashboardIngresoCursoFecha(int? IdCurso, string Nivel, int? Ano, int? Mes, int? Dia)
        {
            ResultObject objResultObject = new ResultObject();

            try
            {
                List<DashboardModel> lstDashboardModel = new List<DashboardModel>();
                UsuarioBC objUsuarioBC = new UsuarioBC();
                IQueryable<ActividadUsuario> lstActividadUsuario = objUsuarioBC.ListarActividadUsuarioCurso();
                if (IdCurso != null)
                {
                    lstActividadUsuario = lstActividadUsuario.Where(a => a.CursoSlot.IdCurso == IdCurso);
                }
                if (Nivel == Constants.Dashboard.Nivel.AÑO)
                {
                    foreach (int Year in lstActividadUsuario.Select(a => a.FechaHora.Value.Year).Distinct())
                    {
                        DateTime FechaInicio = new DateTime(Year, 1, 1);
                        DateTime FechaFin = FechaInicio.AddYears(1);

                        int Sumatoria = lstActividadUsuario.Count(a => a.FechaHora >= FechaInicio && a.FechaHora < FechaFin);

                        DashboardModel objDashboardModel = new DashboardModel();
                        objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboardModel.Resultado = Sumatoria;
                        objDashboardModel.Nombre = Year.ToString();
                        objDashboardModel.Value = Year;
                        lstDashboardModel.Add(objDashboardModel);
                    }

                    if (lstDashboardModel.Count == 0)
                    {
                        DashboardModel objDashboardModel = new DashboardModel();
                        objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboardModel.Resultado = 0;
                        objDashboardModel.Nombre = DateTimeHelper.PeruDateTime.Year.ToString();
                        objDashboardModel.Value = DateTimeHelper.PeruDateTime.Year;
                        lstDashboardModel.Add(objDashboardModel);
                    }
                }
                else if (Nivel == Constants.Dashboard.Nivel.MES)
                {
                    foreach (int Month in lstActividadUsuario.Where(a => a.FechaHora.Value.Year == Ano).Select(a => a.FechaHora.Value.Month).Distinct())
                    {
                        DateTime FechaInicio = new DateTime(Ano.Value, Month, 1);
                        DateTime FechaFin = FechaInicio.AddMonths(1);

                        int Sumatoria = lstActividadUsuario.Count(a => a.FechaHora >= FechaInicio && a.FechaHora < FechaFin);

                        DashboardModel objDashboardModel = new DashboardModel();
                        objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboardModel.Resultado = Sumatoria;
                        objDashboardModel.Nombre = DateTimeHelper.MonthToString(Month);
                        objDashboardModel.Value = Month;
                        lstDashboardModel.Add(objDashboardModel);
                    }
                    if (lstDashboardModel.Count == 0)
                    {
                        DashboardModel objDashboardModel = new DashboardModel();
                        objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboardModel.Resultado = 0;
                        objDashboardModel.Nombre = DateTimeHelper.MonthToString(DateTimeHelper.PeruDateTime.Month);
                        objDashboardModel.Value = DateTimeHelper.PeruDateTime.Month;
                        lstDashboardModel.Add(objDashboardModel);
                    }
                }
                else if (Nivel == Constants.Dashboard.Nivel.DIA)
                {
                    foreach (int Day in lstActividadUsuario.Where(a => a.FechaHora.Value.Year == Ano && a.FechaHora.Value.Month == Mes).Select(a => a.FechaHora.Value.Day).Distinct())
                    {
                        DateTime FechaInicio = new DateTime(Ano.Value, Mes.Value, Day);
                        DateTime FechaFin = FechaInicio.AddDays(1);

                        int Sumatoria = lstActividadUsuario.Count(a => a.FechaHora >= FechaInicio && a.FechaHora < FechaFin);

                        DashboardModel objDashboardModel = new DashboardModel();
                        objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboardModel.Resultado = Sumatoria;
                        objDashboardModel.Nombre = string.Format("{0}/{1}", Day.ToString("00"), Mes.Value.ToString("00"));
                        objDashboardModel.Value = Day;
                        lstDashboardModel.Add(objDashboardModel);
                    }
                    if (lstDashboardModel.Count == 0)
                    {
                        DashboardModel objDashboardModel = new DashboardModel();
                        objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboardModel.Resultado = 0;
                        objDashboardModel.Nombre = string.Format("{0}/{1}", DateTimeHelper.PeruDateTime.Day.ToString("00"), DateTimeHelper.PeruDateTime.Month.ToString("00")); ;
                        objDashboardModel.Value = DateTimeHelper.PeruDateTime.Day;
                        lstDashboardModel.Add(objDashboardModel);
                    }
                }
                else if (Nivel == Constants.Dashboard.Nivel.HORA)
                {
                    foreach (int Hour in lstActividadUsuario.Where(a => a.FechaHora.Value.Year == Ano && a.FechaHora.Value.Month == Mes && a.FechaHora.Value.Day == Dia).Select(a => a.FechaHora.Value.Hour).Distinct())
                    {
                        DateTime FechaInicio = new DateTime(Ano.Value, Mes.Value, Dia.Value, Hour, 0, 0);
                        DateTime FechaFin = FechaInicio.AddHours(1);

                        int Sumatoria = lstActividadUsuario.Count(a => a.FechaHora >= FechaInicio && a.FechaHora < FechaFin);

                        DashboardModel objDashboardModel = new DashboardModel();
                        objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboardModel.Resultado = Sumatoria;
                        objDashboardModel.Nombre = Hour.ToString("00") + ":00";
                        objDashboardModel.Value = Hour;
                        lstDashboardModel.Add(objDashboardModel);
                    }
                    if (lstDashboardModel.Count == 0)
                    {
                        DashboardModel objDashboardModel = new DashboardModel();
                        objDashboardModel.Color = Constants.Dashboard.Color.MORA;
                        objDashboardModel.Resultado = 0;
                        objDashboardModel.Nombre = DateTimeHelper.PeruDateTime.Hour.ToString("00") + ":00";
                        objDashboardModel.Value = DateTimeHelper.PeruDateTime.Hour;
                        lstDashboardModel.Add(objDashboardModel);
                    }
                }
                objResultObject.Data = lstDashboardModel;
                objResultObject.OK = true;
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.exMessage = "Ocurrio un error al cargar el dashboard de ingresos a los cursos";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        private double? CalcularAvanceCurso(IQueryable<Trabajador> lstTrabajador, int? IdCursoSlot)
        {
            try
            {
                int Total = lstTrabajador.Count();
                if (Total > 0)
                    return (double)lstTrabajador.Where(t => t.Inscripcion.FirstOrDefault(i => IdCursoSlot == null || i.IdCursoSlot == IdCursoSlot).Intento.Any(i => i.Terminado)).Count() * 100.0 / Total;
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private double? CalcularAvanceCursoTotal(IQueryable<Trabajador> lstTrabajador, List<CursoSlot> lstCursoSlot)
        {
            try
            {
                int Total = lstTrabajador.Count() * lstCursoSlot.Count();
                if (Total > 0)
                    return (double)lstCursoSlot.Sum(c => lstTrabajador.Count(t => t.Inscripcion.FirstOrDefault(i => i.IdCursoSlot == c.IdCursoSlot).Intento.Any(i => i.Terminado))) * 100.00 / Total;
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
