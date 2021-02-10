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
    public class CertificadosController : Controller
    {
        //
        // GET: /Certificados/
        public const String FOLDER_CURSOS = "Cursos";

        public const String FOLDER_CERTIFICADO = "Certificados";

        public ActionResult Index()
        {
            List<int> lstAno = new CursoBC().ListarAnos();
            ViewBag.ListarAno = lstAno;
            return View();

        }


        public ActionResult ListaCampos(string NombreCampo, int? Ano)
        {
            try
            {
                UsuarioBC objUsuario = new UsuarioBC();
                MapaBC objMapa = new MapaBC();
                List<string> lstCampos = new List<string>();

                if (NombreCampo == Constants.ReporteGeneral.Campos.COMPANIA)
                {
                    lstCampos = objUsuario.ListarCompanias();
                }
                else if (NombreCampo == Constants.ReporteGeneral.Campos.SEDE)
                {
                    lstCampos = objUsuario.ListarSede();
                }
                else if (NombreCampo == Constants.ReporteGeneral.Campos.GERENCIA)
                {
                    lstCampos = objUsuario.ListarGerencia();
                }
                else if (NombreCampo == Constants.ReporteGeneral.Campos.CATEGORIA)
                {
                    lstCampos = objUsuario.ListarGrupoOcupacionales();
                }
                else if (NombreCampo == Constants.ReporteGeneral.Campos.CURSOSLOT)
                {
                    List<SlotModel> lstSlotModel = new List<SlotModel>();

                    var lstCursoSlot = objMapa.ListarCursoSlot();
                    List<string> lstNombreSlot = lstCursoSlot.Select(cs => cs.NombreSlot).Distinct().ToList();
                    //List<CursoSlot> lstCursoSlotAno = objMapa.ListarCursoSlotPorAno(Ano).ToList();
                    //List<string> lstNombreSlot = lstCursoSlotAno.Select(cs => cs.NombreSlot).Distinct().ToList();
                    foreach (string NombreSlot in lstNombreSlot)
                    {
                        SlotModel objSlotModel = new SlotModel();
                        objSlotModel.NombreSlot = NombreSlot;
                        List<CursoSlot> lstCursosDelSlot = lstCursoSlot.Where(cs => cs.NombreSlot == NombreSlot).ToList();
                        objSlotModel.CursoSlot = new List<CursoSlotModel>();
                        foreach (CursoSlot objCursoSlot in lstCursosDelSlot)
                        {
                            objSlotModel.CursoSlot.Add(CursoSlotModel.FromCursoSlot(objCursoSlot));
                        }

                        lstSlotModel.Add(objSlotModel);
                    }
                    ViewBag.lstSlot = lstSlotModel;
                }
                else
                {
                    lstCampos = objMapa.ListarNombreGrupo();
                }

                return PartialView("_TablaReporte", lstCampos);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al cargar la lista";
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult DescargarCertiticados1(string NombreCampo, int? Ano, string[] OpcionSeleccionada,int [] IdCursoSlot)
        {
            ResultObject objResultObject = new ResultObject();
            try
            {
                CertificadosBC certificadosBC = new CertificadosBC();
                List<CertificadoEntity> listaCertificados = new List<CertificadoEntity>();
                if (OpcionSeleccionada !=null )
                {
                    //evaluar que caso es

                     listaCertificados = certificadosBC.ListarCertificadosbyNombreCampo(NombreCampo,Ano, OpcionSeleccionada);
                }
                else if(IdCursoSlot!=null){
                    //filtro con curso slot
                    listaCertificados = certificadosBC.ListarCertificadosbyCursoSlot( Ano, IdCursoSlot);
                }
                else
                {
                    listaCertificados = new List<CertificadoEntity>();
                }



                using (ZipFile zipCertificado = new ZipFile())
                {


                    foreach (var certificado in listaCertificados.Distinct())
                    {
                        string folderCertificado = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS, certificado.Codigo, FOLDER_CERTIFICADO);

                        if (!string.IsNullOrEmpty(certificado.Certificado))
                        {

                            try
                            {
                                string rutaArchivo = Path.Combine(folderCertificado, certificado.Certificado);
                                string nombreArchivo = Path.GetFileName(rutaArchivo);
                                var archivo_arregloBytes = System.IO.File.ReadAllBytes(rutaArchivo);
                                zipCertificado.AddEntry(nombreArchivo, archivo_arregloBytes);
                            }
                            catch (Exception ex)
                            {

                            }                           
                        }
                    }
                    using (MemoryStream stream = new MemoryStream())
                    {
                        string nombreZip = string.Format("Certificados {0} {1} - {2}.zip", "Certificados", Ano, Ano);
                        zipCertificado.Save(stream);
                        return File(stream.ToArray(), "application/zip", nombreZip);
                    }
                }





                //string Codigo=""; string TituloCurso = ""; string FechaInicio = ""; string FechaFin = "";
                //int IdCursoSlot1 = 0;


                
                //CursoBC objCursoBC = new CursoBC();
                //Curso objCurso = objCursoBC.ObtenerCursoPorCursoSlot(IdCursoSlot1);
                //List<string> Certificados = new List<string>();
                //if (objCurso.TieneExamen && objCurso.EmiteCertificado)
                //    Certificados = objCursoBC.ListarInscripcionPorCursoSlot(IdCursoSlot1).Select(i => i.Intento.OrderByDescending(n => n.Nota).FirstOrDefault().Certificado).ToList();
                //if (objCurso.TieneExamen == false && objCurso.EmiteCertificado)
                //    Certificados = objCursoBC.ListarInscripcionPorCursoSlot(IdCursoSlot1).Select(i => i.Certificado).ToList();
                //string folderCertificado = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FOLDER_CURSOS, Codigo, FOLDER_CERTIFICADO);
                //using (ZipFile zipCertificado = new ZipFile())
                //{
                //    foreach (string certificado in Certificados.Distinct())
                //    {
                //        if (!string.IsNullOrEmpty(certificado))
                //        {
                //            string rutaArchivo = Path.Combine(folderCertificado, certificado);
                //            string nombreArchivo = Path.GetFileName(rutaArchivo);
                //            var archivo_arregloBytes = System.IO.File.ReadAllBytes(rutaArchivo);
                //            zipCertificado.AddEntry(nombreArchivo, archivo_arregloBytes);
                //        }
                //    }
                //    using (MemoryStream stream = new MemoryStream())
                //    {
                //        string nombreZip = string.Format("Certificados {0} {1} - {2}.zip", TituloCurso, FechaInicio.Replace("/", "-"), FechaFin.Replace("/", "-"));
                //        zipCertificado.Save(stream);
                //        return File(stream.ToArray(), "application/zip", nombreZip);
                //    }
                //}

            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Ocurrió un error al descargar los certificados.";
                return View("Error");
            }




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
                if (objCurso.TieneExamen == false && objCurso.EmiteCertificado)
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