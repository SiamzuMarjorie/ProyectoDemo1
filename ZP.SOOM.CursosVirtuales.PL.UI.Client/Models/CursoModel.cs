using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Models
{
    public class CursoModel
    {
        public int? IdCurso { get; set; }
        public int IdCursoSlot { get; set; }
        public string Titulo { get; set; }
        public int? Horas { get; set; }
        public string UrlExamen { get; set; }
        public int? Puntos { get; set; }
        public string Codigo { get; set; }
        public string UltimaPublicacion { get; set; }
        public string Estado { get; set; }
        public string Slot { get; set; }
        public string Descripcion { get; set; }
        public string FechaFin { get; set; }
        public string FechaInicio { get; set; }
        public string ImagenCurso { get; set; }
        public int? PuntajeMaximo { get; set; }
        public double? Puntaje { get; set; }
        public int Nota { get; set; }
        public int CantidadIntentos { get; set; }
        public int? IntentosPermitidos { get; set; }
        public List<MaterialModel> Materiales { get; set; }
        public bool? Aprobado { get; set; }
        public bool Terminado { get; set; }
        public int? IdInscripcion { get; set; }
        public int? CodigoSlot { get; set; }
        public bool? EmiteCertificado { get; set; }
        public bool TieneExamen { get; set; }
        public string EstadoCurso { get; set; }
        public string UrlContenidoCurso { get; set; }
        public int CantidadContenidoCurso { get; set; }
        public string Certificado { get; set; }
        public string TipoContenidoCurso { get; set; }

        public const String PLANTILLA_CERTIFICADO = "~/Cursos/{CODIGO_CURSO}/PlantillaCertificado/{ARCHIVO}";
        public const String URL_CERTIFICADO = "~/Cursos/{CODIGO_CURSO}/Certificados/{ARCHIVO}";
        public const String NOMBRE_ARCHIVO = "Certificado {TITULO} - {PERSONA} {FECHA}.pdf";
        public const String CODIGO_CURSO = "{CODIGO_CURSO}";
        public const String ARCHIVO = "{ARCHIVO}";
        public const String CURSO = "{TITULO}";
        public const String PERSONA = "{PERSONA}";
        public const String FECHA = "{FECHA}";

        public static CursoModel FromCurso(Curso objCurso)
        {
            try
            {
                CursoModel objCursoModel = new CursoModel();
                objCursoModel.Codigo = objCurso.Codigo;
                objCursoModel.Horas = objCurso.Horas;
                objCursoModel.IdCurso = objCurso.IdCurso;
                objCursoModel.Puntos = objCurso.Puntos;
                objCursoModel.Titulo = objCurso.Titulo;
                objCursoModel.TieneExamen = objCurso.TieneExamen;
                objCursoModel.UrlExamen = objCurso.UrlExamen;
                objCursoModel.Materiales = new List<MaterialModel>();

                foreach (Material objMaterial in objCurso.Material)
                    objCursoModel.Materiales.Add(MaterialModel.FromMaterial(objMaterial));

                CursoSlot objCursoSlotActivo = objCurso.CursoSlot.FirstOrDefault(cs => cs.Estado != Constants.Cursos.Estado.HISTORIAL);

                if (objCursoSlotActivo != null)
                {
                    if (objCursoSlotActivo.FechaHoraInicio != null)
                        objCursoModel.UltimaPublicacion = objCursoSlotActivo.FechaHoraInicio.Value.ToString("dd/MM");

                    objCursoModel.Estado = objCursoSlotActivo.Estado;
                    objCursoModel.Slot = objCursoSlotActivo.Slot.Nombre;
                }
                else
                    objCursoModel.Estado = Constants.Cursos.Estado.INACTIVO;

                return objCursoModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static CursoModel FromCursoSlot(CursoSlot objCursoSlot)
        {
            try
            {
                CursoModel objCursoModel = new CursoModel();

                objCursoModel.IdCurso = objCursoSlot.IdCurso;
                objCursoModel.IdCursoSlot = objCursoSlot.IdCursoSlot;
                objCursoModel.Slot = objCursoSlot.Slot.Nombre;
                objCursoModel.CodigoSlot = objCursoSlot.Slot.Codigo;
                objCursoModel.Titulo = objCursoSlot.Titulo;
                objCursoModel.Descripcion = objCursoSlot.Descripcion;

                objCursoModel.Materiales = new List<MaterialModel>();
                if (objCursoSlot.Curso != null)
                {
                    foreach (Material objMaterial in objCursoSlot.Curso.Material)
                        objCursoModel.Materiales.Add(MaterialModel.FromMaterial(objMaterial));
                }
                objCursoModel.FechaInicio = objCursoSlot.FechaHoraInicio.Value.ToString("dd/MM/yyyy HH:mm");
                objCursoModel.FechaFin = objCursoSlot.FechaHoraFin.Value.ToString("dd/MM/yyyy HH:mm");
                objCursoModel.Estado = objCursoSlot.Estado;
                objCursoModel.ImagenCurso = objCursoSlot.Curso.Imagen;
                objCursoModel.PuntajeMaximo = objCursoSlot.Puntos;
                objCursoModel.Codigo = objCursoSlot.Codigo;
                objCursoModel.UrlExamen = objCursoSlot.Curso.UrlExamen;
                objCursoModel.IntentosPermitidos = objCursoSlot.Intentos;
                objCursoModel.EmiteCertificado = objCursoSlot.Curso.EmiteCertificado == null || objCursoSlot.Curso.EmiteCertificado == false ? false : true;

                return objCursoModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Curso ToCurso()
        {
            try
            {
                Curso objCurso = new Curso();
                objCurso.Codigo = Codigo;
                objCurso.Horas = Horas;
                objCurso.IdCurso = IdCurso ?? 0;
                objCurso.Puntos = Puntos;
                objCurso.Titulo = Titulo;
                objCurso.UrlExamen = UrlExamen;

                return objCurso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}