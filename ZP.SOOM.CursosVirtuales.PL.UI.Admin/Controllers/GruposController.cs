using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Web.Security;
using System.Net.Mail;
using System.Configuration;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.Util;
using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.DesempenoExpress.PL.UI.Controllers;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    public class GruposController : BaseController
    {
        //
        private int PAGE_SIZE = 20;

        public ActionResult Index()
        {
            MapaBC objMapaBC = new MapaBC();
            List<GrupoModel> lstGrupoModel = new List<GrupoModel>();
            IEnumerable<Grupo> lstGrupo = objMapaBC.ListarGrupo().Where(c => !c.Eliminado).OrderBy(g => g.Nombre);
            foreach (Grupo objGrupo in lstGrupo.ToList())
                lstGrupoModel.Add(GrupoModel.FromGrupo(objGrupo));

            return View(lstGrupoModel);
        }

        public ActionResult DetalleGrupo(int IdGrupo, string Tipo, string __Valor, int? Page)
        {
            try
            {
                List<Trabajador> lstTrabajador = null;

                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);

                ObjectContainer Pages = new ObjectContainer();
                if (Page == null)
                    Page = 1;

                if (Tipo == "Codigo")
                    lstTrabajador = new UsuarioBC().BuscarTrabajadorPorGrupo(__Valor, null, null, null, null, Pages, Page, PAGE_SIZE, IdGrupo);
                else if (Tipo == "Nombre")
                    lstTrabajador = new UsuarioBC().BuscarTrabajadorPorGrupo(null, __Valor, null, null, null, Pages, Page, PAGE_SIZE, IdGrupo);
                else if (Tipo == "Puesto")
                    lstTrabajador = new UsuarioBC().BuscarTrabajadorPorGrupo(null, null, __Valor, null, null, Pages, Page, PAGE_SIZE, IdGrupo);
                else if (Tipo == "Area")
                    lstTrabajador = new UsuarioBC().BuscarTrabajadorPorGrupo(null, null, null, __Valor, null, Pages, Page, PAGE_SIZE, IdGrupo);
                else if (Tipo == "GrupoOcupacional")
                    lstTrabajador = new UsuarioBC().BuscarTrabajadorPorGrupo(null, null, null, null, __Valor, Pages, Page, PAGE_SIZE, IdGrupo);
                else
                    lstTrabajador = new UsuarioBC().BuscarTrabajadorPorGrupo(null, null, null, null, null, Pages, Page, PAGE_SIZE, IdGrupo);

                List<UsuarioModel> lstUsuarioModel = new List<UsuarioModel>();
                foreach (Trabajador objTrabajador in lstTrabajador)
                    lstUsuarioModel.Add(UsuarioModel.FromTrabajador(objTrabajador, false, false));

                ViewBag.Pages = Pages.Value;

                Grupo objGrupo = new MapaBC().ObtenerGrupo(IdGrupo);
                ViewBag.NombreGrupo = objGrupo.Nombre;
                ViewBag.DescripcionGrupo = objGrupo.Descripcion;
                ViewBag.IdGrupo = objGrupo.IdGrupo;

                return View(lstUsuarioModel);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al listar los usuarios.";
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult GuardarGrupo(GrupoModel objGrupoModel)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                Grupo objGrupo = objGrupoModel.ToGrupo(objGrupoModel);

                objMapaBC.GuardarGrupo(objGrupo);
                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error guardar el grupo.";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult EliminarGrupo(int IdGrupo)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();

                objMapaBC.EliminarGrupo(IdGrupo);
                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error al eliminar el grupo.";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult EliminarTrabajadoresGrupo(int IdGrupo)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();

                objMapaBC.EliminarTrabajadoresGrupo(IdGrupo);
                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error desasiganar a los trabajadores del grupo.";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult EliminarTrabajadorGrupo(int IdGrupo, int IdTrabajador)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();

                objMapaBC.EliminarTrabajadorGrupo(IdGrupo, IdTrabajador);
                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error desasiganar al trabajador del grupo.";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult GetProgress()
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioBC objUsuarioBC = new UsuarioBC();

                if (!string.IsNullOrEmpty(objUsuarioBC.GetError()))
                {
                    objResultObject.Message = objUsuarioBC.GetError();
                    objResultObject.OK = false;
                    return new JsonResult() { Data = objResultObject };
                }

                objResultObject.Data = objUsuarioBC.GetProgress();
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

        [HttpPost]
        [Permissions(new string[] { Util.Constants.Usuario.Perfil.ADMINISTRADOR })]
        public JsonResult AsignacionMasivaUsuarios(HttpPostedFileBase FileMasivoGrupo, int IdGrupo)
        {
            ResultObject objResultObject = new ResultObject();
            UsuarioBC objUsuarioBC = new UsuarioBC();

            string fileRuta = string.Empty;
            string fileNombre = string.Empty;
            string fileExtension = string.Empty;
            try
            {
                if (FileMasivoGrupo != null)
                {
                    fileNombre = Path.GetFileNameWithoutExtension(FileMasivoGrupo.FileName);
                    fileExtension = Path.GetExtension(FileMasivoGrupo.FileName);
                    fileNombre = fileNombre + DateTime.Now.ToString("yymmdd_hhmmss") + fileExtension;
                    fileRuta = Path.Combine(Server.MapPath("~/AsignacionMasivaUsuarios"), fileNombre);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        FileMasivoGrupo.SaveAs(fileRuta);

                        objUsuarioBC.AsignacionMasivaUsuarios(fileExtension, fileRuta, IdGrupo);
                        objResultObject.OK = true;
                        return new JsonResult() { Data = objResultObject };
                    }
                    else
                    {
                        objResultObject.OK = false;
                        objResultObject.Message = "Debe seleccionar un archivo Excel.";
                        return new JsonResult() { Data = objResultObject };
                    }
                }
                else
                {
                    objResultObject.OK = false;
                    objResultObject.Message = "Debe seleccionar un archivo Excel.";
                    return new JsonResult() { Data = objResultObject };
                }

            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrio un problema en la carga masiva de usuarios al grupo." + ex;
            }
            return new JsonResult() { Data = objResultObject };
        }

    }
}
