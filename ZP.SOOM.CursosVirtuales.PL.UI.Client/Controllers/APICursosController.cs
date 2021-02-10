using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GemBox.Presentation;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.PL.UI.Client.Models;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Controllers
{
    public class APICursosController : BaseController
    {
        //
        // GET: /APICursos/
        public const int SIN_INTENTOS = 1;

        public ActionResult RegistrarResultadoCurso(String Codigo, double Nota, double NotaMaxima, bool Aprobado, int? IdTrabajador)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                string Certificado = null;
                string nombreCertificadoEmitido = null;

                if (IdTrabajador == null)
                    IdTrabajador = UsuarioModel.FromString(User.Identity.Name).IdTrabajador;

                if (Aprobado)
                {
                    Curso objCurso = new CursoBC().ObtenerCursoPorCodigo(Codigo);
                    CursoSlot objCursoSlot = new MapaBC().ObtenerCursoSlotPorCursoActivo(objCurso.IdCurso);
                    double? PuntajeMaximo = objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == IdTrabajador.Value).Intento.Max(i => i.Puntaje);
                    double? NuevoPuntaje = (objCursoSlot.Puntos * Nota) / NotaMaxima;

                    if ((objCursoSlot.Curso.EmiteCertificado == true && objCursoSlot.Curso.Certificado != null) && (PuntajeMaximo == null || (NuevoPuntaje != null && NuevoPuntaje.Value > PuntajeMaximo.Value)))
                    {
                        nombreCertificadoEmitido = GenerarCertificado(Certificado, objCursoSlot, IdTrabajador.Value, Nota, NuevoPuntaje.Value);
                    }
                }

                new CursoBC().RegistrarResultadoCurso(Codigo, Nota, NotaMaxima, Aprobado, IdTrabajador.Value, nombreCertificadoEmitido);

                double puntajeTrabajador = new MapaBC().ObtenerSumaPuntajeTrabajador(IdTrabajador.Value);

                objResultObject.Data = puntajeTrabajador;
                objResultObject.Code = 0;
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al registrar el resultado del curso.");
                Log(ex, "Ocurrió un error al registrar el resultado del curso.");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult RegistrarRespuestas(String Codigo, string Pregunta, string Respuesta, bool Correcto, int? IdTrabajador)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                if (IdTrabajador == null)
                    IdTrabajador = UsuarioModel.FromString(User.Identity.Name).IdTrabajador;

                new CursoBC().RegistrarRespuestas(Codigo, Pregunta, Respuesta, Correcto, IdTrabajador.Value);
                objResultObject.Code = 0;

            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al registrar la respuesta.");
                Log(ex, "Ocurrió un error al registrar la respuesta.");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult ReintentarExamen(String Codigo, int? IdTrabajador)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                if (IdTrabajador == null)
                    IdTrabajador = UsuarioModel.FromString(User.Identity.Name).IdTrabajador;

                Curso objCurso = new CursoBC().ObtenerCursoPorCodigo(Codigo);
                CursoSlot objCursoSlot = new MapaBC().ObtenerCursoSlotPorCursoActivo(objCurso.IdCurso);
                int CantidadIntentos = objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == IdTrabajador).Intento.Count(i => i.Terminado);
                int? IntentosPermitidos = objCursoSlot.Intentos;
                if (IntentosPermitidos == null || CantidadIntentos < IntentosPermitidos)
                    return RedirectToAction("RegistrarIntento", "Mapa", new {IdInscripcion = objCursoSlot.Inscripcion.FirstOrDefault().IdInscripcion, IdCursoSlot = objCursoSlot.IdCursoSlot });
                else
                {
                    objResultObject.Code = SIN_INTENTOS;
                    objResultObject.Message = "¡Ya no cuentas con más intentos!";
                }
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un al reintentar el examen.");
                Log(ex, "Ocurrió un al reintentar el examen.");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public void Log(Exception ex, String Message)
        {
            String Usuario = "No autenticado";

            if (User.Identity.IsAuthenticated && String.IsNullOrWhiteSpace(User.Identity.Name))
                Usuario = UsuarioModel.FromString(User.Identity.Name).Username;

            string path = Server.MapPath("~/Log.txt");

            using (StreamWriter outputFile = new StreamWriter(path, true))
            {
                outputFile.WriteLine(DateTimeHelper.PeruDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + Message + ex.Message + ex.StackTrace);
            }
        }
    }
}
