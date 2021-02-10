using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using WebPush;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.DL.DA;
using ZP.SOOM.CursosVirtuales.Util;
using System.Configuration;
using System.Net.Mail;

namespace ZP.SOOM.CursosVirtuales.BL.BC
{
    public class NotificacionBC
    {
        public int RegistrarSuscripcionNotificacion(SuscripcionNotificacion objSuscripcionNotificacion)
        {
            try
            {
                return new SuscripcionNotificacionDA().InsertarSuscripcionNotificacion(objSuscripcionNotificacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarSuscripcionNotificacion(SuscripcionNotificacion objSuscripcionNotificacion)
        {
            try
            {
                new SuscripcionNotificacionDA().EliminarSuscripcionNotificacion(objSuscripcionNotificacion);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EnviarNotificacionPush(int IdUsuario, String Titulo, String Texto, String Url)
        {
            try
            {
                IQueryable<SuscripcionNotificacion> lstSuscripcionNotificacion = new SuscripcionNotificacionDA().ListarSuscripcionesNotificacionUsuario(IdUsuario);

                VapidDetails objVapidDetails = new VapidDetails(ConfigurationManager.AppSettings["UrlSOOM"], ConfigurationManager.AppSettings["PushPublicKey"], ConfigurationManager.AppSettings["PushPrivateKey"]);
                foreach (SuscripcionNotificacion objSuscripcionNotificacion in lstSuscripcionNotificacion)
                {
                    PushSubscription objPushSubscription = new PushSubscription(objSuscripcionNotificacion.Endpoint, objSuscripcionNotificacion.PS56DH, objSuscripcionNotificacion.Auth);
                    WebPushClient objWebPushClient = new WebPushClient();
                    JObject jData = new JObject();
                    jData["title"] = Titulo;
                    jData["body"] = Texto;
                    jData["url"] = Url;

                    objWebPushClient.SendNotification(objPushSubscription, jData.ToString(), objVapidDetails);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EnviarEmail(String Asunto, String Contenido, String[] Destinatarios)
        {
            try
            { 
                MailMessage objMailMessage = new MailMessage();
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["SMTPCorreo"], "Ciudad Positiva");

                foreach (String Destinatario in Destinatarios)
                    objMailMessage.To.Add(Destinatario);
                objMailMessage.Subject = Asunto;
                objMailMessage.IsBodyHtml = true;
                objMailMessage.Body = Contenido;

                Mailer.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
