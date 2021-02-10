using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class RespuestaDA
    {
        public IQueryable<Respuesta> ListarRespuesta()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Respuesta;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Respuesta ObtenerRespuesta(int IdRespuesta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Respuesta objRespuestaBD = objModel.Respuesta.SingleOrDefault(x => x.IdRespuesta == IdRespuesta);
                return objRespuestaBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarRespuesta(Respuesta objRespuesta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.Respuesta.Add(objRespuesta);
                objModel.SaveChanges();
                return objRespuesta.IdRespuesta;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarRespuesta(Respuesta objRespuesta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Respuesta objRespuestaBD = objModel.Respuesta.SingleOrDefault(x => x.IdRespuesta == objRespuesta.IdRespuesta);
                objRespuestaBD.Contenido = objRespuesta.Contenido;
                objRespuestaBD.Correcta = objRespuesta.Correcta;
                objRespuestaBD.Intento.IdInscripcion = objRespuesta.Intento.IdInscripcion;
                objRespuestaBD.IdPregunta = objRespuesta.IdPregunta;
                objRespuestaBD.IdRespuesta = objRespuesta.IdRespuesta;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarRespuesta(int IdRespuesta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Respuesta objRespuestaBD = objModel.Respuesta.SingleOrDefault(x => x.IdRespuesta == IdRespuesta);
                objModel.Respuesta.Remove(objRespuestaBD);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GuardarRespuesta(int IdPregunta, int IdIntento, string Contenido, bool Correcta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Respuesta objRespuesta = objModel.Respuesta.FirstOrDefault(x => x.IdPregunta == IdPregunta && x.IdIntento == IdIntento);
                if (objRespuesta == null)
                {
                    objRespuesta = new Respuesta();
                    objRespuesta.IdPregunta = IdPregunta;
                    objRespuesta.IdIntento = IdIntento;
                    objModel.Respuesta.Add(objRespuesta);
                }

                objRespuesta.Contenido = Contenido;
                objRespuesta.Correcta = Correcta;
                objModel.SaveChanges();

                return objRespuesta.IdRespuesta;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public IQueryable<Respuesta> ListarRespuestasPorPregunta(int IdPregunta)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Respuesta.Where(r => r.IdPregunta == IdPregunta);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
