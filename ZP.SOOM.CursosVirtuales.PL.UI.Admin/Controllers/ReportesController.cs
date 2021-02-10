using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.Util;
using ZP.SOOM.DesempenoExpress.PL.UI.Controllers;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        //
        // GET: /Reportes/
        public ActionResult Index()
        {
            List<int> lstAno = new CursoBC().ListarAnos();
            ViewBag.ListarAno = lstAno;
            return View();
            
        }

        public ActionResult ListaCampos(string NombreCampo, int? Ano)
        {
            try
            {
                UsuarioBC objUsuario = new UsuarioBC();
                MapaBC objMapa = new MapaBC();
                List<string> lstCampos = new List<string>();

                if (NombreCampo == Constants.ReporteGeneral.Campos.COMPANIA)
                {
                    lstCampos = objUsuario.ListarCompanias();
                }
                else if (NombreCampo == Constants.ReporteGeneral.Campos.SEDE)
                {
                    lstCampos = objUsuario.ListarSede();
                }
                else if (NombreCampo == Constants.ReporteGeneral.Campos.GERENCIA)
                {
                    lstCampos = objUsuario.ListarGerencia();
                }
                else if (NombreCampo == Constants.ReporteGeneral.Campos.CATEGORIA)
                {
                    lstCampos = objUsuario.ListarGrupoOcupacionales();
                }
                else if (NombreCampo == Constants.ReporteGeneral.Campos.CURSOSLOT)
                {
                    List<SlotModel> lstSlotModel = new List<SlotModel>();
                    List<CursoSlot> lstCursoSlotAno = objMapa.ListarCursoSlotPorAno(Ano).ToList();
                    List<string> lstNombreSlot = lstCursoSlotAno.Select(cs => cs.NombreSlot).Distinct().ToList();
                    foreach (string NombreSlot in lstNombreSlot)
                    {
                        SlotModel objSlotModel = new SlotModel();
                        objSlotModel.NombreSlot = NombreSlot;
                        List<CursoSlot> lstCursosDelSlot = lstCursoSlotAno.Where(cs => cs.NombreSlot == NombreSlot).ToList();
                        objSlotModel.CursoSlot = new List<CursoSlotModel>();
                        foreach (CursoSlot objCursoSlot in lstCursosDelSlot)
                        {
                            objSlotModel.CursoSlot.Add(CursoSlotModel.FromCursoSlot(objCursoSlot));
                        }

                        lstSlotModel.Add(objSlotModel);
                    }
                    ViewBag.lstSlot = lstSlotModel;
                }
                else
                {
                    lstCampos = objMapa.ListarNombreGrupo();
                }
                
                return PartialView("_TablaReporte", lstCampos);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al cargar la lista";
                return View("Error");
            }
            
        }

        public ActionResult GetProgress()
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                double? currentProgress = Convert.ToDouble(Session[Constants.GetProgress.Porcentaje.CurrentProgress] ?? 0);
                double? totalProgress = Convert.ToDouble(Session[Constants.GetProgress.Porcentaje.TotalProgress] ?? 0);

                if (totalProgress != 0)
                    objResultObject.Data = (int)((currentProgress / totalProgress) * 100);
                else
                    objResultObject.Data = 0;

                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al obtener el progreso.";
                objResultObject.exMessage = ex.Message;
                objResultObject.exStackTrace = ex.StackTrace;
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult GenerarReporteGeneral(string NombreCampo, int Ano, string[] Intentos, string FechaInicio, string FechaFin, string[] OpcionSeleccionada, int?[] IdCursoSlot, string[] Estatus)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);

                string FechaActual = DateTimeHelper.PeruDateTime.ToString("ddMMyyyyHHmmssfff");

                ReporteBC objReporteBC = new ReporteBC();
                List<IDictionary<string, object>> ReporteGeneral = objReporteBC.ObtenerReporteGeneral(NombreCampo, Ano, Intentos, FechaInicio, FechaFin, OpcionSeleccionada, IdCursoSlot,Estatus).ToList();
                new ExcelGenerator().GenerarReporteGeneral(ReporteGeneral, Session, FechaActual);
                objResultObject.OK = true;
                objResultObject.Data = "Reporte general detallado " + FechaActual + ".xlsx";
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al generar el reporte el reporte." + ex.Message + ex.StackTrace;
            }

            return new JsonResult() { Data = objResultObject };
        }

    }
}
