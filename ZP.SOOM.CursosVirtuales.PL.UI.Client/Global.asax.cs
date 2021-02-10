using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.Util;
using ZP.SOOM.CursosVirtuales.PL.UI.Client.Models;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            try
            {
                ActividadUsuario objActividadUsuario = new ActividadUsuario();
                if (User.Identity.IsAuthenticated) {
                    objActividadUsuario.IdUsuario = UsuarioModel.FromString(User.Identity.Name).IdUsuario;
                    objActividadUsuario.FechaHora = DateTimeHelper.PeruDateTime;
                    objActividadUsuario.TipoActividad = Constants.TipoActividad.LOGIN;
                }

                new UsuarioBC().RegistrarVisita(objActividadUsuario);
            }
            catch (Exception ex)
            { }
        }
    }

}