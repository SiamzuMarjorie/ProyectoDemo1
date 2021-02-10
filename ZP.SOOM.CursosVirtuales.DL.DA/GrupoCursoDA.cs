using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class GrupoCursoDA
    {
        public IQueryable<GrupoCurso> ListarGrupo(int IdGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoCurso.Where(g => g.IdGrupo == IdGrupo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarFechaDesasignacion(int IdGrupoCurso)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                GrupoCurso objGrupoCurso = objModel.GrupoCurso.FirstOrDefault(g => g.IdGrupoCurso == IdGrupoCurso);
                objGrupoCurso.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime;

                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AsignarGruposACurso(int IdCurso, int[] IdGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Curso objCurso = objModel.Curso.FirstOrDefault(c => c.IdCurso == IdCurso);
                List<GrupoCurso> lstGrupoCurso = objModel.GrupoCurso.Where(gt => gt.IdCurso == IdCurso && gt.FechaHoraDesasignacion == null).ToList();

                if (IdGrupo != null)
                {
                    foreach (GrupoCurso objGrupoCurso in lstGrupoCurso)
                    {
                        if (!IdGrupo.Contains(objGrupoCurso.IdGrupo))
                        {
                            objGrupoCurso.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime;
                        }
                    }
                    foreach (int Grupo in IdGrupo.Where(g => lstGrupoCurso.All(s => s.IdGrupo != g)))
                    {
                        GrupoCurso objGrupoCurso = new GrupoCurso();
                        objGrupoCurso.IdGrupo = Grupo;
                        objGrupoCurso.IdCurso = IdCurso;
                        objGrupoCurso.FechaHoraRegistro = DateTimeHelper.PeruDateTime;

                        objModel.GrupoCurso.Add(objGrupoCurso);
                    }
                }
                else
                {
                    foreach (GrupoCurso objGrupoCurso in lstGrupoCurso)
                    {
                        objGrupoCurso.FechaHoraDesasignacion = DateTimeHelper.PeruDateTime;
                    }
                }
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
