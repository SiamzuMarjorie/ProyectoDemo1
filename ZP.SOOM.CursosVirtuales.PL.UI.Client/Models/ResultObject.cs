using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Models
{
    public class ResultObject
    {
        public int Code { get; set; }
        public Object Data { get; set; }
        public String Message { get; set; }
        public String exMessage { get; set; }
        public String exStackTrace { get; set; }

        public void CaptureException(Exception ex, String Message)
        {
            this.Code = -1;
            this.Message = Message;
            this.exMessage = ex.Message;
            this.exStackTrace = ex.StackTrace;
        }
    }
}