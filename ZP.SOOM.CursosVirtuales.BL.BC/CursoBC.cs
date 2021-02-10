using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.DL.DA;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.BL.BC
{
    public class CursoBC
    {
        //Variables de progreso
        static double totalProgress;
        static double currentProgress;

        public int GetProgress()
        {
            try
            {
                if (currentProgress == -1)
                {
                    return (int)currentProgress;
                }
                int numero = (int)((currentProgress / totalProgress) * 100);
                return (numero);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> ListarAnos()
        {
            try
            {
                return new CursoSlotDA().ListarAnos();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region Logica Cursos

        public IQueryable<Curso> ListarCurso()
        {
            try
            {
                return new CursoDA().ListarCurso();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListarCursoSlotIngresados(int IdTrabajador)
        {
            try
            {
                return new CursoSlotDA().ListarCursoSlotIngresados(IdTrabajador); ;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Curso> ListarCursosNoAsignados()
        {
            try
            {
                return new CursoDA().ListarCursosNoAsignados();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListarCursoSlotActivo()
        {
            try
            {
                return new CursoSlotDA().ListarCursoSlotActivo();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListarCursoSlotAsignado(int IdSlot)
        {
            try
            {
                return new CursoSlotDA().ListarCursoSlotAsignado(IdSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Curso> FiltarCurso(string Tipo, string Valor, string Estado)
        {
            try
            {
                return new CursoDA().FiltarCurso(Tipo, Valor, Estado);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Curso ObtenerCurso(int? IdCurso)
        {
            try
            {
                return new CursoDA().ObtenerCurso(IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AsignarGruposACurso(int IdCurso, int [] IdGrupo)
        {
            try
            {
                new GrupoCursoDA().AsignarGruposACurso(IdCurso, IdGrupo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Curso ObtenerCursoAsignado(int? IdCurso)
        {
            try
            {
                return new CursoDA().ObtenerCursoAsignado(IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public int GuardarCurso(Curso objCurso)
        {
            try
            {
                if (objCurso.IdCurso == 0)
                    return new CursoDA().InsertarCurso(objCurso);
                else
                {
                    new CursoDA().ActualizarCurso(objCurso);
                    CursoSlot objCursoSlot = new CursoSlotDA().ObtenerCursoSlotPorCursoActivo(objCurso.IdCurso);
                    if (objCursoSlot != null)
                    {
                        objCursoSlot.Titulo = objCurso.Titulo;
                        objCursoSlot.Horas = objCurso.Horas;
                        objCursoSlot.Puntos = objCurso.Puntos;
                        objCursoSlot.Codigo = objCurso.Codigo;
                        objCursoSlot.Descripcion = objCurso.Descripcion;

                        new CursoSlotDA().ActualizarCursoSlot(objCursoSlot);
                    }
                    return objCurso.IdCurso;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarCurso(int IdCurso)
        {
            try
            {
                Curso objCurso = new CursoDA().ObtenerCurso(IdCurso);
                //Eliminar material
                foreach (Material objMaterial in objCurso.Material)
                    new MaterialDA().EliminarMaterial(objMaterial.IdMaterial);
                //Eliminar contenido curso
                foreach (ContenidoCurso objContenidoCurso in objCurso.ContenidoCurso)
                    new ContenidoCursoDA().EliminarContenidoCurso(objContenidoCurso.IdContenidoCurso);
                //Eliminar curso
                new CursoDA().EliminarCurso(IdCurso);
                //Actualizar historial
                new CursoSlotDA().ActualizarCursoSlotEstadoHistorial(IdCurso);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Logica Material

        public IQueryable<Material> ListarMaterial()
        {
            try
            {
                return new MaterialDA().ListarMaterial();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Material ObtenerMaterial(int IdMaterial)
        {
            try
            {
                return new MaterialDA().ObtenerMaterial(IdMaterial);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GuardarMaterial(Material objMaterial)
        {
            try
            {
                if (objMaterial.IdMaterial == 0)
                {
                    return new MaterialDA().InsertarMaterial(objMaterial);
                }
                else
                {
                    new MaterialDA().ActualizarMaterial(objMaterial);
                    return objMaterial.IdMaterial;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GuardarContenidoCurso(ContenidoCurso objContenidoCurso)
        {
            try
            {
                if (objContenidoCurso.IdContenidoCurso == 0)
                {
                    return new ContenidoCursoDA().InsertarContenidoCurso(objContenidoCurso);
                }
                else
                {
                    new ContenidoCursoDA().ActualizarContenidoCurso(objContenidoCurso);
                    return objContenidoCurso.IdContenidoCurso;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualiazarVistoMaterialInscripcion(int IdMaterialInscripcion)
        {
            try
            {
                new MaterialInscripcionDA().ActualiazarVistoMaterialInscripcion(IdMaterialInscripcion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void RegistrarVistaContenidoCurso(int IdContenidoCurso, int IdTrabajador)
        {
            try
            {
                ContenidoCursoDA objContenidoCursoDA = new ContenidoCursoDA();
                CursoSlotDA objCursoSlotDA = new CursoSlotDA();
                InscripcionDA objInscripcionDA = new InscripcionDA();

                CursoSlot objCursoSlot = objCursoSlotDA.ObtenerCursoSlotActivoPorIdCurso(objContenidoCursoDA.ObtenerContenidoCursoPorIdContenidoCurso(IdContenidoCurso).IdCurso);
                Inscripcion objInscripcion = objInscripcionDA.ObtenerInscripcionPorTrabajadorYCursoSlot(IdTrabajador, objCursoSlot.IdCursoSlot);

                new ContenidoCursoInscripcionDA().RegistrarContenidoCursoInscripcion(IdContenidoCurso, objInscripcion.IdInscripcion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarMaterial(int IdMaterial)
        {
            try
            {
                new MaterialDA().EliminarMaterial(IdMaterial);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarContenidoCurso(int IdContenidoCurso)
        {
            try
            {
                new ContenidoCursoDA().EliminarContenidoCurso(IdContenidoCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion



        public Curso ObtenerCursoPorCodigo(string Codigo)
        {
            try
            {
                return new CursoDA().ObtenerCursoPorCodigo(Codigo);
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
                return new InscripcionDA().ObtenerInscripcionPorTrabajadorYCursoSlotAprobado(IdTrabajador, IdCursoSlot);
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
                return new InscripcionDA().ObtenerInscripcionPorTrabajadorYCursoSlot(IdTrabajador, IdCursoSlot);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistrarResultadoCurso(String Codigo, double Nota, double NotaMaxima, bool Aprobado, int IdTrabajador, string Certificado)
        {
            try
            {
                InscripcionDA objInscripcionDA = new InscripcionDA();
                CursoSlotDA objCursoSlotDA = new CursoSlotDA();

                //Obtener (DA) el CursoSlot activo asociado al Código de Curso
                CursoSlot objCursoSlot = objCursoSlotDA.ObtenerCursoSlotActivoPorCodigoCurso(Codigo);
                //Obtener (DA) el objeto Inscripción asociado a dicho trabajador y dicho CursoSlot
                Inscripcion objInscripcion = new MapaBC().ObtenerInscripcion(objCursoSlot.IdCursoSlot, IdTrabajador);
                
                if (objInscripcion != null)
                {
                    Intento objIntento = new Intento();

                    if (objIntento != null)
                    {
                        objIntento.Nota = Nota;
                        objIntento.IdIntento = objInscripcion.Intento.FirstOrDefault(i => !i.Terminado) != null ? objInscripcion.Intento.FirstOrDefault(i => !i.Terminado).IdIntento : 0;
                        objIntento.NotaMaxima = NotaMaxima;
                        objIntento.Puntaje = (objCursoSlot.Puntos * Nota) / NotaMaxima;
                        objIntento.Aprobado = Aprobado;
                        if (objIntento.Aprobado == true)
                            objIntento.FechaHoraAprobacion = DateTimeHelper.PeruDateTime;
                        objIntento.Terminado = true;
                        objIntento.IdInscripcion = objInscripcion.IdInscripcion;
                        objIntento.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                        if (Certificado != null)
                            objIntento.Certificado = Certificado;
                        //Guardar (DA) el objeto en BD
                        new IntentoDA().GuardarIntento(objIntento);
                    }

                    if (objIntento.Aprobado == true)
                        objInscripcion.FechaHoraAprobacion = DateTimeHelper.PeruDateTime;
                    if(Certificado != null)
                        objInscripcion.Certificado = Certificado;
                    int PuntajeMaximo = new IntentoDA().ObtenerMayorPuntaje(objInscripcion.IdInscripcion);
                    double PuntajeActual = (objCursoSlot.Puntos * Nota) / NotaMaxima ?? 0;
                    if (PuntajeActual >= PuntajeMaximo)
                        objInscripcion.Puntaje = PuntajeActual;

                    new InscripcionDA().ActualizarInscripcion(objInscripcion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistrarResultadoCursoEnInscripcion(String Codigo, int IdTrabajador, string Certificado)
        {
            try
            {
                InscripcionDA objInscripcionDA = new InscripcionDA();
                CursoSlotDA objCursoSlotDA = new CursoSlotDA();

                //Obtener (DA) el CursoSlot activo asociado al Código de Curso
                CursoSlot objCursoSlot = objCursoSlotDA.ObtenerCursoSlotActivoPorCodigoCurso(Codigo);
                //Obtener (DA) el objeto Inscripción asociado a dicho trabajador y dicho CursoSlot
                Inscripcion objInscripcion = new MapaBC().ObtenerInscripcion(objCursoSlot.IdCursoSlot, IdTrabajador);

                if (objInscripcion != null)
                {
                    objInscripcion.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                    if (Certificado != null)
                    {
                        objInscripcion.Certificado = Certificado;
                        objInscripcion.Puntaje = objInscripcion.CursoSlot.Curso.Puntos;
                    }

                    new InscripcionDA().ActualizarInscripcion(objInscripcion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistrarRespuestas(string Codigo, string Pregunta, string Respuesta, bool Correcto, int IdTrabajador)
        {
            try
            {
                CursoSlotDA objCursoSlotDA = new CursoSlotDA();
                PreguntaDA objPreguntaDA = new PreguntaDA();
                RespuestaDA objRespuestaDA = new RespuestaDA();
                InscripcionDA objInscripcionDA = new InscripcionDA();
                //Obtener (DA) el CursoSlot activo asociado al Código de Curso
                CursoSlot objCursoSlot = objCursoSlotDA.ObtenerCursoSlotActivoPorCodigoCurso(Codigo);
                //Obtener (DA) el objeto Inscripción asociado a dicho trabajador y dicho CursoSlot
                Inscripcion objInscripcion = new MapaBC().ObtenerInscripcion(objCursoSlot.IdCursoSlot, IdTrabajador);          
                Intento objIntentoPendiente = objInscripcion.Intento.FirstOrDefault(i => !i.Terminado);
                
                //Guardar (DA) el objeto pregunta en BD
                if (objIntentoPendiente == null)
                {
                    objIntentoPendiente = new Intento();
                    objIntentoPendiente.IdInscripcion = objInscripcion.IdInscripcion;
                    objIntentoPendiente.FechaHoraRegistro = DateTimeHelper.PeruDateTime;
                    objIntentoPendiente.FechaHoraUltimaActualizacion = DateTimeHelper.PeruDateTime;
                    objIntentoPendiente.Terminado = false;

                    new MapaBC().GuardarIntento(objIntentoPendiente);
                }

                int IdPregunta = objPreguntaDA.GuardarPregunta(Pregunta, objCursoSlot.IdCursoSlot);
                objRespuestaDA.GuardarRespuesta(IdPregunta, objIntentoPendiente.IdIntento, Respuesta, Correcto);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Logica Configuracion

        public IQueryable<Configuracion> ListarConfiguracion()
        {
            try
            {
                return new ConfiguracionDA().ListarConfiguracion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Configuracion ObtenerConfiguracion(int IdConfiguracion)
        {
            try
            {
                return new ConfiguracionDA().ObtenerConfiguracion(IdConfiguracion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public Configuracion ObtenerConfiguracionPorCodigo(string Codigo)
        {
            try
            {
                return new ConfiguracionDA().ObtenerConfiguracion(Codigo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int GuardarConfiguracion(Configuracion objConfiguracion)
        {
            try
            {
                if (objConfiguracion.IdConfiguracion == 0)
                {
                    return new ConfiguracionDA().InsertarConfiguracion(objConfiguracion);
                }
                else
                {
                    new ConfiguracionDA().ActulizarConfiguracion(objConfiguracion);
                    return objConfiguracion.IdConfiguracion;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarConfiguracionPorCodigo(string Codigo)
        {
            try
            {
                new ConfiguracionDA().EliminarConfiguracionPorCodigo(Codigo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion


        public IQueryable<Inscripcion> ListarInscripcion()
        {
            try
            {
                return new InscripcionDA().ListarInscripcion();
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
                return new InscripcionDA().ListarInscripcionPorCursoSlot(IdCursoSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Curso ObtenerCursoPorCursoSlot(int IdCursoSlot)
        {
            try
            {
                CursoSlot objCursoSlot = new MapaBC().ObtenerCursoSlot(IdCursoSlot);
                return new CursoDA().ObtenerCurso(objCursoSlot.IdCurso);
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
                return new InscripcionDA().ObtenerGuardarFechas(IdCursoSlot, IdTrabajador);
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
                return new InscripcionDA().ListarInscripcionPorUsuario(IdTrabajador);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Pregunta> ListarPreguntas()
        {
            try
            {
                return new PreguntaDA().ListarPregunta();
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
                return new PreguntaDA().ListarPreguntasPorCursoSlot(IdCursoSlot);
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
                return new RespuestaDA().ListarRespuestasPorPregunta(IdPregunta);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int IngresarMaterialInscripcion(MaterialInscripcion objMaterialInscripcion)
        {
            try
            {
                return new MaterialInscripcionDA().IngresarMaterialInscripcion(objMaterialInscripcion);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GuardarContenidoCursoInscripcion(ContenidoCursoInscripcion objContenidoCursoInscripcion)
        {
            try
            {
                return new ContenidoCursoInscripcionDA().GuardarContenidoCursoInscripcion(objContenidoCursoInscripcion);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public IQueryable<Inscripcion> ListarInscripcionTerminado()
        //{
        //    try
        //    {
        //        return new InscripcionDA().ListarInscripcionTerminado();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public MaterialInscripcion ObtenerMaterialInscripcionPorIdMaterialYIdInscripcion(int IdMaterial, int IdInscripcion)
        {
            try
            {
                return new MaterialInscripcionDA().ObtenerMaterialInscripcionPorIdMaterialYIdInscripcion(IdMaterial, IdInscripcion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<MaterialInscripcion> ObtenerMaterialObligatorio(int IdInscripcion, int IdCurso)
        {
            try
            {
                return new MaterialInscripcionDA().ObtenerMaterialObligatorio(IdInscripcion, IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
