using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemBox.Presentation;

using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Batch
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //IQueryable<Inscripcion> lstInscripcion = new MapaBC().ListarInscripcionAprobadoConNotaSinTerminarSinCertificado();

                //int count = 1;
                //foreach (Inscripcion objInscripcion in lstInscripcion)
                //{
                //    Trabajador objTrabajador = objInscripcion.Trabajador;
                //    CursoSlot objCursoSlot = objInscripcion.CursoSlot;

                //    Console.WriteLine(count + ": " + String.Join(" ", objTrabajador.Nombres, objTrabajador.PrimerApellido, objTrabajador.SegundoApellido) + " - " + objCursoSlot.Titulo);

                //    Configuracion objConfiguracion = new MapaBC().ObtenerConfiguracionPorNombre(Constants.Configuracion.FORMATO_CERTIFICADO);
                //    double? PuntajeMaximo = objCursoSlot.Inscripcion.FirstOrDefault(i => i.IdTrabajador == objInscripcion.IdTrabajador).Intento.Max(i => i.Puntaje);
                //    double? NuevoPuntaje = (objCursoSlot.Puntos * objInscripcion.Nota) / objInscripcion.NotaMaxima;

                //    String Certificado = null;

                //    if (objCursoSlot.EmiteCertificado == true)
                //    {
                //        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

                //        var powerpoint = PresentationDocument.Load(@"C:\Production\Cursos Virtuales\Cursos Virtuales Admin\Cursos\Plantilla de Certificado.pptx");

                //        var slide = powerpoint.Slides[0];

                //        slide.TextContent.Replace("[NOMBRE_COLABORADOR]", objTrabajador.Nombres + " " + objTrabajador.PrimerApellido + " " + objTrabajador.SegundoApellido);
                //        slide.TextContent.Replace("[NOMBRE_CURSO]", objCursoSlot.Curso.Titulo);
                //        slide.TextContent.Replace("[FECHA]", DateTimeHelper.PeruDateTime.ToString("dd/MM/yyyy"));
                //        slide.TextContent.Replace("[HORAS]", objCursoSlot.Curso.Horas != null ? objCursoSlot.Curso.Horas.Value.ToString() : String.Empty);
                //        slide.TextContent.Replace("[NOTA]", objInscripcion.Nota.ToString());
                //        slide.TextContent.Replace("[PUNTAJE]", NuevoPuntaje != null ? NuevoPuntaje.Value.ToString() : String.Empty);

                //        Certificado = "Certificado " + objCursoSlot.Titulo + " - " + String.Join(" ", objTrabajador.Nombres, objTrabajador.PrimerApellido, objTrabajador.SegundoApellido) + " " + DateTimeHelper.PeruDateTime.ToString("dd-MM-yyyy") + ".pdf";
                //        string pathPDF = @"C:\Production\Cursos Virtuales\Cursos Virtuales Admin\Cursos\" + objCursoSlot.Codigo + @"\Certificados\" + Certificado;

                //        powerpoint.Save(pathPDF);
                //    }

                //    new CursoBC().RegistrarResultadoCurso(objCursoSlot.Codigo, objInscripcion.Nota.Value, objInscripcion.NotaMaxima.Value, true, objInscripcion.IdTrabajador, Certificado);

                //    count++;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.ReadLine();
        }
    }
}
