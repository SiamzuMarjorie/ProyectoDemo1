using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Client.Models;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Controllers
{
    public class LoginController : Controller
    {
        public const int ERROR_INVALID_USERNAME = 1;
        public const int ERROR_INCOMPLETE_DATA = 2;

        public ActionResult Index(String Username, String Password, String ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("", "");

            if (Username == null ||
                Password == null)
                return View();

            if (!Request.HttpMethod.Equals("POST"))
                return View();

            Username = Username.Trim();
            ViewBag.Username = Username;

            if (String.IsNullOrEmpty(Username) ||
                String.IsNullOrEmpty(Password))
            {
                ViewBag.Message = "Debes completar todos los campos.";
                return View();
            }

            try
            {
                Usuario objUsuario = new UsuarioBC().Login(Username, Encryptor.SHA256Hash(Password));

                if (objUsuario != null)
                {
                    if (objUsuario.Trabajador != null)
                    {
                        UsuarioModel objUsuarioModel = UsuarioModel.FromUsuario(objUsuario);
                        FormsAuthentication.SetAuthCookie(objUsuarioModel.ToString(), false);
                        Personaje objPersonaje = new MapaBC().ObtenerPersonaje(objUsuarioModel.IdUsuario);
                        if (objPersonaje == null)
                        {
                            return RedirectToAction("Personaje", "Mapa");
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(ReturnUrl))
                                return RedirectToRoute("Default");
                            else
                                return Redirect(ReturnUrl);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Debes ser un colaborador de la empresa para acceder a la plataforma de cursos.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Tus datos de acceso son incorrectos.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ocurrió un error al iniciar sesión";
                ViewBag.exMessage = ex.Message;
                ViewBag.exStackTrace = ex.StackTrace;
                return View("Error");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult RestablecerPassword()
        {
            return View();
        }

        public ActionResult Restaurar(int Id)
        {
            try
            {
                ViewBag.IdUsuario = Id;
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ActionResult PasswordRegenerado(int Id)
        {
            try
            {
                UsuarioBC objUsuarioBC = new UsuarioBC();
                Usuario objUsuario = objUsuarioBC.ObtenerUsuario(Id);
                objUsuario.Password = Encryptor.SHA256Hash(objUsuario.PasswordAntiguo);
                objUsuario.PasswordAntiguo = null;
                objUsuarioBC.GuardarUsuario(objUsuario, objUsuario.Trabajador, null, false);
                return View("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult PasswordRestablecido()
        {
            return View();
        }

        public ActionResult RestaurarPassword(string UsernameoCorreo)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                if (!string.IsNullOrEmpty(UsernameoCorreo))
                {
                    UsuarioBC objUsuarioBC = new UsuarioBC();
                    Usuario objUsuarioEncontrado = objUsuarioBC.EncontrarUsuario(UsernameoCorreo);

                    if (objUsuarioEncontrado != null)
                    {
                        Usuario objUsuario = objUsuarioBC.ObtenerUsuario(objUsuarioEncontrado.IdUsuario);
                        Trabajador objTrabajador = objUsuario.Trabajador;

                        string ContaseñaAleatoria = new UsuarioBC().ContrasenaRandom();
                        objUsuario.PasswordAntiguo = Encryptor.SHA256Hash(objUsuarioEncontrado.Password);
                        objUsuario.Password = Encryptor.SHA256Hash(ContaseñaAleatoria);
                        objUsuarioBC.GuardarUsuario(objUsuario, objTrabajador, objTrabajador.GrupoOcupacional.Nombre, false);
                        objUsuarioBC.EnviarNuevoPassword(objTrabajador.Email, objTrabajador.Nombres + " " + objTrabajador.PrimerApellido + " " + objTrabajador.SegundoApellido, objUsuario.Username, ContaseñaAleatoria, objUsuario.IdUsuario);

                        objResultObject.Code = 0;
                    }
                    else
                    {
                        objResultObject.Code = ERROR_INVALID_USERNAME;
                        objResultObject.Message = "El username o correo no existen.";
                    }
                }
                else
                {
                    objResultObject.Code = ERROR_INCOMPLETE_DATA;
                    objResultObject.Message = "Debes completar todos los campos.";

                }
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al enviar la nueva contraseña.");
            }
            return new JsonResult() { Data = objResultObject };

        }
    }
}
