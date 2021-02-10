using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class InscripcionDA
    {
        public IQueryable<Inscripcion> ListarInscripcion()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Inscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public Inscripcion ObtenerInscripcion(int IdInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcionBD = objModel.Inscripcion.SingleOrDefault(x => x.IdInscripcion == IdInscripcion);
                return objInscripcionBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int IngresarInscripcion(Inscripcion objInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.Inscripcion.Add(objInscripcion);
                objModel.SaveChanges();
                return objInscripcion.IdInscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarInscripcion(Inscripcion objInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcionBD = objModel.Inscripcion.SingleOrDefault(x => x.IdInscripcion == objInscripcion.IdInscripcion);
                objInscripcionBD.IdCursoSlot = objInscripcion.IdCursoSlot;
                objInscripcionBD.IdInscripcion = objInscripcion.IdInscripcion;
                objInscripcionBD.IdTrabajador = objInscripcion.IdTrabajador;
                objInscripcionBD.FechaHoraUltimaActualizacion = objInscripcion.FechaHoraUltimaActualizacion;
                if (objInscripcion.Certificado != null)
                    objInscripcionBD.Certificado = objInscripcion.Certificado;
                objInscripcionBD.FechaHoraAprobacion = objInscripcion.FechaHoraAprobacion;
                if (objInscripcion.FechaHoraDescargaCertificado != null)
                    objInscripcionBD.FechaHoraDescargaCertificado = objInscripcion.FechaHoraDescargaCertificado;
                if (objInscripcion.Puntaje != null)
                objInscripcionBD.Puntaje = objInscripcion.Puntaje;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarInscripcion(int IdInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcionBD = objModel.Inscripcion.SingleOrDefault(x => x.IdInscripcion == IdInscripcion);
                objModel.Inscripcion.Remove(objInscripcionBD);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Inscripcion ObtenerInscripcionPorTrabajadorYCursoSlot(int IdTrabajador, int IdCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcion = objModel.Inscripcion.FirstOrDefault(x => x.IdTrabajador == IdTrabajador && x.IdCursoSlot == IdCursoSlot);
                return objInscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Inscripcion> ListarInscripcionPorCursoSlot(int IdCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Inscripcion.Where(x => x.IdCursoSlot == IdCursoSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Inscripcion> ListarInscripcionPorUsuario(int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Inscripcion.Where(x => x.IdTrabajador == IdTrabajador);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Inscripcion ObtenerInscripcionPorTrabajadorYCursoSlotAprobado(int IdTrabajador, int IdCursoSlot)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcion = objModel.Inscripcion.FirstOrDefault(x => x.IdTrabajador == IdTrabajador && x.IdCursoSlot == IdCursoSlot && x.Intento.Any(i => i.Aprobado == true));
                //Inscripcion objInscripcion = objModel.Inscripcion.FirstOrDefault(x => x.IdTrabajador == IdTrabajador && x.IdCursoSlot == IdCursoSlot && x.FechaHoraAprobacion != null);
                return objInscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Inscripcion GuardarInscripcion(int IdCursoSlot, int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcion = objModel.Inscripcion.FirstOrDefault(x => x.IdCursoSlot == IdCursoSlot && x.IdTrabajador == IdTrabajador);
                if (objInscripcion == null)
                {
                    objInscripcion = new Inscripcion();
                    objInscripcion.IdCursoSlot = IdCursoSlot;
                    objInscripcion.IdTrabajador = IdTrabajador;
                    objInscripcion.FechaHoraRegistro = DateTimeHelper.PeruDateTime;
                    objInscripcion.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                    objModel.Inscripcion.Add(objInscripcion);
                    objModel.SaveChanges();
                }

                return objInscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Inscripcion ObtenerInscripcion(int IdCursoSlot, int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcion = objModel.Inscripcion.FirstOrDefault(x => x.IdCursoSlot == IdCursoSlot && x.IdTrabajador == IdTrabajador);

                return objInscripcion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Inscripcion ObtenerGuardarFechas(int IdCursoSlot, int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcionBD = objModel.Inscripcion.FirstOrDefault(x => x.IdCursoSlot == IdCursoSlot && x.IdTrabajador == IdTrabajador);
                if (objInscripcionBD != null)
                {

                    if (objInscripcionBD.FechaHoraRegistro == null)
                    {
                        objInscripcionBD.IdCursoSlot = IdCursoSlot;
                        objInscripcionBD.IdTrabajador = IdTrabajador;
                        objInscripcionBD.FechaHoraRegistro = DateTimeHelper.PeruDateTime;
                        objInscripcionBD.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                    }
                    else
                    {
                        objInscripcionBD.IdCursoSlot = IdCursoSlot;
                        objInscripcionBD.IdTrabajador = IdTrabajador;
                        objInscripcionBD.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                    }
                    objModel.SaveChanges();
                }

                return objInscripcionBD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public IQueryable<Inscripcion> ListarInscripcionTerminado()
        //{
        //    try
        //    {
        //        SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
        //        return objModel.Inscripcion.Where(i => i.Terminado == true);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public double ObtenerSumaPuntajeTrabajador(int IdTrabajador)
        {
            try
            {
                double sumaPuntajeTrabajador = 0;
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                int Year = DateTimeHelper.PeruDateTime.Year;
                if (objModel.Inscripcion.Where(i => i.IdTrabajador == IdTrabajador && i.CursoSlot.Ano == Year).Count() > 0)
                {
                    sumaPuntajeTrabajador = objModel.Inscripcion.Where(i => i.IdTrabajador == IdTrabajador && i.CursoSlot.Ano == Year).Sum(i => i.Puntaje ?? 0);
                }
                else
                    sumaPuntajeTrabajador = 0;

                return sumaPuntajeTrabajador;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void RegistrarDescargaCertificado(int IdInscripcion)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Inscripcion objInscripcion = objModel.Inscripcion.FirstOrDefault(x => x.IdInscripcion == IdInscripcion);
                objInscripcion.FechaHoraDescargaCertificado = DateTimeHelper.PeruDateTime;
                objModel.SaveChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
