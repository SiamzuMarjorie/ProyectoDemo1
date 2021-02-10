using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class SlotDA
    {
        public IQueryable<Slot> ListarSlot()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Slot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Slot ObtenerSlot(int IdSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Slot.SingleOrDefault(x => x.IdSlot == IdSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarSlot(Slot objSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.Slot.Add(objSlot);
                objModel.SaveChanges();
                return objSlot.IdSlot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarSlot(Slot objSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Slot objSlotBD = objModel.Slot.SingleOrDefault(x => x.IdSlot == objSlot.IdSlot);
                objSlotBD.IdSlot = objSlot.IdSlot;
                objSlotBD.Codigo = objSlot.Codigo;
                objSlotBD.Nombre = objSlot.Nombre;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GuardarSlot(Slot objSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                if(objSlot.IdSlot == 0)
                {
                    objModel.Slot.Add(objSlot);
                }
                else
                {
                    Slot objSlotBD = objModel.Slot.FirstOrDefault(s => s.IdSlot == objSlot.IdSlot);
                    objSlotBD.NombreSlot = objSlot.NombreSlot;
                    objSlotBD.CursosDependientes = objSlot.CursosDependientes;

                    foreach (CursoSlot objCursoSlot in objSlotBD.CursoSlot.Where(cs => cs.Estado != Constants.Cursos.Estado.HISTORIAL))
                    {
                        objCursoSlot.NombreSlot = objSlot.NombreSlot;
                    }
                }

                objModel.SaveChanges();
                return objSlot.IdSlot;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        
        public void EliminarSlot(int IdSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Slot objSlotBD = objModel.Slot.SingleOrDefault(x => x.IdSlot == IdSlot);
                objModel.Slot.Remove(objSlotBD);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
