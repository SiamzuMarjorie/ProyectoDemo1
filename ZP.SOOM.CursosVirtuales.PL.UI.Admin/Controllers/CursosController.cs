using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZP.SOOM.CursosVirtuales.Util;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.BL.BC;
using System.IO;
using System.Configuration;


namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Controllers
{
    [Authorize]
    public class CursosController : Controller
    {
        public const String FOLDER_CURSOS = "Cursos";
        public const String FOLDER_HTML = "ExamenHtml5";
        public const String FOLDER_CONTENIDOCURSO = "ContenidoCurso";
        public const String FOLDER_MATERIAL = "Materiales";
        public const String FOLDER_CERTIFICADO = "Certificados";
        public const String FOLDER_PLANTILLA_CERTIFICADO = "PlantillaCertificado";
        public const String NOMBRESCORM = "story.html";
        public const String NOMBREEXAMEN = "story.html";

        public ActionResult Cursos()
        {
            return View();
        }

        public ActionResult SeccionDatosCursos(string opcion)
        {
            try
            {
                List<CursoModel> lstCursoModel = new List<CursoModel>();
                List<CursoSlotModel> lstCursoSlotModel = new List<CursoSlotModel>();
                CursoBC objCursoBC = new CursoBC();
                MapaBC objMapaBC = new MapaBC();
                if (!string.IsNullOrEmpty(opcion) && opcion == Constants.Cursos.Secciones.HISTORIAL)
                {
                    foreach (CursoSlot objCursoSlot in objMapaBC.ListarCursoSlot().Where(x => x.Estado == Constants.Cursos.Estado.HISTORIAL && !x.Curso.Eliminado))
                        lstCursoSlotModel.Add(CursoSlotModel.FromCursoSlot(objCursoSlot));
                    return PartialView("_DatosHistorial", lstCursoSlotModel);
                }
                if (!string.IsNullOrEmpty(opcion) && opcion == Constants.Cursos.Secciones.ARCHIVADOS)
                {
                    foreach (Curso objCurso in objCursoBC.ListarCurso().Where(c => c.Estado == Constants.Cursos.Estado.ARCHIVADO && !c.Eliminado))
                        lstCursoModel.Add(CursoModel.FromCurso(objCurso));
                    return PartialView("_DatosArchivados", lstCursoModel);
                }
                foreach (Curso objCurso in objCursoBC.ListarCurso().Where(c => c.Estado == Constants.Cursos.Estado.ACTIVO && !c.Eliminado))
                    lstCursoModel.Add(CursoModel.FromCurso(objCurso));

                Configuracion objConfiguracion = objCursoBC.ObtenerConfiguracionPorCodigo(Constants.Configuracion.FORMATO_CERTIFICADO);

                if (objConfiguracion != null)
                    ViewBag.Certificado = objConfiguracion.Valor;

                return PartialView("_DatosCursos", lstCursoModel);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al cargar datos de los cursos";
                return View("Error");
            }
        }

        public ActionResult SeccionTablaCurso(string opcion, string Tipo, string Valor, string estadoCurso)
        {
            try
            {
                List<CursoModel> lstCursoModel = new List<CursoModel>();
                List<CursoSlotModel> lstCursoSlotModel = new List<CursoSlotModel>();
                CursoBC objCursoBC = new CursoBC();
                MapaBC objMapaBC = new MapaBC();
                if (string.IsNullOrEmpty(estadoCurso))
                {
                    foreach (Curso objCurso in objCursoBC.FiltarCurso(Tipo, Valor, Constants.Cursos.Estado.ACTIVO))
                        lstCursoModel.Add(CursoModel.FromCurso(objCurso));
                    return PartialView("_TablaCurso", lstCursoModel);
                }
                else if (estadoCurso == Constants.Cursos.Estado.ARCHIVADO)
                {
                    foreach (Curso objCurso in objCursoBC.FiltarCurso(Tipo, Valor, Constants.Cursos.Estado.ARCHIVADO))
                        lstCursoModel.Add(CursoModel.FromCurso(objCurso));
                    return PartialView("_TablaCursoArchivado", lstCursoModel);
                }
                else
                {
                    foreach (CursoSlot objCursoSlot in objMapaBC.FiltarCursoSlot(Tipo, Valor, Constants.Cursos.Estado.HISTORIAL))
                        lstCursoSlotModel.Add(CursoSlotModel.FromCursoSlot(objCursoSlot));
                    return PartialView("_TablaCursoHistorial", lstCursoSlotModel);
                }

            }
            catch (Exception ex)
            {

                ViewBag.Msg = "Ocurrió un error al cargar datos de los cursos";
                return View("Error");
            }
        }

        public ActionResult Material(MaterialModel objMaterialModel)
        {
            return PartialView(objMaterialModel);
        }

        public ActionResult ContenidoCurso(ContenidoCursoModel objContenidoCursoModel)
        {
            if (objContenidoCursoModel.TipoArchivo == Constants.Cursos.ContenidoCurso.SCORM)
            {
                objContenidoCursoModel.Archivo = NOMBRESCORM;
            }
            return PartialView("_ContenidoCurso", objContenidoCursoModel);
        }

        public ActionResult Examen(CursoModel objCursoModel)
        {
            return PartialView("_Examen", objCursoModel);
        }

        public ActionResult Certificado(CursoModel objCursoModel)
        {
            return PartialView("_Certificado", objCursoModel);
        }

        public ActionResult NuevoCurso()
        {
            List<GrupoModel> lstGrupoModel = new List<GrupoModel>();
            IEnumerable<Grupo> lstGrupo = new MapaBC().ListarGrupo().OrderBy(g => g.Nombre);
            foreach (Grupo objGrupo in lstGrupo.ToList())
                lstGrupoModel.Add(GrupoModel.FromGrupo(objGrupo, null, null));

            ViewBag.Grupos = lstGrupoModel;
            return View("DetalleCurso");
        }

        public ActionResult Detalle(int IdCurso)
        {
            CursoBC objCursoBC = new CursoBC();
            try
            {
                CursoModel objCursoModel = CursoModel.FromCurso(objCursoBC.ObtenerCurso(IdCurso));

                List<GrupoModel> lstGrupoModel = new List<GrupoModel>();
                IEnumerable<Grupo> lstGrupo = new MapaBC().ListarGrupo().OrderBy(g => g.Nombre);
                foreach (Grupo objGrupo in lstGrupo.ToList())
                    lstGrupoModel.Add(GrupoModel.FromGrupo(objGrupo, null, IdCurso));

                ViewBag.Grupos = lstGrupoModel;

                return View("DetalleCurso", objCursoModel);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al cargar el detalle del curso";
                return View("Error");
            }
        }

        //Agregar y Modificar Curso con sus respectivos materiales
        [HttpPost]
        public ActionResult GuardarCurso(
            CursoModel objCursoModel, int[] IdMaterial, string[] NombreMaterial, HttpPostedFileBase[] Archivo,
            string[] TipoMaterial, int[] Obligatorio, HttpPostedFileBase ImagenUploadCurso, string[] linkCurso,
            HttpPostedFileBase[] Contenido, string[] NombreContenido, string[] TipoContenido, int[] IdContenido,
            HttpPostedFileBase[] Examen,
            HttpPostedFileBase Certificado, string NombreCertificado,
            int [] IdGrupo
            )
        {
            ResultObject objResultObject = new ResultObject();

            try
            {
                CursoBC objCursoBC = new CursoBC();
                if ((objCursoModel.EmiteCertificado == true && NombreCertificado != null) || objCursoModel.EmiteCertificado == false)
                {
                    if ((objCursoModel.TieneExamen == true && Examen != null) || objCursoModel.TieneExamen == false)
                    {
                        if (TipoContenido == null || TipoContenido.Where(tc => tc == Constants.Cursos.ContenidoCurso.SCORM).Count() <= 1)
                        {

                            if (IdMaterial == null)
                            {
                                IdMaterial = new int[] { };
                            }

                            if (IdContenido == null)
                            {
                                IdContenido = new int[] { };
                            }

                            if (Contenido == null || Contenido.All(n => n == null || n.FileName.Length <= 100))
                            {
                                if (Archivo == null || Archivo.All(n => n == null || n.FileName.Length <= 100))
                                {
                                    if (NombreContenido == null || NombreContenido.All(n => !String.IsNullOrWhiteSpace(n)))
                                    {
                                        if (NombreMaterial == null || NombreMaterial.All(n => !String.IsNullOrWhiteSpace(n)))
                                        {
                                            Curso objCursoConCodigo = objCursoBC.ObtenerCursoPorCodigo(objCursoModel.Codigo);

                                            if (objCursoConCodigo == null || objCursoConCodigo.IdCurso == objCursoModel.IdCurso)
                                            {
                                                //Crear folder materiales,ExamenHTML5, ContenidoCurso, certificado y plantilla de certificado
                                                string folderMateriales = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS, objCursoModel.Codigo, FOLDER_MATERIAL);
                                                string folderHtml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS, objCursoModel.Codigo, FOLDER_HTML);
                                                string folderContenidoCurso = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS, objCursoModel.Codigo, FOLDER_CONTENIDOCURSO);
                                                string folderCertificado = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS, objCursoModel.Codigo, FOLDER_CERTIFICADO);
                                                string folderPlantillaCertificado = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS, objCursoModel.Codigo, FOLDER_PLANTILLA_CERTIFICADO);
                                                System.IO.Directory.CreateDirectory(folderMateriales);
                                                System.IO.Directory.CreateDirectory(folderHtml);
                                                System.IO.Directory.CreateDirectory(folderContenidoCurso);
                                                System.IO.Directory.CreateDirectory(folderCertificado);
                                                System.IO.Directory.CreateDirectory(folderPlantillaCertificado);

                                                if (objCursoModel.IdCurso != 0)
                                                {
                                                    //Eliminar materiales borrados por el usuario
                                                    IEnumerable<Material> lstMaterial = objCursoBC.ObtenerCurso(objCursoModel.IdCurso).Material.Where(m => IdMaterial.All(i => m.IdMaterial != i));
                                                    foreach (Material objMaterial in lstMaterial)
                                                    {
                                                        string ruta = Path.Combine(folderMateriales, objMaterial.Archivo);
                                                        if (System.IO.File.Exists(ruta))
                                                            System.IO.File.Delete(ruta);
                                                        objCursoBC.EliminarMaterial(objMaterial.IdMaterial);
                                                    }

                                                    //Eliminar contenidos
                                                    IEnumerable<ContenidoCurso> lstContenidoCurso = objCursoBC.ObtenerCurso(objCursoModel.IdCurso).ContenidoCurso.Where(cc => IdContenido.All(i => cc.IdContenidoCurso != i));
                                                    foreach (ContenidoCurso objContenidoCurso in lstContenidoCurso)
                                                    {
                                                        string ruta = Path.Combine(folderContenidoCurso, objContenidoCurso.Archivo);
                                                        if (System.IO.File.Exists(ruta))
                                                        {
                                                            if (objContenidoCurso.TipoArchivo != Constants.Cursos.ContenidoCurso.SCORM)
                                                                System.IO.File.Delete(ruta);
                                                            else
                                                            {
                                                                System.IO.DirectoryInfo di = new DirectoryInfo(folderContenidoCurso);
                                                                foreach (FileInfo file in di.GetFiles())
                                                                {
                                                                    file.Delete();
                                                                }
                                                                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                                                                {
                                                                    dir.Delete(true);
                                                                }
                                                            }

                                                        }
                                                        objCursoBC.EliminarContenidoCurso(objContenidoCurso.IdContenidoCurso);
                                                    }
                                                }

                                                //Subir nuevos contenidos
                                                if (Contenido != null)
                                                {
                                                    foreach (HttpPostedFileBase objContenido in Contenido)
                                                    {
                                                        if (objContenido != null)
                                                        {
                                                            string fileNombre = objContenido.FileName;
                                                            string rutafinal = Path.Combine(folderContenidoCurso, fileNombre);
                                                            string fileExtension = Path.GetExtension(rutafinal);
                                                            if (fileExtension == Constants.Extenciones.ZIP || fileExtension == Constants.Extenciones.PDF || fileExtension == Constants.Extenciones.VIDEO)
                                                            {
                                                                if (fileExtension == Constants.Extenciones.ZIP)
                                                                {
                                                                    objContenido.SaveAs(rutafinal);
                                                                    using (var fileScorm = Ionic.Zip.ZipFile.Read(rutafinal))
                                                                    {
                                                                        foreach (var file in fileScorm)
                                                                        {
                                                                            file.Extract(folderContenidoCurso);
                                                                        }
                                                                    }
                                                                    if (!System.IO.File.Exists(Path.Combine(folderContenidoCurso, NOMBRESCORM)))
                                                                    {
                                                                        System.IO.DirectoryInfo di = new DirectoryInfo(folderContenidoCurso);

                                                                        foreach (FileInfo file in di.GetFiles())
                                                                        {
                                                                            file.Delete();
                                                                        }
                                                                        foreach (DirectoryInfo dir in di.EnumerateDirectories())
                                                                        {
                                                                            dir.Delete(true);
                                                                        }
                                                                        objResultObject.OK = false;
                                                                        objResultObject.Message = "El ZIP no tiene el formato correcto. Verificar que contenga el archivo story.html.";
                                                                        return new JsonResult() { Data = objResultObject };
                                                                    }
                                                                    else
                                                                    {
                                                                        System.IO.File.Delete(rutafinal);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    objContenido.SaveAs(rutafinal);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objResultObject.OK = false;
                                                                objResultObject.Message = "Los contenidos de los cursos deben tener los formatos en .zip, .pdf o .mp4";
                                                                return new JsonResult() { Data = objResultObject };
                                                            }
                                                        }
                                                    }
                                                }

                                                //Subir Examen
                                                if (Examen != null)
                                                {
                                                    foreach (HttpPostedFileBase objExamen in Examen)
                                                    {
                                                        if (objExamen != null)
                                                        {
                                                            string fileNombre = objExamen.FileName;
                                                            string rutafinal = Path.Combine(folderHtml, fileNombre);
                                                            string fileExtension = Path.GetExtension(rutafinal);
                                                            if (fileExtension == Constants.Extenciones.ZIP)
                                                            {
                                                                //ELIMINOS LOS ARCHIVOS ANTERIORES
                                                                System.IO.DirectoryInfo di = new DirectoryInfo(folderHtml);

                                                                foreach (FileInfo file in di.GetFiles())
                                                                {
                                                                    file.Delete();
                                                                }
                                                                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                                                                {
                                                                    dir.Delete(true);
                                                                }
                                                                //GUARDO EL NUEVO ZIP
                                                                objExamen.SaveAs(rutafinal);
                                                                // EXTRAER ZIP
                                                                using (var fileScorm = Ionic.Zip.ZipFile.Read(rutafinal))
                                                                {
                                                                    foreach (var file in fileScorm)
                                                                    {
                                                                        file.Extract(folderHtml);
                                                                    }
                                                                }
                                                                // VALIADR SI ES QUE EXISTE EL ARCHIVO story.html
                                                                if (!System.IO.File.Exists(Path.Combine(folderHtml, NOMBREEXAMEN)))
                                                                {
                                                                    //ELIMINOS LOS ARCHIVOS QUE YA SE EXTRAYERON
                                                                    foreach (FileInfo file in di.GetFiles())
                                                                    {
                                                                        file.Delete();
                                                                    }
                                                                    foreach (DirectoryInfo dir in di.EnumerateDirectories())
                                                                    {
                                                                        dir.Delete(true);
                                                                    }
                                                                    objResultObject.OK = false;
                                                                    objResultObject.Message = "El ZIP no tiene el formato correcto. Verificar que contenga el archivo story.html";
                                                                    return new JsonResult() { Data = objResultObject };
                                                                }
                                                                else
                                                                {
                                                                    // ELIMINO EL ZIP
                                                                    System.IO.File.Delete(rutafinal);
                                                                    // SETEO LA URL
                                                                    objCursoModel.UrlExamen = NOMBREEXAMEN;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objResultObject.OK = false;
                                                                objResultObject.Message = "Para el examen de este curso solo se cargan archivos en formatos .zip";
                                                                return new JsonResult() { Data = objResultObject };
                                                            }
                                                            
                                                        }
                                                    }
                                                }

                                                //Subir archivos nuevos materiales
                                                if (Archivo != null)
                                                {
                                                    foreach (HttpPostedFileBase objArchivo in Archivo)
                                                    {
                                                        if (objArchivo != null)
                                                        {
                                                            string fileNombre = objArchivo.FileName;
                                                            string rutafinal = Path.Combine(folderMateriales, fileNombre);
                                                            objArchivo.SaveAs(rutafinal);
                                                        }
                                                    }
                                                }

                                                // SUBIR CERTIFICADO
                                                if (Certificado != null)
                                                {
                                                    string nombreArchivo = Certificado.FileName;
                                                    if (Path.GetExtension(nombreArchivo) == Constants.Certificado.TipoFormato.POWER_POINT)
                                                    {
                                                        string rutaBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderPlantillaCertificado);
                                                        string rutaFinal = Path.Combine(rutaBase, nombreArchivo);
                                                        if (System.IO.File.Exists(rutaFinal))
                                                            System.IO.File.Delete(rutaFinal);
                                                        else
                                                        {
                                                            List<string> strFiles = Directory.GetFiles(rutaBase).ToList();
                                                            if (strFiles.Count > 0)
                                                            {
                                                                foreach (string item in strFiles)
                                                                {
                                                                    System.IO.File.Delete(item);
                                                                }
                                                            }
                                                        }

                                                        string NuevoNombreCertificado = nombreArchivo.Replace(Path.GetFileNameWithoutExtension(nombreArchivo), Path.GetFileNameWithoutExtension(nombreArchivo) + " " + DateTimeHelper.PeruDateTime.ToString("dd/MM/yyyHH:mm").Replace("/", "").Replace(":", "").Replace("AM", "").Replace("PM", ""));
                                                        string RutaNueva = Path.Combine(rutaBase, NuevoNombreCertificado);
                                                        Certificado.SaveAs(RutaNueva);
                                                        objCursoModel.Certificado = NuevoNombreCertificado;
                                                    }
                                                    else
                                                    {
                                                        objResultObject.OK = false;
                                                        objResultObject.Message = "El archivo no tiene el formato adecuado. Verificar que el archivo se pptx.";
                                                        return new JsonResult() { Data = objResultObject };
                                                    }
                                                }

                                                //Imagenes de los cursos
                                                string folderImageCurso = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", FOLDER_CURSOS);
                                                System.IO.Directory.CreateDirectory(folderImageCurso);

                                                //Guarda curso
                                                if (ImagenUploadCurso != null)
                                                {
                                                    string rutaImageCurso = Path.Combine(folderImageCurso, ImagenUploadCurso.FileName);
                                                    ImagenUploadCurso.SaveAs(rutaImageCurso);
                                                    objCursoModel.ImagenCurso = ImagenUploadCurso.FileName;
                                                }

                                                objCursoModel.EstadoCurso = Constants.Cursos.Estado.ACTIVO;

                                                Curso objCurso = objCursoModel.ToCurso();
                                                int IdCurso = objCursoBC.GuardarCurso(objCurso);

                                                if (IdContenido != null)
                                                {
                                                    for (int i = 0; i < IdContenido.Length; i++)
                                                    {
                                                        ContenidoCurso objContenidoCurso = new ContenidoCurso();
                                                        objContenidoCurso.IdContenidoCurso = IdContenido[i];
                                                        if (IdContenido[i] == 0)
                                                        {
                                                            objContenidoCurso.Archivo = (TipoContenido[i] == Constants.Cursos.ContenidoCurso.SCORM) ? NOMBRESCORM : Contenido[i].FileName;
                                                        }
                                                        objContenidoCurso.Nombre = NombreContenido[i];
                                                        objContenidoCurso.IdCurso = IdCurso;
                                                        objContenidoCurso.TipoArchivo = TipoContenido[i];

                                                        objCursoBC.GuardarContenidoCurso(objContenidoCurso);
                                                    }
                                                }

                                                int indexLink = 0;

                                                if (IdMaterial != null)
                                                {
                                                    for (int i = 0; i < IdMaterial.Length; i++)
                                                    {
                                                        Material objMaterial = new Material
                                                        {
                                                            IdMaterial = IdMaterial[i],
                                                            Archivo = Archivo[i] != null ? Archivo[i].FileName : (TipoMaterial[i] == Constants.Cursos.Material.TipoMaterial.LINK ? linkCurso[indexLink] : null),
                                                            Nombre = NombreMaterial[i],
                                                            Obligatorio = Obligatorio == null ? false : (Obligatorio.Contains(i) ? true : false),
                                                            IdCurso = IdCurso,
                                                            TipoMaterial = TipoMaterial[i]
                                                        };

                                                        if (TipoMaterial[i] == Constants.Cursos.Material.TipoMaterial.LINK)
                                                            indexLink++;
                                                        int IdMaterialGuardado = objMaterial.IdMaterial;
                                                        objCursoBC.GuardarMaterial(objMaterial);

                                                        CursoSlot objCursoSlot = new MapaBC().ObtenerCursoSlotAsignadoPorIdCurso(objCurso.IdCurso);
                                                        if (objCursoSlot != null)
                                                        {
                                                            List<Inscripcion> lstInscripcion = objCursoSlot.Inscripcion.ToList();
                                                            foreach (Inscripcion objInscripcion in lstInscripcion)
                                                            {
                                                                MaterialInscripcion objMaterialInscripcion = null;
                                                                if (IdMaterialGuardado != 0)
                                                                {
                                                                    objMaterialInscripcion = objInscripcion.MaterialInscripcion.FirstOrDefault(mi => mi.IdMaterial == IdMaterialGuardado);
                                                                }
                                                                else
                                                                {
                                                                    objMaterialInscripcion = new MaterialInscripcion();
                                                                    objMaterialInscripcion.Archivo = objMaterial.Archivo;
                                                                }

                                                                objMaterialInscripcion.IdMaterial = objMaterial.IdMaterial;
                                                                objMaterialInscripcion.IdInscripcion = objInscripcion.IdInscripcion;
                                                                objMaterialInscripcion.Visto = false;
                                                                objMaterialInscripcion.IdCurso = IdCurso;
                                                                objMaterialInscripcion.Nombre = objMaterial.Nombre;
                                                                objMaterialInscripcion.TipoMaterial = objMaterial.TipoMaterial;
                                                                objMaterialInscripcion.Obligatorio = objMaterial.Obligatorio;
                                                                new CursoBC().IngresarMaterialInscripcion(objMaterialInscripcion);
                                                            }
                                                        }
                                                    }
                                                }

                                                objCursoBC.AsignarGruposACurso(objCurso.IdCurso, IdGrupo);
                                                objResultObject.OK = true;
                                                objResultObject.Message = "El curso ha sido guardado correctamente.";
                                            }
                                            else
                                            {
                                                objResultObject.OK = false;
                                                objResultObject.Message = "El código del curso ya existe. Por favor, ingresa un código distinto.";
                                            }
                                        }
                                        else
                                        {
                                            objResultObject.OK = false;
                                            objResultObject.Message = "Los nombres de material son obligatorios.";
                                        }
                                    }
                                    else
                                    {
                                        objResultObject.OK = false;
                                        objResultObject.Message = "Los nombres de los contenidos son obligatorios.";
                                    }
                                }
                                else
                                {
                                    objResultObject.OK = false;
                                    objResultObject.Message = "El nombre del archivo del material debe tener un máximo de 100 caracteres.";
                                }
                            }
                            else
                            {
                                objResultObject.OK = false;
                                objResultObject.Message = "El nombre del archivo del contenido debe tener un máximo de 100 caracteres.";
                            }

                        }
                        else
                        {
                            objResultObject.OK = false;
                            objResultObject.Message = "Solo se puede subir un archivo SCORM por curso.";
                        }
                    }
                    else
                    {
                        objResultObject.OK = false;
                        objResultObject.Message = "Por favor, cargar un examen para este curso.";
                    }
                }
                else
                {
                    objResultObject.OK = false;
                    objResultObject.Message = "Por favor, cargar una plantilla de certificado para este curso.";
                }

                
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un problema al crear un curso.";
                objResultObject.exMessage = ex.Message;
                objResultObject.exStackTrace = ex.StackTrace;
            }
            return new JsonResult() { Data = objResultObject };
        }

        public ActionResult EliminarCurso(int IdCurso)
        {
            ResultObject objResultObject = new ResultObject();
            CursoBC objCursoBC = new CursoBC();
            try
            {
                Curso objCurso = objCursoBC.ObtenerCurso(IdCurso);
                string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cursos", objCurso.Codigo);
                if (Directory.Exists(ruta))
                    Directory.Delete(ruta, true);
                if (objCurso.Imagen != null)
                {
                    string rutaImagenCurso = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", FOLDER_CURSOS, objCurso.Imagen);
                    if (System.IO.File.Exists(rutaImagenCurso))
                        System.IO.File.Delete(rutaImagenCurso);
                }

                objCursoBC.EliminarCurso(objCurso.IdCurso);
                objResultObject.OK = true;
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult SubirCertificado(HttpPostedFileBase FileCertificado)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                CursoBC objCursoBC = new CursoBC();
                if (FileCertificado != null)
                {
                    string nombreArchivo = FileCertificado.FileName;
                    if (Path.GetExtension(nombreArchivo) == Constants.Certificado.TipoFormato.POWER_POINT)
                    {
                        string rutaBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS);
                        string rutaFinal = Path.Combine(rutaBase, nombreArchivo);
                        if (System.IO.File.Exists(rutaFinal))
                            System.IO.File.Delete(rutaFinal);
                        else
                        {
                            List<string> strFiles = Directory.GetFiles(rutaBase).ToList();
                            if (strFiles.Count > 0)
                            {
                                foreach (string item in strFiles)
                                {
                                    System.IO.File.Delete(item);
                                }
                            }
                        }

                        Configuracion objConfiguracion = new Configuracion
                        {
                            Nombre = Constants.Configuracion.FORMATO_CERTIFICADO,
                            Valor = nombreArchivo
                        };

                        FileCertificado.SaveAs(rutaFinal);

                        objCursoBC.EliminarConfiguracionPorCodigo(Constants.Configuracion.FORMATO_CERTIFICADO);
                        objCursoBC.GuardarConfiguracion(objConfiguracion);

                        objResultObject.OK = true;
                    }
                    else
                    {
                        objResultObject.OK = false;
                        objResultObject.Message = "El formato del certificado es incorrecto";
                    }
                }
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult ArchivarCurso(int IdCurso)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                Curso objCurso = new CursoBC().ObtenerCurso(IdCurso);
                objCurso.Estado = Constants.Cursos.Estado.ARCHIVADO;
                new CursoBC().GuardarCurso(objCurso);
                objResultObject.OK = true;
            }
            catch (Exception ex)
            {

                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al modificar el estado del curso";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult ReactivarCurso(int IdCurso)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                Curso objCurso = new CursoBC().ObtenerCurso(IdCurso);
                objCurso.Estado = Constants.Cursos.Estado.ACTIVO;
                new CursoBC().GuardarCurso(objCurso);
                objResultObject.OK = true;
            }
            catch (Exception ex)
            {

                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al modificar el estado del curso";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }

        [HttpPost]
        public ActionResult GetProgress()
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                objResultObject.Data = new CursoBC().GetProgress();
                objResultObject.OK = true;
            }
            catch (Exception ex)
            {
                objResultObject.OK = false;
                objResultObject.Message = "Ocurrió un error al modificar el estado del curso";
                objResultObject.exMessage = ex.Message;
            }
            return new JsonResult() { Data = objResultObject };
        }
    }
}
