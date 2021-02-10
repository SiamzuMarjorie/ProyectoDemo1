using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class CursoSlotModel
    {
        public int IdCursoSlot { get; set; }
        public int? IdCurso { get; set; }
        public int IdSlot { get; set; }
        public string FechaHoraInicio { get { return FechaInicio + " " + HoraInicio; } }
        public string FechaHoraFin { get { return FechaFin + " " + HoraFin; } }
        public bool? EmiteCertificado { get; set; }
        public int? Intentos { get; set; }
        public bool? Visible { get; set; }
        public string Estado { get; set; }
        public string CodigoCurso { get; set; }
        public string TituloCurso { get; set; }
        public int? HorasCurso { get; set; }
        public int? PuntajeCurso { get; set; }
        public string UrlCurso { get; set; }
        public string SlotCurso { get; set; }
        public List<CursoModel> lstCursoModel { get; set; }
        public string Descripcion { get; set; }
        public string ImagenCurso { get; set; }
        public string UltimaPublicacion { get; set; }
        public bool Empezo { get; set; }
        public string FechaHoraRegistro { get; set; }
        public string FechaHoraUltimaActualizacion { get; set; }
        public int Orden { get; set; }
        public int? IdCursoSlotDependiente { get; set; }
        public string FechaInicio { get; set; }
        public string HoraInicio { get; set; }
        public string FechaFin { get; set; }
        public string HoraFin { get; set; }
        public int Ano { get; set; }
        public string NombreSlot { get; set; }

        public static CursoSlotModel FromCursoSlot(CursoSlot objCursoSlot)
        {
            try
            {
                CursoSlotModel objCursoSlotModel = new CursoSlotModel();
                objCursoSlotModel.IdCursoSlot = objCursoSlot.IdCursoSlot;
                objCursoSlotModel.IdCurso = objCursoSlot.IdCurso;
                objCursoSlotModel.IdSlot = objCursoSlot.IdSlot;

                if (objCursoSlot.FechaHoraInicio != null)
                {
                    objCursoSlotModel.FechaInicio = objCursoSlot.FechaHoraInicio.Value.ToString("dd/MM/yyyy");
                    objCursoSlotModel.HoraInicio = objCursoSlot.FechaHoraInicio.Value.ToString("HH:mm");
                }
                if (objCursoSlot.FechaHoraFin != null)
                {
                    objCursoSlotModel.FechaFin = objCursoSlot.FechaHoraFin.Value.ToString("dd/MM/yyyy");
                    objCursoSlotModel.HoraFin = objCursoSlot.FechaHoraFin.Value.ToString("HH:mm");
                }
                objCursoSlotModel.Intentos = objCursoSlot.Intentos;
                objCursoSlotModel.Ano = objCursoSlot.Ano;
                objCursoSlotModel.Visible = objCursoSlot.Visible;
                objCursoSlotModel.Estado = objCursoSlot.Estado;
                objCursoSlotModel.CodigoCurso = objCursoSlot.Codigo;
                objCursoSlotModel.TituloCurso = objCursoSlot.Titulo;
                objCursoSlotModel.HorasCurso = objCursoSlot.Horas;
                objCursoSlotModel.PuntajeCurso = objCursoSlot.Puntos;
                objCursoSlotModel.SlotCurso = objCursoSlot.Slot.Nombre;
                objCursoSlotModel.Descripcion = objCursoSlot.Descripcion;
                if (objCursoSlot.Curso.Imagen != null)
                    objCursoSlotModel.ImagenCurso = objCursoSlot.Curso.Imagen;
                objCursoSlotModel.TituloCurso = objCursoSlot.Titulo;
                objCursoSlotModel.NombreSlot = objCursoSlot.NombreSlot;

                objCursoSlotModel.Empezo = objCursoSlot.FechaHoraInicio != null && objCursoSlot.FechaHoraInicio <= DateTimeHelper.PeruDateTime;
                objCursoSlotModel.EmiteCertificado = objCursoSlot.Curso.EmiteCertificado;

                //if (objCursoSlotModel.FechaHoraInicio != null)
                //    objCursoSlotModel.UltimaPublicacion = DateTime.ParseExact(objCursoSlotModel.FechaHoraInicio, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("dd/MM");

                return objCursoSlotModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ToCursoSlot()
        {
            try
            {
                CursoSlot objCursoSlot = new CursoSlot();
                objCursoSlot.IdCursoSlot = IdCursoSlot;
                objCursoSlot.IdCurso = IdCurso;
                objCursoSlot.IdSlot = IdSlot;
                objCursoSlot.NombreSlot = NombreSlot;
                objCursoSlot.FechaHoraInicio = DateTime.ParseExact(FechaHoraInicio, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                objCursoSlot.FechaHoraFin = DateTime.ParseExact(FechaHoraFin, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                objCursoSlot.FechaHoraRegistro = DateTime.ParseExact(FechaHoraRegistro, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                objCursoSlot.FechaHoraUltimaActualizacion = DateTime.ParseExact(FechaHoraUltimaActualizacion, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                objCursoSlot.Orden = Orden;
                objCursoSlot.Ano = Ano;
                objCursoSlot.IdCursoSlotDependiente = IdCursoSlotDependiente;
                objCursoSlot.Intentos = Intentos;
                objCursoSlot.Visible = Visible;
                objCursoSlot.Estado = Estado;
                objCursoSlot.Titulo = TituloCurso;
                objCursoSlot.Horas = HorasCurso;
                objCursoSlot.Puntos = PuntajeCurso;
                objCursoSlot.Codigo = CodigoCurso;
                objCursoSlot.Descripcion = Descripcion;
                return objCursoSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
