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
    [Authorize]
    public class MapaController : BaseController
    {
        public const int INVALID_TOPIC = 1;
        public const int ERROR_MATERIAL_OBLIGATORIO = 2;

        public ActionResult Index()
        {
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                Personaje objPersonaje = new MapaBC().ObtenerPosicionPersonaje(objUsuarioLogueado.IdUsuario);
                PersonajeModel objPersonajeModel = PersonajeModel.FromPersonaje(objPersonaje);

                IQueryable<CursoSlot> lstCursosActivos = new CursoBC().ListarCursoSlotActivo().Where(cs => cs.Curso != null);
                List<CursoModel> lstCursosActivosModel = new List<CursoModel>();

                foreach (CursoSlot objCursoActivo in lstCursosActivos)
                {
                    CursoModel objCursoActivoModel = CursoModel.FromCursoSlot(objCursoActivo);
                    lstCursosActivosModel.Add(objCursoActivoModel);

                    Inscripcion objInscripcion = new MapaBC().ObtenerInscripcion(objCursoActivo.IdCursoSlot, objUsuarioLogueado.IdTrabajador);
                    objCursoActivoModel.IdInscripcion = objInscripcion.IdInscripcion;
                    objCursoActivoModel.Aprobado = objCursoActivo.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).Intento.Any(i => i.Aprobado == true);
                    objCursoActivoModel.CantidadIntentos = objInscripcion.Intento.Count(i => i.Terminado);
                }
                MapaBC objMapaBC = new MapaBC();
               List<int> IdsCurso = objMapaBC.ListarIdsCursoXTrabajador(objUsuarioLogueado.IdTrabajador).ToList();

                IQueryable<Slot> lstSlot = objMapaBC.ListarSlot();
                List<SlotModel> lstSlotModel = new List<SlotModel>();

                foreach (Slot objSlot in lstSlot)
                {
                    SlotModel objSlotModel = SlotModel.FromSlot(objSlot);
                    if (objSlot.CursoSlot.Where(s => s.Estado == Constants.Cursos.Estado.ACTIVO && s.FechaHoraFin > DateTimeHelper.PeruDateTime && IdsCurso.Contains(s.IdCurso ?? 0)).Count() > 0)
                    {
                        //Si es que en la lista de slot hay algun curso que no ha sido aprobado y tiene intentos restantes, entonces verde
                        if (objSlot.CursoSlot.Where(cs => cs.FechaHoraInicio < DateTimeHelper.PeruDateTime && cs.FechaHoraFin > DateTimeHelper.PeruDateTime && cs.Estado == Constants.Cursos.Estado.ACTIVO && IdsCurso.Contains(cs.IdCurso ?? 0)).Any(cs => cs.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).Certificado == null))
                        {
                            objSlotModel.EstadoSlot = "verde";
                        }
                        else
                        {
                            objSlotModel.EstadoSlot = "plomo";
                        }

                        lstSlotModel.Add(objSlotModel);
                    }
                    
                }

                ViewBag.CursosActivos = lstCursosActivosModel;
                ViewBag.Slots = lstSlotModel;

                return View(objPersonajeModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult ObtenerEstadoSlot(int IdSlot)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                MapaBC objMapaBC = new MapaBC();
                List<int> IdsCurso = objMapaBC.ListarIdsCursoXTrabajador(objUsuarioLogueado.IdTrabajador).ToList();
                Slot objSlot = objMapaBC.ObtenerSlot(IdSlot);
                SlotModel objSlotModel = SlotModel.FromSlot(objSlot);
                if (objSlot.CursoSlot.Where(cs => cs.FechaHoraInicio < DateTimeHelper.PeruDateTime && cs.FechaHoraFin > DateTimeHelper.PeruDateTime && cs.Estado == Constants.Cursos.Estado.ACTIVO && IdsCurso.Contains(cs.IdCurso ?? 0)).Any(cs => cs.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).Certificado == null))
                {
                    objSlotModel.EstadoSlot = "verde";
                }
                else
                {
                    objSlotModel.EstadoSlot = "plomo";
                }
                objResultObject.Code = 0;
                objResultObject.Data = objSlotModel;
              
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al cargar la lista de cursos.");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult Personaje()
        {
            return View();
        }

        public ActionResult ObtenerListaCursos(int IdSlot)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);

                MapaBC objMapaBC = new MapaBC();
                int[] IdsCurso = objMapaBC.ListarIdsCursoXTrabajador(objUsuarioLogueado.IdTrabajador).ToArray();

                List<CursoModel> lstCursoModel = new List<CursoModel>();
                IQueryable<CursoSlot> lstCursoSlot = new MapaBC().ObtenerListaCursoSlotXIdSlot(IdSlot, IdsCurso).OrderBy(cs => cs.Orden);
                if (lstCursoSlot.Count() > 0)
                {
                    foreach (CursoSlot objCursoSlot in lstCursoSlot)
                    {
                        CursoModel objCursoModel = CursoModel.FromCursoSlot(objCursoSlot);
                        
                        Inscripcion objInscripcion = objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador);
                        //Si es que en la lista de slot hay algun curso que no ha sido aprobado y tiene intentos restantes, entonces verde
                        if (objInscripcion.Certificado == null)
                        {
                            objCursoModel.EstadoCurso = "verde";
                        }
                        else
                        {
                            objCursoModel.EstadoCurso = "plomo";
                        }

                        lstCursoModel.Add(objCursoModel);
                    }
                    objResultObject.Data = lstCursoModel;
                }
                else
                {
                    objResultObject.Code = INVALID_TOPIC;
                    objResultObject.Message = "No hay un cursos asignados en esta ubicación.";
                }
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al cargar la lista de cursos.");
            }
            return new JsonResult() { Data = objResultObject };
        }


        public ActionResult ObtenerSlot(int IdCursoSlot)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                CursoSlot objCursoSlot = objMapaBC.ObtenerCursoSlot(IdCursoSlot);

                if (objCursoSlot != null)
                {
                    bool CursoDisponible = true;
                    if (objCursoSlot.Slot.CursosDependientes == true)
                    {
                        if (objCursoSlot.Orden > 1)
                        {
                            CursoSlot objCursoSlotAnterior = objMapaBC.ObtenerOrdenCursoSlot(objCursoSlot.Orden.Value - 1, objCursoSlot.IdSlot);
                            Inscripcion objInscripcionAnterior = objCursoSlotAnterior.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador);

                            if (objCursoSlot.Curso.TieneExamen == true)
                            {

                                if (objInscripcionAnterior == null || !objInscripcionAnterior.Intento.Any(x => x.Aprobado == true))
                                {
                                    CursoDisponible = false;
                                }
                            }
                            else
                            {
                                if (objInscripcionAnterior == null || objInscripcionAnterior.ContenidoCursoInscripcion.Count == 0 || objInscripcionAnterior.ContenidoCursoInscripcion.Any(ci => ci.FechaHoraRevision == null))
                                {
                                    CursoDisponible = false;
                                }
                            }
                        }
                    }
                    if (CursoDisponible)
                    {
                        Inscripcion objInscripcion = new MapaBC().ObtenerInscripcion(objCursoSlot.IdCursoSlot, objUsuarioLogueado.IdTrabajador);
                        CursoModel objCursoModel = CursoModel.FromCursoSlot(objCursoSlot);
                        objCursoModel.IdInscripcion = objInscripcion.IdInscripcion;
                        objCursoModel.Aprobado = objInscripcion.Intento.Any(i => i.Aprobado == true);
                        objCursoModel.CantidadIntentos = objInscripcion.Intento.Count(i => i.Terminado);
                        if ((objCursoSlot.Curso.TieneExamen && objCursoSlot.Curso.EmiteCertificado) || (objCursoSlot.Curso.TieneExamen && objCursoSlot.Curso.EmiteCertificado == false))
                        {
                            objCursoModel.Terminado = objInscripcion.Intento.Any(i => i.Terminado == true);
                            objCursoModel.Puntaje = objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).Intento.Max(i => i.Puntaje);
                            if (!objInscripcion.Intento.Any(a => a.Aprobado == true) && (objCursoSlot.Intentos == null || objInscripcion.Intento.Count(t => t.Terminado) < objCursoSlot.Intentos))
                                objCursoModel.Estado = Constants.Cursos.Estado.ENCURSO;
                            else
                                objCursoModel.Estado = Constants.Cursos.Estado.TERMINADO;
                        }
                        if (objCursoSlot.Curso.TieneExamen == false && objCursoSlot.Curso.EmiteCertificado)
                        {
                            objCursoModel.Terminado = objCursoSlot.Inscripcion.FirstOrDefault(i => i.Certificado != null) != null ? true: false;
                            if (objCursoSlot.Inscripcion != null)
                                objCursoModel.Puntaje = objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).Puntaje;
                            if (objInscripcion.ContenidoCursoInscripcion.Count(ci => ci.FechaHoraRevision != null) == objCursoSlot.Curso.ContenidoCurso.Count)
                                objCursoModel.Estado = Constants.Cursos.Estado.TERMINADO;
                            else
                                objCursoModel.Estado = Constants.Cursos.Estado.ENCURSO;
                        }

                        objCursoModel.IdCurso = objCursoSlot.Curso.IdCurso;
                        objCursoModel.Titulo = objCursoSlot.Curso.Titulo;
                        objCursoModel.TieneExamen = objCursoSlot.Curso.TieneExamen;
                        objCursoModel.CantidadContenidoCurso = objCursoSlot.Curso.ContenidoCurso.Count;
                        if (objCursoSlot.Curso.ContenidoCurso.Count == 1)
                        {
                            objCursoModel.UrlContenidoCurso = objCursoSlot.Curso.ContenidoCurso.FirstOrDefault().Archivo;
                            objCursoModel.TipoContenidoCurso = objCursoSlot.Curso.ContenidoCurso.FirstOrDefault().TipoArchivo;
                        }
                        objCursoModel.Certificado = objInscripcion.Certificado;

                        objResultObject.Code = 0;
                        objResultObject.Data = objCursoModel;
                    }
                    else
                    {
                        objResultObject.Code = 1;
                        objResultObject.Message = "Debes completar el curso anterior para poder ingresar a este curso.";
                    }
                }

            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al cargar los cursos.");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult ObtenerListaContenidoCurso(int IdCurso)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);

                List<ContenidoCursoModel> lstContenidoCursoModel = new List<ContenidoCursoModel>();
                IQueryable<ContenidoCurso> lstContenidoCursoSlot = new MapaBC().ListarContenidoCurso(IdCurso);

                foreach (ContenidoCurso objContenidoCurso in lstContenidoCursoSlot)
                {
                    ContenidoCursoModel objCursoModel = ContenidoCursoModel.FromContenidoCurso(objContenidoCurso);
                    objCursoModel.UrlArchivo = Url.Content(objCursoModel.UrlArchivo).Replace("'", "\\'");
                    ContenidoCursoInscripcion objContenidoCursoInscripcion = objContenidoCurso.ContenidoCursoInscripcion.FirstOrDefault(cci => cci.Inscripcion.IdTrabajador == objUsuarioLogueado.IdTrabajador);
                    objCursoModel.Visto = objContenidoCursoInscripcion != null && objContenidoCursoInscripcion.FechaHoraRevision != null;
                    lstContenidoCursoModel.Add(objCursoModel);
                }

                objResultObject.Data = lstContenidoCursoModel;

            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al cargar la lista de cursos.");
            }
            return new JsonResult() { Data = objResultObject };
        }

        

        public ActionResult Material(MaterialModel objMaterialModel)
        {
            return PartialView(objMaterialModel);
        }

        public ActionResult ObtenerCertificado(int IdCurso)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                string pathPDF = null;
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                CursoSlot objCursoSlot = new MapaBC().ObtenerCursoSlotPorCursoActivo(IdCurso);
                Inscripcion objInscripcion = new CursoBC().ObtenerInscripcionPorTrabajadorYCursoSlotAprobado(objUsuarioLogueado.IdTrabajador, objCursoSlot.IdCursoSlot);

                if (objCursoSlot.Curso.TieneExamen && objCursoSlot.Curso.EmiteCertificado)
                {
                    Intento objIntento = objInscripcion.Intento.OrderByDescending(i => i.Nota).FirstOrDefault(a => a.Aprobado == true);
                    new ReporteBC().RegistrarDescargaCertificado(objIntento.IdIntento);
                    pathPDF = CursoModel.URL_CERTIFICADO.Replace(CursoModel.CODIGO_CURSO, objCursoSlot.Codigo).Replace(CursoModel.ARCHIVO, objIntento.Certificado);
                }

                if (objCursoSlot.Curso.EmiteCertificado && objCursoSlot.Curso.TieneExamen == false)
                {
                    if (objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).FechaHoraDescargaCertificado == null)
                    {
                        objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).FechaHoraDescargaCertificado = DateTimeHelper.PeruDateTime;
                        new MapaBC().RegistrarDescargaCertificado(objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).IdInscripcion);
                    }

                    pathPDF = CursoModel.URL_CERTIFICADO.Replace(CursoModel.CODIGO_CURSO, objCursoSlot.Codigo).Replace(CursoModel.ARCHIVO, objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador).Certificado);
                }

                objResultObject.Code = 0;
                objResultObject.Data = Url.Content(pathPDF);
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al descargar el certificado");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult RegistrarFechaIngresoCurso(int IdCursoSlot, int IdInscripcion)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                CursoSlot objCursoSlot = new MapaBC().ObtenerCursoSlot(IdCursoSlot);

                new CursoBC().ObtenerGuardarFechas(objCursoSlot.IdCursoSlot, objUsuarioLogueado.IdTrabajador);
                ActividadUsuario objActividadUsuario = new ActividadUsuario();
                objActividadUsuario.IdUsuario = objUsuarioLogueado.IdUsuario;
                objActividadUsuario.FechaHora = DateTimeHelper.PeruDateTime;
                objActividadUsuario.TipoActividad = Constants.TipoActividad.CURSO;
                objActividadUsuario.IdCursoSlot = objCursoSlot.IdCursoSlot;
                new UsuarioBC().RegistrarFechaHoraIngresoCurso(objActividadUsuario);

                objResultObject.Code = 0;

            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al guardar la fecha");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult RegistrarIntento(int IdInscripcion, int IdCursoSlot)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                CursoSlot objCursoSlot = new MapaBC().ObtenerCursoSlot(IdCursoSlot);
                IQueryable<MaterialInscripcion> lstMaterialInscripcion = new CursoBC().ObtenerMaterialObligatorio(IdInscripcion, (int)objCursoSlot.IdCurso);
                if (lstMaterialInscripcion.Count() > 0)
                {
                    objResultObject.Code = ERROR_MATERIAL_OBLIGATORIO;
                    objResultObject.Message = "Para poder ingresar, debes ver los siguentes materiales: ";
                    foreach (string nombreMaterial in lstMaterialInscripcion.Select(m => m.Nombre))
                    {
                        objResultObject.Message += "<br>";
                        objResultObject.Message += nombreMaterial;
                    }

                }
                else
                {
                    if (!new MapaBC().ObtenerInscripcion(IdInscripcion).Intento.Any(i => i.Terminado == false))
                    {
                        int NumeroIntento = new MapaBC().ObtenerMayorIntento(IdInscripcion);
                        Intento objIntento = new Intento();
                        objIntento.IdInscripcion = IdInscripcion;
                        objIntento.FechaHoraRegistro = DateTimeHelper.PeruDateTime;
                        objIntento.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                        objIntento.Terminado = false;
                        objIntento.NumeroIntento = NumeroIntento == null ? 1 : NumeroIntento + 1;
                        new MapaBC().GuardarIntento(objIntento);
                    }
                    objResultObject.Code = 0;
                }

            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al guardar la fecha");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult RegistrarPosicionPersonaje(int PosicionX, int PosicionY)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                new MapaBC().RegistrarPosicionPersonaje(objUsuarioLogueado.IdUsuario, PosicionX, PosicionY);

                objResultObject.Code = 0;
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al guardar la posición del personaje");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult RegistrarAlias(string Alias)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                Personaje objPersonaje = new MapaBC().RegistrarAlias(objUsuarioLogueado.IdUsuario, Alias);

                objResultObject.Code = 0;
                objResultObject.Data = objPersonaje.Alias;
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al guardar tú alias");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult ActualizarAvatar(string Avatar)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                Personaje objPersonaje = new MapaBC().RegistrarAvatar(objUsuarioLogueado.IdUsuario, Avatar);

                objResultObject.Code = 0;
                objResultObject.Data = objPersonaje.Avatar;
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al guardar tú avatar");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult ObtenerMaterialInscripcion(int IdMaterial, int IdInscripcion)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MaterialInscripcion objMaterialInscripcion = new CursoBC().ObtenerMaterialInscripcionPorIdMaterialYIdInscripcion(IdMaterial, IdInscripcion);
                new CursoBC().ActualiazarVistoMaterialInscripcion(objMaterialInscripcion.IdMaterialInscripcion);
                objResultObject.Code = 0;
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al actualizar la visualización del material");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult RegistrarVistaContenidoCurso(int IdContenidoCurso, int IdCurso)
        {
            ResultObject objResultObject = new ResultObject();

            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioLogueado;
                new CursoBC().RegistrarVistaContenidoCurso(IdContenidoCurso, objUsuarioLogueado.IdTrabajador);

                string Certificado = null;
                CursoSlot objCursoSlot = new MapaBC().ObtenerCursoSlotPorCursoActivo(IdCurso);
                if ((objCursoSlot.Curso.EmiteCertificado == true && objCursoSlot.Curso.Certificado != null) && objCursoSlot.Curso.TieneExamen == false)
                {
                    string nombreCertificadoEmitido = null;
                    Inscripcion objInscripcion = objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objUsuarioLogueado.IdTrabajador && i.IdCursoSlot == objCursoSlot.IdCursoSlot);

                    if (objInscripcion.ContenidoCursoInscripcion.Count(ci => ci.FechaHoraRevision != null) == objCursoSlot.Curso.ContenidoCurso.Count)
                    {
                        nombreCertificadoEmitido = GenerarCertificado(Certificado, objCursoSlot, objUsuarioLogueado.IdTrabajador, null, objCursoSlot.Curso.Puntos);
                    }
                    new CursoBC().RegistrarResultadoCursoEnInscripcion(objCursoSlot.Curso.Codigo, objUsuarioLogueado.IdTrabajador, nombreCertificadoEmitido);
                }

                objResultObject.Code = 0;
            }
            catch (Exception ex)
            {
                objResultObject.CaptureException(ex, "Ocurrió un error al actualizar la visualización del material");
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult ActualizarPersonaje(string Alias, string Avatar)
        {
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                new MapaBC().ActualizarPersonaje(objUsuarioLogueado.IdUsuario, Alias, Avatar);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = "Ocurrió un problema al guardar el personaje";
                return View("Error");
            }
        }

        public ActionResult DescargarMaterialDocumento(string UrlArchivo, string Archivo)
        {
            try
            {
                string url = Server.MapPath(UrlArchivo);
                byte[] fileBytes = System.IO.File.ReadAllBytes(url);
                string extension = System.IO.Path.GetExtension(Archivo);
                string mimeType = string.Empty;
                MemoryStream ms = new MemoryStream(fileBytes);
                switch (extension)
                {
                    case ".xlsx": mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; break;
                    case ".xls": mimeType = "application/vnd.ms-excel"; break;
                    case ".docx": mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;
                    case ".doc": mimeType = "application/msword"; break;
                    case ".pptx": mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"; break;
                    case ".ppt": mimeType = "application/vnd.ms-powerpoint"; break;
                    case ".pdf": mimeType = "application/pdf"; break;
                    case ".txt": mimeType = "text/plain"; break;
                    case ".csv": mimeType = "text/csv"; break;
                }
                return File(ms, mimeType, Archivo);
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = "Ocurrió un problema al descargar el documento";
                return View("Error");
            }
        }


    }
}
