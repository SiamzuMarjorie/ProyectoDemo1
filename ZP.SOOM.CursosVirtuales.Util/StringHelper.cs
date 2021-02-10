using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZP.SOOM.CursosVirtuales.Util
{
    public class StringHelper
    {
        public static String ToRoman(int Number)
        {
            try
            {
                if ((Number < 0) || (Number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
                if (Number < 1) return string.Empty;
                if (Number >= 1000) return "M" + ToRoman(Number - 1000);
                if (Number >= 900) return "CM" + ToRoman(Number - 900);
                if (Number >= 500) return "D" + ToRoman(Number - 500);
                if (Number >= 400) return "CD" + ToRoman(Number - 400);
                if (Number >= 100) return "C" + ToRoman(Number - 100);
                if (Number >= 90) return "XC" + ToRoman(Number - 90);
                if (Number >= 50) return "L" + ToRoman(Number - 50);
                if (Number >= 40) return "XL" + ToRoman(Number - 40);
                if (Number >= 10) return "X" + ToRoman(Number - 10);
                if (Number >= 9) return "IX" + ToRoman(Number - 9);
                if (Number >= 5) return "V" + ToRoman(Number - 5);
                if (Number >= 4) return "IV" + ToRoman(Number - 4);
                if (Number >= 1) return "I" + ToRoman(Number - 1);
                throw new ArgumentOutOfRangeException("something bad happened");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Boolean ComprobarFormatoEmail(String email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
