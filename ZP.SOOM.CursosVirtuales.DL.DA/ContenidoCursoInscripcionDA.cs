using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class ContenidoCursoInscripcionDA
    {
        public int RegistrarContenidoCursoInscripcion(int IdContenidoCurso, int IdInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                ContenidoCursoInscripcion objContenidoCursoInscripcion = objModel.ContenidoCursoInscripcion.FirstOrDefault(cci => cci.IdContenidoCurso == IdContenidoCurso && cci.IdInscripcion == IdInscripcion);
                if (objContenidoCursoInscripcion == null)
                {
                    objContenidoCursoInscripcion = new ContenidoCursoInscripcion();
                    objContenidoCursoInscripcion.IdContenidoCurso = IdContenidoCurso;
                    objContenidoCursoInscripcion.IdInscripcion = IdInscripcion;
                    objContenidoCursoInscripcion.FechaHoraRevision = DateTimeHelper.PeruDateTime;
                    objModel.ContenidoCursoInscripcion.Add(objContenidoCursoInscripcion);
                    objModel.SaveChanges();
                }
                
                return objContenidoCursoInscripcion.IdContenidoCursoInscripcion;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GuardarContenidoCursoInscripcion(ContenidoCursoInscripcion objContenidoCursoInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                if (objContenidoCursoInscripcion.IdContenidoCursoInscripcion == 0)
                {
                    objModel.ContenidoCursoInscripcion.Add(objContenidoCursoInscripcion);
                }
                else
                {
                    ContenidoCursoInscripcion objContenidoCursoInscripcionBD = objModel.ContenidoCursoInscripcion.FirstOrDefault(c => c.IdContenidoCursoInscripcion == objContenidoCursoInscripcion.IdContenidoCursoInscripcion);
                    objContenidoCursoInscripcionBD.IdContenidoCurso = objContenidoCursoInscripcion.IdContenidoCurso;
                    objContenidoCursoInscripcionBD.IdInscripcion = objContenidoCursoInscripcion.IdInscripcion;
                    objContenidoCursoInscripcionBD.FechaHoraRevision = objContenidoCursoInscripcion.FechaHoraRevision;
                }
                objModel.SaveChanges();
                return objContenidoCursoInscripcion.IdContenidoCursoInscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
