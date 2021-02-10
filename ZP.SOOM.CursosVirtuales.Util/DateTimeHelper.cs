using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;

namespace ZP.SOOM.CursosVirtuales.Util
{
    public class DateTimeHelper
    {
        public static DateTime PeruDateTime
        {
            get { return DateTime.UtcNow.AddHours(-5); }
        }

        public static DateTime EndOfDay(DateTime objDateTime)
        {
            return objDateTime.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
        }

        public static String MonthToString(int Month)
        {
            string respuesta = string.Empty;
            switch (Month)
            {
                case 1: respuesta = "Enero"; break;
                case 2: respuesta = "Febrero"; break;
                case 3: respuesta = "Marzo"; break;
                case 4: respuesta = "Abril"; break;
                case 5: respuesta = "Mayo"; break;
                case 6: respuesta = "Junio"; break;
                case 7: respuesta = "Julio"; break;
                case 8: respuesta = "Agosto"; break;
                case 9: respuesta = "Septiembre"; break;
                case 10: respuesta = "Octubre"; break;
                case 11: respuesta = "Noviembre"; break;
                case 12: respuesta = "Diciembre"; break;
            }
            return respuesta;
        }

        public static DateTime FromString(String sDateTime, bool Date, bool Time)
        {
            String Format = "";

            if (Date)
                Format += "dd/MM/yyyy";

            if (Time)
            {
                if (!String.IsNullOrWhiteSpace(Format))
                    Format += " ";

                Format += "HH:mm";
            }

            return DateTime.ParseExact(sDateTime, Format, CultureInfo.InvariantCulture);
        }
    }
}
