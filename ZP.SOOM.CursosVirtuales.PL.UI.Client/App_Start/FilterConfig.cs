using System.Web;
using System.Web.Mvc;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}