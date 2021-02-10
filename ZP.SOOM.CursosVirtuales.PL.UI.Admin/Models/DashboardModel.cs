using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class DashboardModel
    {
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public double Resultado { get; set; }
        public int IdTrabajador { get; set; }
        public string Color { get; set; }
        public int? Value { get; set; }
    }
}