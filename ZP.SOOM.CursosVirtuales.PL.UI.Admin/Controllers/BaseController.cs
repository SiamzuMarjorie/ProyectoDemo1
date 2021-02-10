using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.BL.BC;
using System.Web.Security;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                UsuarioModel objUsuarioModel = UsuarioModel.FromString(User.Identity.Name);
                Usuario objUsuario = new UsuarioBC().ObtenerUsuario(objUsuarioModel.IdUsuario);
                if (objUsuario == null || objUsuario.UltimaActualizacion != null && objUsuario.UltimaActualizacion > objUsuarioModel.FechaHoraLogin)
                {
                    FormsAuthentication.SignOut();
                    filterContext.Result = RedirectToAction("Index", "Login");
                    base.OnActionExecuting(filterContext);
                }
            }

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }
    }
}
