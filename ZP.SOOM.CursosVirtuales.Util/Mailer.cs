using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ZP.SOOM.CursosVirtuales.Util
{
    public class Mailer
    {
        public static void Send(MailMessage objMailMessage)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = ConfigurationManager.AppSettings["SMTPSSL"] == "true";
                smtp.Host = ConfigurationManager.AppSettings["SMTPHost"];
                smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPUser"], ConfigurationManager.AppSettings["SMTPPassword"]);

                ServicePointManager.ServerCertificateValidationCallback =

               delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
               { return true; };

                smtp.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
