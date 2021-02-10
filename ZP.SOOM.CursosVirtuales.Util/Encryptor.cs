using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZP.SOOM.CursosVirtuales.Util
{
    public class Encryptor
    {
        public static String SHA256Hash(String sPalabra)
        {
            using (SHA256 objSHA256 = SHA256.Create())
            {
                byte[] bytes = objSHA256.ComputeHash(Encoding.UTF8.GetBytes(sPalabra));
                StringBuilder objStringBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    objStringBuilder.Append(bytes[i].ToString("x2"));
                return objStringBuilder.ToString();
            }  
        }
    }
}
