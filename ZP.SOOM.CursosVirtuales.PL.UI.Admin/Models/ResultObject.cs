using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class ResultObject
    {
        public bool OK { get; set; }
        public Object Data { get; set; }
        public String Message { get; set; }
        public String exMessage { get; set; }
        public String exStackTrace { get; set; }
    }
}