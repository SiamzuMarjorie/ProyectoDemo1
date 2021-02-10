using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Models
{
    public class SlotModel
    {
        public int IdSlot { get; set; }
        public string Nombre { get; set; }
        public int? Codigo { get; set; }
        public string NombreCurso { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; }
        public int IdCursoSlot { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? W { get; set; }
        public string ColorText { get; set; }
        public string ColorBackground { get; set; }
        public string NombreSlot { get; set; }
        public string EstadoSlot { get; set; }

        public static SlotModel FromSlot(Slot objSlot)
        {
            try
            {
                SlotModel objSlotModel = new SlotModel();
                objSlotModel.IdSlot = objSlot.IdSlot;
                objSlotModel.Nombre = objSlot.Nombre;
                objSlotModel.Codigo = objSlot.Codigo;
                objSlotModel.X = objSlot.Izquierda;
                objSlotModel.Y = objSlot.Alto;
                objSlotModel.W = objSlot.Ancho;
                objSlotModel.ColorText = objSlot.ColorText;
                objSlotModel.ColorBackground = objSlot.ColorBackground;
                objSlotModel.NombreSlot = objSlot.NombreSlot;

                CursoSlot objCursoSlotActivo = objSlot.CursoSlot.FirstOrDefault(c => c.Estado == Constants.Cursos.Estado.ACTIVO && c.IdSlot == objSlot.IdSlot && c.Curso != null);
                if (objCursoSlotActivo != null)
                {
                    objSlotModel.NombreCurso = objCursoSlotActivo.Curso.Titulo;
                    objSlotModel.FechaInicio = objCursoSlotActivo.FechaHoraFin;
                    objSlotModel.FechaFin = objCursoSlotActivo.FechaHoraFin;
                    objSlotModel.Estado = objCursoSlotActivo.Estado;
                    objSlotModel.IdCursoSlot = objCursoSlotActivo.IdCursoSlot;
                }
                else
                {
                    objSlotModel.Estado = Constants.Cursos.Estado.INACTIVO;
                }

                return objSlotModel;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}