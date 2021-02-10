using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
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
        public bool EmiteCertificado { get; set; }
        public bool Empezo { get; set; }
        public DateTime? FechaHoraInicio { get; set; }
        public DateTime? FechaHoraFin { get; set; }
        public string Descripcion { get; set; }
        public string ImagenCurso { get; set; }
        public List<MaterialModel> Materiales { get; set; }
        public List<ContenidoCursoModel> ContenidoCurso { get; set; }
        public List<CursoSlotModel> Lanzamientos { get; set; }
        public string EstadoCurso { get; set; }
        public bool TieneExamen { get; set; }
        public string Certificado { get; set; }

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
                objCursoModel.UrlExamen = objCurso.UrlExamen;
                objCursoModel.Certificado = objCurso.Certificado;
                objCursoModel.Descripcion = objCurso.Descripcion;
                objCursoModel.EmiteCertificado = objCurso.EmiteCertificado;
                objCursoModel.TieneExamen = objCurso.TieneExamen;
                
                objCursoModel.ImagenCurso = objCurso.Imagen;
                objCursoModel.EstadoCurso = objCurso.Estado;
                objCursoModel.Materiales = new List<MaterialModel>();
                objCursoModel.ContenidoCurso = new List<ContenidoCursoModel>();

                foreach (Material objMaterial in objCurso.Material)
                    objCursoModel.Materiales.Add(MaterialModel.FromMaterial(objMaterial));

                foreach (ContenidoCurso objContenidoCurso in objCurso.ContenidoCurso)
                    objCursoModel.ContenidoCurso.Add(ContenidoCursoModel.FromContenidoCurso(objContenidoCurso));

                CursoSlot objCursoSlotActivo = objCurso.CursoSlot.OrderByDescending(cs=>cs.IdCursoSlot).FirstOrDefault(cs => cs.Estado != Constants.Cursos.Estado.HISTORIAL);
                if (objCursoSlotActivo != null)
                {
                    objCursoModel.FechaHoraInicio = objCursoSlotActivo.FechaHoraInicio;
                    objCursoModel.FechaHoraFin = objCursoSlotActivo.FechaHoraFin;
                    objCursoModel.Empezo = objCursoSlotActivo.FechaHoraInicio != null && objCursoSlotActivo.FechaHoraInicio <= DateTimeHelper.PeruDateTime;
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
                objCursoModel.Slot = objCursoSlot.Slot.Nombre;
                objCursoModel.Titulo = objCursoSlot.Titulo;
                objCursoModel.FechaHoraInicio = objCursoSlot.FechaHoraInicio;
                objCursoModel.FechaHoraFin = objCursoSlot.FechaHoraFin;
                objCursoModel.IdCursoSlot = objCursoSlot.IdCursoSlot;

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
                objCurso.IdCurso = (int)IdCurso;
                objCurso.Puntos = Puntos;
                objCurso.Titulo = Titulo;
                objCurso.UrlExamen = UrlExamen;
                objCurso.Descripcion = Descripcion;
                objCurso.Imagen = ImagenCurso;
                objCurso.Estado = EstadoCurso;
                objCurso.EmiteCertificado = EmiteCertificado;
                objCurso.TieneExamen = TieneExamen;
                objCurso.Certificado = Certificado;

                return objCurso;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}