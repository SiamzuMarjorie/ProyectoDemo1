using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    public class LoginController : Controller
    {
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
                ViewBag.Msg = "Debes completar todos los campos.";
                return View();
            }

            try
            {
                Usuario objUsuario = new UsuarioBC().Login(Username, Encryptor.SHA256Hash(Password));

                if (objUsuario != null)
                {

                    if (objUsuario.Perfil == Constants.Usuario.Perfil.ADMINISTRADOR)
                    {
                        UsuarioModel objUsuarioModel = UsuarioModel.FromUsuario(objUsuario);
                        FormsAuthentication.SetAuthCookie(objUsuarioModel.ToString(), false);
                        if (String.IsNullOrEmpty(ReturnUrl))
                            return RedirectToRoute("Default");
                        else
                            return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ViewBag.Msg = "Usted no cuenta con permisos para loguearse.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Msg = "Tus datos de acceso son incorrectos.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al iniciar sesión";
                ViewBag.Message = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
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

                ViewBag.Msg = "Ocurrió un error al restaurar contraseña.";
                return View("Error");
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
                objUsuarioBC.GuardarUsuario(objUsuario, objUsuario.Trabajador, objUsuario.Trabajador.GrupoOcupacional.Nombre, false);
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al intentar regenerar password.";
                return View("Error");
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

                        objResultObject.OK = true;
                    }
                    else
                    {

                        objResultObject.OK = false;
                        objResultObject.Message = "El username o correo no existen.";
                    }
                }
                else
                {

                    objResultObject.OK = false;
                    objResultObject.Message = "Debes completar todos los campos.";

                }
            }
            catch (Exception ex)
            {

                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al enviar la nueva contraseña";
            }
            return new JsonResult() { Data = objResultObject };

        }

    }
}
