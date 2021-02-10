using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class PreguntaDA
    {
        public IQueryable<Pregunta> ListarPregunta()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Pregunta;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Pregunta ObtenerPregunta(int IdPregunta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Pregunta objPreguntaBD = objModel.Pregunta.SingleOrDefault(x => x.IdPregunta == IdPregunta);
                return objPreguntaBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarPregunta(Pregunta objPregunta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.Pregunta.Add(objPregunta);
                objModel.SaveChanges();
                return objPregunta.IdPregunta;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarPregunta(Pregunta objPregunta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Pregunta objPreguntaBD = objModel.Pregunta.SingleOrDefault(x => x.IdPregunta == objPregunta.IdPregunta);
                objPreguntaBD.Contenido = objPregunta.Contenido;
                objPreguntaBD.IdCursoSlot = objPregunta.IdCursoSlot;
                objPreguntaBD.IdPregunta = objPregunta.IdPregunta;
                objPreguntaBD.Respuesta = objPregunta.Respuesta;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarPregunta(int IdPregunta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Pregunta objPreguntaBD = objModel.Pregunta.SingleOrDefault(x => x.IdPregunta == IdPregunta);
                objModel.Pregunta.Remove(objPreguntaBD);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GuardarPregunta(string Pregunta, int IdCursoSlot)
        {
            try
            {
                Pregunta = Pregunta.Trim();

                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Pregunta objPregunta = objModel.Pregunta.FirstOrDefault(x => x.IdCursoSlot == IdCursoSlot && x.Contenido.ToLower() == Pregunta.ToLower());
                if (objPregunta == null)
                {
                    objPregunta = new Pregunta();
                    objPregunta.IdCursoSlot = IdCursoSlot;
                    objPregunta.Contenido = Pregunta;
                    objModel.Pregunta.Add(objPregunta);
                    objModel.SaveChanges();
                }

                return objPregunta.IdPregunta;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public IQueryable<Pregunta> ListarPreguntasPorCursoSlot(int IdCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Pregunta.Where(p => p.IdCursoSlot == IdCursoSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
