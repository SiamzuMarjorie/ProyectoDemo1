using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    [Authorize]
    public class MapaController : Controller
    {
        public const String FOLDER_CURSOS = "Cursos";
        public const String FOLDER_CERTIFICADO = "Certificados";

        public ActionResult Mapa()
        {
            MapaBC objMapaBC = new MapaBC();
            List<SlotModel> lstSlotModel = new List<SlotModel>();
            IEnumerable<Slot> lstSlot = objMapaBC.ListarSlot();
            foreach (Slot objSlot in lstSlot.ToList())
                lstSlotModel.Add(SlotModel.FromSlot(objSlot));
            
            return View(lstSlotModel);
        }

        public ActionResult Detalle(int? IdCursoSlot, int? IdSlot)
        {
            try
            {
                CursoBC objCursoBC = new CursoBC();
                MapaBC objMapaBC = new MapaBC();

                CursoSlotModel objCursoSlotModel = new CursoSlotModel();
                List<CursoModel> lstCursoModel = new List<CursoModel>();
                IQueryable<Curso> listaCurso = new CursoBC().ListarCursosNoAsignados();
                foreach (Curso objCurso in listaCurso)
                {
                    lstCursoModel.Add(CursoModel.FromCurso(objCurso));
                }
                ViewBag.ListaCurso = lstCursoModel;
                ViewBag.SlotModel = SlotModel.FromSlot(objMapaBC.ObtenerSlot((int)IdSlot));
                CursoSlot objCursoSlot = objMapaBC.ObtenerCursoSlot((int)IdCursoSlot);
                if (objCursoSlot != null)
                    objCursoSlotModel = CursoSlotModel.FromCursoSlot(objCursoSlot);

                ViewBag.SlotName = new Slot().Nombre;

                return View(objCursoSlotModel);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al cargar el detalle del curso slot";
                return View("Error");
            }
        }

        public ActionResult DetalleSlot(int IdSlot)
        {
            try
            {
                CursoBC objCursoBC = new CursoBC();
                IQueryable<CursoSlot> lstCursoSlot = objCursoBC.ListarCursoSlotAsignado(IdSlot).Where(cs => cs.Curso != null).OrderBy(cs => cs.Orden);
                List<CursoSlotModel> lstCursoSlotModel = new List<CursoSlotModel>();
                foreach (CursoSlot objCursoSlot in lstCursoSlot)
                {
                    lstCursoSlotModel.Add(CursoSlotModel.FromCursoSlot(objCursoSlot));
                }

                Slot objSlot =new MapaBC().ObtenerSlot(IdSlot);
                IQueryable<Curso> lstCurso = new CursoBC().ListarCursosNoAsignados();
                List<CursoModel> lstCursoModel = new List<CursoModel>();
                foreach (Curso objCurso in lstCurso)
                {
                    lstCursoModel.Add(CursoModel.FromCurso(objCurso));
                }

                ViewBag.NombreSlot = objSlot.NombreSlot;
                ViewBag.IdSlot = objSlot.IdSlot;
                ViewBag.Estado = objSlot.CursosDependientes;
                ViewBag.IdCursoSlot = new CursoSlot().IdCursoSlot;
                ViewBag.ListaCursos = lstCursoModel;
                
                return View(lstCursoSlotModel);
                
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al cargar el detalle del slot";
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult GuardarCursoSlot(CursoSlotModel objCursoSlotModel)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                CursoBC objCursoBC = new CursoBC();
                Slot objSlot = objMapaBC.ObtenerSlot(objCursoSlotModel.IdSlot);
                Curso objCurso = objCursoBC.ObtenerCurso(objCursoSlotModel.IdCurso);
                CursoSlot objCursoSlot = new CursoSlot();
                if (objCursoSlotModel.IdCursoSlot == 0 && objCursoSlotModel.IdCurso != null)
                {
                    objCursoSlot = objMapaBC.ObtenerCursoSlotActivoPorIdCurso((int)objCursoSlotModel.IdCurso);
                    if (objCursoSlot != null)
                    {
                        objResultObject.OK = false;
                        objResultObject.Message = "El curso seleccionado esta asignado a un slot, seleccione otro por favor";
                        return new JsonResult() { Data = objResultObject };
                    }
                }
                else
                {
                    objCursoSlot = objMapaBC.ObtenerCursoSlotActivo(objCursoSlotModel.IdCursoSlot);
                }



                if (DateTimeHelper.FromString(objCursoSlotModel.FechaHoraInicio, true, true) <= DateTimeHelper.FromString(objCursoSlotModel.FechaHoraFin, true, true))
                {
                    if (objCurso != null)
                    {
                        objCursoSlotModel.TituloCurso = objCurso.Titulo;
                        objCursoSlotModel.HorasCurso = objCurso.Horas;
                        objCursoSlotModel.Ano = objCursoSlotModel.Ano;
                        objCursoSlotModel.UrlCurso = objCurso.UrlExamen;
                        objCursoSlotModel.PuntajeCurso = objCurso.Puntos;
                        objCursoSlotModel.CodigoCurso = objCurso.Codigo;
                        objCursoSlotModel.Descripcion = objCurso.Descripcion;
                        objCursoSlotModel.FechaHoraRegistro = DateTimeHelper.PeruDateTime.ToString("dd/MM/yyyy HH:mm");
                        objCursoSlotModel.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime.ToString("dd/MM/yyyy HH:mm");
                        objCursoSlotModel.Orden = (int)(objCursoSlot == null || objCursoSlotModel.IdCursoSlot != objCursoSlot.IdCursoSlot ? objMapaBC.ObtenerOrdenMayorCursoSlot(objCursoSlotModel.IdSlot) != null ? objMapaBC.ObtenerOrdenMayorCursoSlot(objCursoSlotModel.IdSlot).Value + 1 : 1 : objCursoSlot.Orden);
                        objCursoSlotModel.ImagenCurso = objCurso.Imagen;
                        objCursoSlotModel.Estado = Constants.Cursos.Estado.ACTIVO;
                        objCursoSlotModel.Visible = true;
                        if (objCursoSlotModel.EmiteCertificado == true)
                        {
                            if (objCursoBC.ListarConfiguracion().Count() > 0)
                            {
                                objMapaBC.GuardarCursoSlot(objCursoSlotModel.ToCursoSlot());
                                objResultObject.OK = true;
                            }
                            else
                            {
                                objResultObject.OK = false;
                                objResultObject.Message = "Debes subir el formato del certificado para los cursos.";
                            }
                        }
                        else
                        {
                            objMapaBC.GuardarCursoSlot(objCursoSlotModel.ToCursoSlot());
                            objResultObject.OK = true;

                        }
                    }
                    else
                    {
                        objResultObject.OK = false;
                        objResultObject.Message = "Debe seleccionar un curso.";
                    }
                }
                else
                {
                    objResultObject.OK = false;
                    objResultObject.Message = "La fecha y hora de inicio debe ser menor a la fecha y hora final";
                }
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al asignar el curso al slot" + ex.Message;
                objResultObject.exMessage = "Error: " + ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult GuardarSlot(SlotModel objSlotModel)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                Slot objSlot = objSlotModel.ToSlot(objSlotModel);
                
                objMapaBC.GuardarSlot(objSlot);
                objResultObject.OK = true;
               
            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error al editar en nombre del slot";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }


        [HttpPost]
        public ActionResult GuardarSlotCurso(int IdSlot, string NombreSlot)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                CursoSlot objCursoSlot = new CursoSlot();
                objCursoSlot.IdSlot = IdSlot;
                objCursoSlot.NombreSlot = NombreSlot;
                int idCursoSlot = new MapaBC().InsertarCursoSlot(objCursoSlot);
                
                objResultObject.Data = idCursoSlot;
                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error al editar el nombre del slot";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult GuardarCursoXId(int IdCursoSlot, int IdCurso, int Orden)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                CursoSlot objCursoSlot = objMapaBC.ObtenerCursoSlot(IdCursoSlot);
                Curso objCurso = new CursoBC().ObtenerCurso(IdCurso);
                objCursoSlot.IdCurso = IdCurso;
                objCursoSlot.Titulo = objCurso.Titulo;
                objCursoSlot.Estado = Constants.Cursos.Estado.ACTIVO;
                objCursoSlot.FechaHoraRegistro = DateTimeHelper.PeruDateTime;
                objCursoSlot.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                objCursoSlot.Orden = Orden;
                objMapaBC.ActualizarCursoSlot(objCursoSlot);

                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error al guardar el curso";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult ActualizarEstadoCurso(int IdCursoSlot, bool Estado)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                String sEstado = Estado ? Util.Constants.Cursos.Estado.ACTIVO : Util.Constants.Cursos.Estado.INACTIVO;
                CursoSlot objCursoSlot = objMapaBC.ObtenerCursoSlot(IdCursoSlot);
                objCursoSlot.Estado = sEstado;
                objCursoSlot.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                objMapaBC.ActualizarCursoSlot(objCursoSlot);
                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error al cambiar el estado del curso.";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult CursoDependiente(int IdSlot, bool CursosDependientes)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                objMapaBC.ActualizarCursoSlotDependiente(IdSlot, CursosDependientes);
                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error al cambiar el estado del curso.";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult CambiarOrdenCursoSlot(int IdCursoSlot, int NumeroOrden, int OrdenIdCursoSlot)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                objMapaBC.ActualizarOrdenCursoSlot(IdCursoSlot, NumeroOrden, OrdenIdCursoSlot);

                objResultObject.OK = true;

            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error al cambiar el orden del curso.";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult CambiarEstadoCursoSlot(int IdCursoSlot)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                MapaBC objMapaBC = new MapaBC();
                objMapaBC.ActualizarOrdenCursoSlot(IdCursoSlot);
                CursoSlot objCursoSlot = objMapaBC.ObtenerCursoSlot(IdCursoSlot);
                    objCursoSlot.FechaHoraFin = DateTimeHelper.PeruDateTime;
                    objCursoSlot.Estado = Constants.Cursos.Estado.HISTORIAL;
                    objCursoSlot.Orden = null;
                    objCursoSlot.IdCursoSlotDependiente = null;
                    objMapaBC.GuardarCursoSlot(objCursoSlot);

                objResultObject.OK = true;
            }
            catch (Exception ex)
            {
                objResultObject.OK = true;
                objResultObject.Message = "Ocurrió un error al pasar el curso al historial";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult DescargarCertiticados(string Codigo, int IdCursoSlot, string TituloCurso, string FechaInicio, string FechaFin)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                CursoBC objCursoBC = new CursoBC();
                Curso objCurso = objCursoBC.ObtenerCursoPorCursoSlot(IdCursoSlot);
                List<string> Certificados = new List<string>();
                if (objCurso.TieneExamen && objCurso.EmiteCertificado)
                    Certificados = objCursoBC.ListarInscripcionPorCursoSlot(IdCursoSlot).Select(i => i.Intento.OrderByDescending(n => n.Nota).FirstOrDefault().Certificado).ToList();
                if (objCurso.TieneExamen== false && objCurso.EmiteCertificado)
                    Certificados = objCursoBC.ListarInscripcionPorCursoSlot(IdCursoSlot).Select(i => i.Certificado).ToList();
                string folderCertificado = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS, Codigo, FOLDER_CERTIFICADO);
                using (ZipFile zipCertificado = new ZipFile())
                {
                    foreach (string certificado in Certificados.Distinct())
                    {
                        if (!string.IsNullOrEmpty(certificado))
                        {
                            string rutaArchivo = Path.Combine(folderCertificado, certificado);
                            string nombreArchivo = Path.GetFileName(rutaArchivo);
                            var archivo_arregloBytes = System.IO.File.ReadAllBytes(rutaArchivo);
                            zipCertificado.AddEntry(nombreArchivo, archivo_arregloBytes);
                        }
                    }
                    using (MemoryStream stream = new MemoryStream())
                    {
                        string nombreZip = string.Format("Certificados {0} {1} - {2}.zip", TituloCurso, FechaInicio.Replace("/", "-"), FechaFin.Replace("/", "-"));
                        zipCertificado.Save(stream);
                        return File(stream.ToArray(), "application/zip", nombreZip);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al descargar los certificados.";
                return View("Error");
            }
        }

    }
}
