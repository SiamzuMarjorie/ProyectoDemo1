using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    public class Permissions : ActionFilterAttribute
    {
        private String[] Perfiles;

        public Permissions(String[] Perfiles)
        {
            this.Perfiles = Perfiles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(filterContext.HttpContext.User.Identity.Name);
                if (!Perfiles.Contains(objUsuarioLogueado.Perfil))
                {
                    ViewResult objViewResult = new ViewResult
                    {
                        ViewName = "Error",
                        ViewData = filterContext.Controller.ViewData,
                        TempData = filterContext.Controller.TempData
                    };
                    objViewResult.ViewBag.Message = "No tiene los permisos para acceder a esta página.";
                    filterContext.Result = objViewResult;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}