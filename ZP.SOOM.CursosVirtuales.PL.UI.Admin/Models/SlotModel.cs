using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class SlotModel
    {
        public int IdSlot { get; set; }
        public string Nombre { get; set; }
        public string NombreSlot { get; set; }
        public int? Codigo { get; set; }
        public string NombreCurso { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; }
        public int IdCursoSlot { get; set; }
        public int? IdCurso { get; set; }
        public int CantidadCursos { get; set; }
        public List<CursoSlotModel> CursoSlot { get; set; }

        public static SlotModel FromSlot(Slot objSlot)
        {
            try
            {
                SlotModel objSlotModel = new SlotModel();
                objSlotModel.IdSlot = objSlot.IdSlot;
                objSlotModel.Nombre = objSlot.Nombre;
                objSlotModel.NombreSlot = objSlot.NombreSlot;
                objSlotModel.Codigo = objSlot.Codigo;
                objSlotModel.CursoSlot = new List<CursoSlotModel>();
                foreach (CursoSlot objCursoSlot in objSlot.CursoSlot.Where(cs => cs.Curso != null))
                {
                    objSlotModel.CursoSlot.Add(CursoSlotModel.FromCursoSlot(objCursoSlot));
                }


                CursoSlot objCursoSlotActivo = objSlot.CursoSlot.FirstOrDefault(c => c.Estado == Constants.Cursos.Estado.ACTIVO && c.FechaHoraFin >= DateTimeHelper.PeruDateTime);
                if (objCursoSlotActivo != null)
                {
                    objSlotModel.NombreCurso = objCursoSlotActivo.Titulo;
                    objSlotModel.FechaInicio = objCursoSlotActivo.FechaHoraInicio;
                    objSlotModel.FechaFin = objCursoSlotActivo.FechaHoraFin;
                    objSlotModel.Estado = objCursoSlotActivo.Estado;
                    objSlotModel.IdCursoSlot = objCursoSlotActivo.IdCursoSlot;
                    objSlotModel.IdCurso = objCursoSlotActivo.IdCurso;
                    objSlotModel.CantidadCursos = objSlot.CursoSlot.Where(e => e.Estado != Constants.Cursos.Estado.HISTORIAL && e.Curso != null).Count();

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

        public Slot ToSlot(SlotModel objSlotModel)
        {
            try
            {
                Slot objSlot = new Slot();
                objSlot.IdSlot = objSlotModel.IdSlot;
                objSlot.NombreSlot = objSlotModel.NombreSlot;

                return objSlot;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}