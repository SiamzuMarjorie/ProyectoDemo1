using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Client.Models;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Controllers
{
    [Authorize]    
    public class CuentaController : BaseController
    {
        public const int ERROR_INCOMPLETE_DATA = 1;
        public const int ERROR_INVALID_PASSWORD = 2;
        public const int ERROR_INVALID_INPUTS = 3;
        public const int ERROR_ACTUAL_PASSWORD = 4;
        public const int ERROR_UPTDATE_PASSWORD = 5;

        public ActionResult Index()
        {
            try
            {
                UsuarioBC objUsuarioBC = new UsuarioBC();
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);

                Usuario objUsuario = objUsuarioBC.ObtenerUsuario(objUsuarioLogueado.IdUsuario);
                objUsuarioLogueado = UsuarioModel.FromUsuario(objUsuario);

                return View(objUsuarioLogueado);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ocurrió un error al obtener la información de cuenta";
                ViewBag.exMessage = ex.Message;
                ViewBag.exStackTrace = ex.StackTrace;
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
                            objResultObject.Code = 0;
                        }
                        else
                        {
                            objResultObject.Code = ERROR_INVALID_PASSWORD;
                            objResultObject.Message = "Los campos de la nueva contraseña no son iguales.";
                        }
                    }
                    else
                    {
                        objResultObject.Code = ERROR_INVALID_INPUTS;
                        objResultObject.Message = "Debes completar todos los campos.";
                    }
                }
                else
                {
                    objResultObject.Code = ERROR_ACTUAL_PASSWORD;
                    objResultObject.Message = "La contraseña actual es incorrecta.";
                }
            }
            catch (Exception ex)
            {
                objResultObject.Code = ERROR_UPTDATE_PASSWORD;
                objResultObject.Message = "Ocurrió un error al actualizar la contraseña";
            }

            return new JsonResult() { Data = objResultObject };
        }

    }
}
