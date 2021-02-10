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
    [Authorize]
    public class UsuariosController : BaseController
    {
        private int PAGE_SIZE = 20;

        public ActionResult Index(string Tipo, string __Valor, int? Page)
        {
            try
            {
                List<Usuario> lstUsuario = null;

                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);

                ObjectContainer Pages = new ObjectContainer();
                if (Page == null)
                    Page = 1;

                if (Tipo == "Codigo")
                    lstUsuario = new UsuarioBC().BuscarUsuarios(__Valor, null, null, null, null, Pages, Page, PAGE_SIZE);
                else if (Tipo == "Nombre")
                    lstUsuario = new UsuarioBC().BuscarUsuarios(null, __Valor, null, null, null, Pages, Page, PAGE_SIZE);
                else if (Tipo == "Puesto")
                    lstUsuario = new UsuarioBC().BuscarUsuarios(null, null, __Valor, null, null, Pages, Page, PAGE_SIZE);
                else if (Tipo == "Area")
                    lstUsuario = new UsuarioBC().BuscarUsuarios(null, null, null, __Valor, null, Pages, Page, PAGE_SIZE);
                else if (Tipo == "GrupoOcupacional")
                    lstUsuario = new UsuarioBC().BuscarUsuarios(null, null, null, null, __Valor, Pages, Page, PAGE_SIZE);
                else
                    lstUsuario = new UsuarioBC().BuscarUsuarios(null, null, null, null, null, Pages, Page, PAGE_SIZE);

                List<UsuarioModel> lstUsuarioModel = new List<UsuarioModel>();
                foreach (Usuario objUsuario in lstUsuario)
                    lstUsuarioModel.Add(UsuarioModel.FromUsuario(objUsuario));



                ViewBag.Pages = Pages.Value;
                return View(lstUsuarioModel);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al listar los usuarios.";
                return View("Error");
            }
        }

        public ActionResult ListarUsuarios(int Nivel)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                List<UsuarioModel> lstUsuarioModels = new List<UsuarioModel>();
                IQueryable<Usuario> lstUsuarios = new UsuarioBC().ListarUsuario();
                foreach (Usuario objUsuarios in lstUsuarios.Where(u => u.IdTrabajador != null && u.Trabajador.Nivel >= Nivel))
                    lstUsuarioModels.Add(UsuarioModel.FromUsuario(objUsuarios));

                objResultObject.Data = lstUsuarioModels;
                objResultObject.OK = true;
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "ocurrio un error";
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult ActualizarEstado(int IdTrabajador, bool Estado)
        {
            ResultObject objResultObject = new ResultObject();

            try
            {
                String sEstado = Estado ? Util.Constants.Trabajador.Estado.ACTIVO : Util.Constants.Trabajador.Estado.INACTIVO;
                new UsuarioBC().ActualizarEstado(IdTrabajador, sEstado);
                objResultObject.OK = true;
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al actualizar el estado.";
                objResultObject.exMessage = ex.Message;
                objResultObject.exStackTrace = ex.StackTrace;
            }

            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult MiCuenta()
        {
            try
            {
                UsuarioModel objUsuarioModel = UsuarioModel.FromString(User.Identity.Name);
                Usuario objUsuario = new UsuarioBC().ObtenerUsuario(objUsuarioModel.IdUsuario);

                if (objUsuarioModel.Username != Constants.Usuario.MASTER_USER)
                {
                    List<GrupoTrabajadorModel> lstGrupoTrabajadorModel = new List<GrupoTrabajadorModel>();
                    IEnumerable<GrupoTrabajador> lstGrupoTrabajador = new UsuarioBC().ListarGrupoTrabajador(objUsuarioModel.IdTrabajador);
                    foreach (GrupoTrabajador objGrupoTrabajador in lstGrupoTrabajador.ToList())
                        lstGrupoTrabajadorModel.Add(GrupoTrabajadorModel.FromGrupoTrabajador(objGrupoTrabajador));

                    ViewBag.MisGrupos = lstGrupoTrabajadorModel;
                }
                

                return View(UsuarioModel.FromUsuario(objUsuario));
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al mostrar la cuenta del usuario.";
                return View("Error");
            }
        }

        public ActionResult Detalle(int id)
        {
            try
            {
                IQueryable<Trabajador> lstTrabajador = new UsuarioBC().ListarTrabajadores();

                Usuario objUsuario = new UsuarioBC().ObtenerUsuario(id);

                List<string> lstArea = new List<string>();
                List<string> listarArea = new UsuarioBC().ListarAreaUsuario();
                foreach (string Area in listarArea)
                    lstArea.Add(Area);
                ViewBag.listarArea = lstArea;

                List<string> lstGerencia = new List<string>();
                //List<string> listarGerencia = new UsuarioBC().ListarGerenciaUsuario();
                //foreach (string Gerencia in listarGerencia)
                //    lstGerencia.Add(Gerencia);
                //ViewBag.listarGerencia = lstGerencia;

                IQueryable<GrupoOcupacional> lstGrupoOcupacional = new UsuarioBC().ListarGrupoOcupacional();
                List<GrupoOcupacionalModel> lstGrupoOcupacionalModel = new List<GrupoOcupacionalModel>();
                foreach (GrupoOcupacional objGrupoOcupacional in lstGrupoOcupacional)
                    lstGrupoOcupacionalModel.Add(GrupoOcupacionalModel.FromGrupoOcupacional(objGrupoOcupacional));
                ViewBag.ListaGrupoOcupacional = lstGrupoOcupacionalModel;

                List<UsuarioModel> lstJefeDirecto = new List<UsuarioModel>();
                List<Trabajador> ListarPosibleJefe = new UsuarioBC().ListarPosibleJefe();
                foreach (Trabajador objJefeDirecto in ListarPosibleJefe)
                    if (objUsuario.IdTrabajador != objJefeDirecto.IdTrabajador)
                    {
                        lstJefeDirecto.Add(new UsuarioModel()
                        {
                            IdTrabajador = objJefeDirecto.IdTrabajador,
                            Nombres = objJefeDirecto.Nombres,
                            PrimerApellido = objJefeDirecto.PrimerApellido,
                            SegundoApellido = objJefeDirecto.SegundoApellido
                        });
                    }

                ViewBag.Jefes = lstJefeDirecto;

                List<GrupoModel> lstGrupoModel = new List<GrupoModel>();
                IEnumerable<Grupo> lstGrupo = new MapaBC().ListarGrupo().Where(t=>t.Nombre != "Todos").OrderBy(g => g.Nombre);
                foreach (Grupo objGrupo in lstGrupo.ToList())
                    lstGrupoModel.Add(GrupoModel.FromGrupo(objGrupo, objUsuario.IdTrabajador));

                ViewBag.Grupos = lstGrupoModel;

                return View(UsuarioModel.FromUsuario(objUsuario));
            }
            catch (Exception)
            {
                ViewBag.Msg = "Ocurrió un error al tratar de ingresar al detalle del usuario.";
                return View("Error");
            }
        }

        public ActionResult ActualizarPassword(string Password, string NewPassword, string RepeatPassword)
        {
            ResultObject objResultObject = new ResultObject();

            try
            {
                Usuario objUsuario = new UsuarioBC().Login(UsuarioModel.FromString(User.Identity.Name).Username, Encryptor.SHA256Hash(Password));
                if (objUsuario != null)
                {
                    if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(RepeatPassword))
                    {
                        if (NewPassword == RepeatPassword)
                        {
                            new UsuarioBC().ActualizarPassword(objUsuario.IdUsuario, Encryptor.SHA256Hash(NewPassword));
                            objResultObject.OK = true;
                        }
                        else
                        {
                            objResultObject.OK = false;
                            objResultObject.Message = "Los campos de la nueva contraseña no son iguales.";
                        }
                    }
                    else
                    {
                        objResultObject.OK = false;
                        objResultObject.Message = "Debes completar todos los campos.";
                    }
                }
                else
                {
                    objResultObject.OK = false;
                    objResultObject.Message = "La contraseña actual es incorrecta.";
                }
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al actualizar la contraseña";
            }

            return new JsonResult() { Data = objResultObject };
        }



        public ActionResult EliminarUsuario(int IdUsuario)
        {
            ResultObject objResultObject = new ResultObject();

            try
            {
                new UsuarioBC().EliminarUsuario(IdUsuario);
                objResultObject.OK = true;
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.exMessage = ex.Message;
                objResultObject.Message = "Ocurrió un error al eliminar al usuario";
            }

            return new JsonResult() { Data = objResultObject };
        }


        public ActionResult NuevoUsuario(int? id)
        {
            try
            {

                List<string> lstArea = new List<string>();
                List<string> listarArea = new UsuarioBC().ListarAreaUsuario();
                foreach (string Area in listarArea)
                    lstArea.Add(Area);
                ViewBag.listarArea = lstArea;

                List<string> lstGerencia = new List<string>();
                //List<string> listarGerencia = new UsuarioBC().ListarGerenciaUsuario();
                //foreach (string Gerencia in listarGerencia)
                //    lstGerencia.Add(Gerencia);
                //ViewBag.listarGerencia = lstGerencia;

                IQueryable<GrupoOcupacional> lstGrupoOcupacional = new UsuarioBC().ListarGrupoOcupacional();
                List<GrupoOcupacionalModel> lstGrupoOcupacionalModel = new List<GrupoOcupacionalModel>();
                foreach (GrupoOcupacional objGrupoOcupacional in lstGrupoOcupacional)
                    lstGrupoOcupacionalModel.Add(GrupoOcupacionalModel.FromGrupoOcupacional(objGrupoOcupacional));
                ViewBag.ListaGrupoOcupacional = lstGrupoOcupacionalModel;

                ViewBag.usuario = id;
                //ViewBag.Trabajadores = lstUsuarioModel;

                List<UsuarioModel> lstJefeDirecto = new List<UsuarioModel>();
                List<Trabajador> ListarPosibleJefe = new UsuarioBC().ListarPosibleJefe();
                foreach (Trabajador objJefeDirecto in ListarPosibleJefe)
                    lstJefeDirecto.Add(new UsuarioModel()
                    {
                        IdTrabajador = objJefeDirecto.IdTrabajador,
                        Nombres = objJefeDirecto.Nombres,
                        PrimerApellido = objJefeDirecto.PrimerApellido,
                        SegundoApellido = objJefeDirecto.SegundoApellido
                    });
                ViewBag.Jefes = lstJefeDirecto;

                //List<GrupoOcupacional> lstGrupoocupacional = new UsuarioBC().ListarGrupoOcupacional();
                //List<GrupoOcupacionalModel> lstGrupoOcupacionalModel = new List<GrupoOcupacionalModel>();
                //foreach (GrupoOcupacional objGrupoOcupacional in lstGrupoocupacional)
                //    lstGrupoOcupacionalModel.Add(GrupoOcupacionalModel.FromGrupoOcupacional(objGrupoOcupacional));
                //ViewBag.GrupoOcupacional = lstGrupoOcupacionalModel;

                if (id != null)
                {

                    Usuario objUsuario = new UsuarioBC().ObtenerUsuario(id.Value);
                    //if (objUsuario.Trabajador.Jefe != null)
                    //{
                    //    Trabajador objJefe = objUsuario.Trabajador.Jefe;
                    //    ViewBag.jefe = UsuarioModel.FromTrabajador(objJefe, false, false);
                    //}

                }

                List<GrupoModel> lstGrupoModel = new List<GrupoModel>();
                IEnumerable<Grupo> lstGrupo = new MapaBC().ListarGrupo().Where(t => t.Nombre != "Todos").OrderBy(g => g.Nombre);
                foreach (Grupo objGrupo in lstGrupo.ToList())
                    lstGrupoModel.Add(GrupoModel.FromGrupo(objGrupo));

                ViewBag.Grupos = lstGrupoModel;
                return View("Detalle");
            }
            catch (Exception)
            {

                ViewBag.Msg = "Ocurrió un error al tratar de ir a la vista de Nuevo Usuario.";
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult GuardarUsuario(UsuarioModel objUsuarioModel, int? IdUsuarioEliminar, string GrupoOcupacionalNombre, int[] IdGrupo)
        {
            ResultObject objResultObject = new ResultObject();

            try
            {

                UsuarioBC objUsuarioBC = new UsuarioBC();
                if (!String.IsNullOrWhiteSpace(objUsuarioModel.Codigo) &&
                    !String.IsNullOrWhiteSpace(objUsuarioModel.Nombres) &&
                    !String.IsNullOrWhiteSpace(objUsuarioModel.PrimerApellido) &&
                    !String.IsNullOrWhiteSpace(objUsuarioModel.SegundoApellido) &&
                    !String.IsNullOrWhiteSpace(objUsuarioModel.Email) &&
                    !String.IsNullOrWhiteSpace(objUsuarioModel.Area) &&
                    !String.IsNullOrWhiteSpace(GrupoOcupacionalNombre) &&
                    !String.IsNullOrWhiteSpace(objUsuarioModel.Username) &&
                    !String.IsNullOrWhiteSpace(objUsuarioModel.Perfil))
                {
                    Usuario objUsuarioExistente = objUsuarioBC.ObtenerUsuario(objUsuarioModel.Username);
                    Trabajador objTrabajadorExistente = objUsuarioBC.ObtenerTrabajador(objUsuarioModel.DNI);
                    if (objUsuarioModel.IdJefe == null)
                        objUsuarioModel.IdJefe = 0;
                    Trabajador objTrabajadorCabeza = new UsuarioBC().ObtenerTrabajadorCabeza();
                    Trabajador objTrabajadorJefe = new UsuarioBC().ObtenerTrabajador(objUsuarioModel.IdJefe.Value);
                    if (objTrabajadorJefe != null || objTrabajadorCabeza == null || objTrabajadorCabeza != null)
                    {
                        if (objTrabajadorExistente == null || objTrabajadorExistente.IdTrabajador == objUsuarioModel.IdTrabajador)
                        {
                            if (objUsuarioExistente == null || objUsuarioExistente.IdUsuario == objUsuarioModel.IdUsuario)
                            {
                                if (objUsuarioModel.Username != Util.Constants.Usuario.MASTER_USER)
                                {
                                    if ((objUsuarioModel.IdUsuario != 0 && String.IsNullOrEmpty(objUsuarioModel.Password)) ||
                                        (objUsuarioModel.IdUsuario == 0 && !String.IsNullOrEmpty(objUsuarioModel.Password)) ||
                                        (objUsuarioModel.IdUsuario != 0 && objUsuarioModel.Password.Length >= 8))
                                    {
                                        bool EmailValido = true;
                                        try
                                        {
                                            new MailAddress(objUsuarioModel.Email);
                                        }
                                        catch (Exception ex)
                                        {
                                            EmailValido = false;
                                        }

                                        if (EmailValido)
                                        {
                                            Trabajador objTrabajador = objUsuarioBC.ObtenerTrabajador(objUsuarioModel.IdTrabajador);

                                            if (objUsuarioModel.UploadImage != null)
                                            {
                                                if (objTrabajador != null && objTrabajador.Foto != null)
                                                    new FileInfo(Path.Combine(Server.MapPath("~/Images/Usuarios"), objTrabajador.Foto)).Delete();

                                                string fileName = Path.GetFileNameWithoutExtension(objUsuarioModel.UploadImage.FileName);
                                                string extension = Path.GetExtension(objUsuarioModel.UploadImage.FileName);
                                                fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                                                objUsuarioModel.Foto = fileName;
                                                objUsuarioModel.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Images/Usuarios"), fileName));
                                            }

                                            if (IdUsuarioEliminar != null)
                                            {
                                                objUsuarioBC.EliminarConReemplazoNuevo(objUsuarioModel.ToUsuario(), objUsuarioModel.ToTrabajador(), IdUsuarioEliminar.Value, GrupoOcupacionalNombre.ToUpper());
                                            }
                                            else
                                            {
                                                Usuario objUsuario = objUsuarioModel.ToUsuario();
                                                objUsuarioBC.GuardarUsuario(objUsuario, objUsuarioModel.ToTrabajador(), GrupoOcupacionalNombre.ToUpper(), false);

                                                objUsuarioBC.AsignarGrupoATrabajador(objUsuario.IdTrabajador.Value, IdGrupo);
                                            }


                                            if (objTrabajador == null)
                                                objUsuarioBC.EnviarMailNuevoUsuario(objUsuarioModel.Email, objUsuarioModel.Nombres + " " + objUsuarioModel.PrimerApellido + " " + objUsuarioModel.SegundoApellido, objUsuarioModel.Username, objUsuarioModel.Password);

                                            objResultObject.OK = true;
                                        }
                                        else
                                        {
                                            objResultObject.OK = false;
                                            objResultObject.Message = "Debes ingresar un email válido.";
                                        }
                                    }
                                    else
                                    {
                                        objResultObject.OK = false;
                                        objResultObject.Message = "La contraseña debe tener al menos 8 caracteres.";
                                    }
                                }
                                else
                                {
                                    objResultObject.OK = false;
                                    objResultObject.Message = "El administrador Master no puede ser modificado.";
                                }
                            }
                            else
                            {
                                objResultObject.OK = false;
                                objResultObject.Message = "Ya existe un trabajador registrado con este nombre de usuario.";
                            }
                        }
                        else
                        {
                            objResultObject.OK = false;
                            objResultObject.Message = "Ya existe un usuario registrado con este DNI.";
                        }
                    }
                    else
                    {
                        objResultObject.OK = false;
                        objResultObject.Message = objUsuarioModel.Jefe + " no es un jefe valido.";

                    }
                }
                else
                {
                    objResultObject.OK = false;
                    objResultObject.Message = "Debes llenar los campos solicitados.";
                }

            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al guardar el usuario";
            }

            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult SaveData(UsuarioModel objUsuarioModel)
        {
            try
            {
                UsuarioBC objUsuarioBC = new UsuarioBC();
                if (objUsuarioModel.UploadImage != null)
                {
                    Trabajador objTrabajador = objUsuarioBC.ObtenerTrabajador(objUsuarioModel.IdTrabajador);

                    if (objTrabajador != null && objTrabajador.Foto != null)
                        new FileInfo(Path.Combine(Server.MapPath("~/Images/Usuarios"), objTrabajador.Foto)).Delete();

                    string fileName = Path.GetFileNameWithoutExtension(objUsuarioModel.UploadImage.FileName);
                    string extension = Path.GetExtension(objUsuarioModel.UploadImage.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                    objUsuarioModel.Foto = fileName;
                    objUsuarioModel.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Images/Usuarios"), fileName));

                    //hay que eliminar la foto anterior
                    objUsuarioBC.ActualizarFoto(objUsuarioModel.ToTrabajador());
                }
                var result = "subido";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult EliminarconReemplazoExistente(int IdUsuarioEliminado, int IdUsuarioReemplazante)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                new UsuarioBC().EliminarconReemplazoExistente(IdUsuarioEliminado, IdUsuarioReemplazante);

                objResultObject.OK = true;
                objResultObject.Message = "Usuario eliminado";
            }
            catch (Exception ex)
            {

                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al eliminar al usuario";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult EliminarSinReemplazo(int IdUsuarioEliminado)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                new UsuarioBC().EliminarSinReemplazo(IdUsuarioEliminado);

                objResultObject.OK = true;
                objResultObject.Message = "Usuario eliminado";
            }
            catch (Exception ex)
            {

                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al eliminar al usuario";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpGet]
        [Permissions(new string[] { Util.Constants.Usuario.Perfil.ADMINISTRADOR })]
        public FileResult DownloadPlantillaCargaMasiva()
        {
            try
            {
                string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plantillas", "[Ciudad Positiva] Plantilla carga masiva.xlsx");
                return File(ruta, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "[Ciudad Positiva] Plantilla carga masiva.xlsx");
            }
            catch (Exception ex)
            {
                throw ex;
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


        public ActionResult GenerarReporteTrabajadores()
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                string FechaActual = DateTimeHelper.PeruDateTime.ToString("ddMMyyyyHHmmssfff");
                UsuarioBC objUsuarioBC = new UsuarioBC();
                IEnumerable<IDictionary<string, object>> lstTrabajador = objUsuarioBC.DescargarBaseTrabajadores();

                new ExcelGenerator().GenerarReporterBaseTrabajadores(lstTrabajador, Session, FechaActual);
                objResultObject.OK = true;
                objResultObject.Data = "Base de usuarios " + FechaActual + ".xlsx";
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al generar la base de usuarios." + ex.Message + ex.StackTrace;
            }

            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        [Permissions(new string[] { Util.Constants.Usuario.Perfil.ADMINISTRADOR })]
        public JsonResult CargaMasivaUsuarios(HttpPostedFileBase FileMasivoUsuarios)
        {
            ResultObject objResultObject = new ResultObject();
            UsuarioBC objUsuarioBC = new UsuarioBC();

            string fileRuta = string.Empty;
            string fileNombre = string.Empty;
            string fileExtension = string.Empty;
            try
            {
                if (FileMasivoUsuarios != null)
                {
                    fileNombre = Path.GetFileNameWithoutExtension(FileMasivoUsuarios.FileName);
                    fileExtension = Path.GetExtension(FileMasivoUsuarios.FileName);
                    fileNombre = fileNombre + DateTime.Now.ToString("yymmdd_hhmmss") + fileExtension;
                    fileRuta = Path.Combine(Server.MapPath("~/CargaMasivaUsuarios"), fileNombre);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        FileMasivoUsuarios.SaveAs(fileRuta);

                        objUsuarioBC.CargaMasivaUsuariosExcel(fileExtension, fileRuta);
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
                objResultObject.Message = "Ocurrio un problema en la carga masiva." + ex;
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult GetProgressCargaMasiva()
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

    }
}
