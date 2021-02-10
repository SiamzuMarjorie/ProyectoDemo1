using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using System.Configuration;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    public class NotificacionesController : Controller
    {
        [Authorize]
        public ActionResult Suscribir(String Subscription)
        {
            ResultObject objResultObject = new ResultObject();

            try
            {
                JObject jSubscription = JObject.Parse(Subscription);
                String endpoint = (String)jSubscription["endpoint"];
                String p256dh = (String)jSubscription["keys"]["p256dh"];
                String auth = (String)jSubscription["keys"]["auth"];

                UsuarioModel objUsuarioModel = UsuarioModel.FromString(User.Identity.Name);
                SuscripcionNotificacion objSuscripcionNotificacion = new SuscripcionNotificacion();
                objSuscripcionNotificacion.IdUsuario = objUsuarioModel.IdUsuario;
                objSuscripcionNotificacion.Endpoint = endpoint;
                objSuscripcionNotificacion.PS56DH = p256dh;
                objSuscripcionNotificacion.Auth = auth;

                int Result = new NotificacionBC().RegistrarSuscripcionNotificacion(objSuscripcionNotificacion);
                objResultObject.OK = true;

                if (Result > 0)
                    new NotificacionBC().EnviarNotificacionPush(objUsuarioModel.IdUsuario, "SOOM Cursos Virtuales", "¡Las notificaciones se han activado con éxito!", ConfigurationManager.AppSettings["UrlSOOM"]);
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al suscribir al usuario.";
                objResultObject.exMessage = ex.Message;
                objResultObject.exStackTrace = ex.StackTrace;
            }

            return new JsonResult() { Data = objResultObject };
        }

        [Authorize]
        public ActionResult Desuscribir(String Subscription)
        {
            ResultObject objResultObject = new ResultObject();

            try
            {
                if (Subscription != null)
                {
                    JObject jSubscription = JObject.Parse(Subscription);
                    String endpoint = (String)jSubscription["endpoint"];
                    String p256dh = (String)jSubscription["keys"]["p256dh"];
                    String auth = (String)jSubscription["keys"]["auth"];

                    UsuarioModel objUsuarioModel = UsuarioModel.FromString(User.Identity.Name);
                    SuscripcionNotificacion objSuscripcionNotificacion = new SuscripcionNotificacion();
                    objSuscripcionNotificacion.IdUsuario = objUsuarioModel.IdUsuario;
                    objSuscripcionNotificacion.Endpoint = endpoint;
                    objSuscripcionNotificacion.PS56DH = p256dh;
                    objSuscripcionNotificacion.Auth = auth;

                    new NotificacionBC().EliminarSuscripcionNotificacion(objSuscripcionNotificacion);
                    objResultObject.OK = true;
                }
                else
                {
                    objResultObject.OK = false;
                    objResultObject.Message = "La suscripción no es válida.";
                }
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al desuscribir al usuario.";
                objResultObject.exMessage = ex.Message;
                objResultObject.exStackTrace = ex.StackTrace;
            }

            return new JsonResult() { Data = objResultObject };
        }
    }
}
