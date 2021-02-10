using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.BL.BE;
using ZP.SOOM.CursosVirtuales.DL.DA;

namespace ZP.SOOM.CursosVirtuales.BL.BC
{
    public class ReporteBC
    {
        public string GenerarCSVReporteStatusCurso(int IdCursoSlot)
        {
            try
            {
                CursoBC objCursoBC = new CursoBC();
                MapaBC objMapaBC = new MapaBC();

                CursoSlot objCursoSlot = objMapaBC.ObtenerCursoSlot(IdCursoSlot);

                List<ReporteStatus> lstReporteStatus = new List<ReporteStatus>();
                IQueryable<Trabajador> lstTrabajador = new UsuarioBC().ListarTrabajadores();
                foreach (Trabajador objTrabajador in lstTrabajador)
                {
                    ReporteStatus objReporteStatus = new ReporteStatus();
                    objReporteStatus.Trabajador = objTrabajador;
                    Intento objIntento = objTrabajador.Inscripcion.FirstOrDefault(i => i.IdCursoSlot == IdCursoSlot).Intento.OrderByDescending(i => i.Nota).FirstOrDefault();
                    if (objIntento != null)
                    {
                        objReporteStatus.Ingresado = objIntento.Aprobado != null;
                        objReporteStatus.TerminoCurso = objIntento.Terminado;

                        objReporteStatus.TotalMateriales = objIntento.Inscripcion.MaterialInscripcion.Count;
                        objReporteStatus.MaterialesVisto = objIntento.Inscripcion.MaterialInscripcion.Where(x => x.Visto == true).Count();

                        objReporteStatus.RespuestasIncorrectas = objIntento.Respuesta.Where(r => r.Correcta != true).Count();
                        objReporteStatus.RespuestasCorrectas = objIntento.Respuesta.Where(x => x.Correcta == true).Count();

                        objReporteStatus.Porcentaje = objReporteStatus.RespuestasCorrectas * 100.0 / (objIntento.Respuesta.Count == 0 ? 1 : objIntento.Respuesta.Count);

                        objReporteStatus.Nota = objIntento.Nota;
                        objReporteStatus.Aprobado = objIntento.Aprobado;
                        objReporteStatus.CertificadoDescargado = objIntento.FechaHoraCertificadoDescargado != null;
                        objReporteStatus.TituloCurso = objIntento.Inscripcion.CursoSlot.Titulo;
                        objReporteStatus.FechaPrimeraVez = objIntento.Inscripcion.CursoSlot.Inscripcion.Where(i => i.IdTrabajador == objTrabajador.IdTrabajador).Min(i => i.FechaHoraRegistro);
                        objReporteStatus.FechaUltimaVez = objIntento.Inscripcion.CursoSlot.Inscripcion.Where(i => i.IdTrabajador == objTrabajador.IdTrabajador).Max(i => i.FechaHoraUltimaActualizacion);
                        objReporteStatus.Respuestas = objIntento.Respuesta.OrderBy(r => r.IdPregunta).ToList();

                    }
                    lstReporteStatus.Add(objReporteStatus);
                }

                //Arma cabecera
                string csv = "#\tNOMBRE Y APELLIDOS\tDNI\tCORREO\tFECHA DE INGRESO\tCOMPAÑIA\tROL\tGERENCIA\tAREA\tPUESTO\tJEFE\tGRUPO OCUPACIONAL\tSEDE\tINGRESADO\tFECHA PRIMER INGRESO\tFECHA ULTIMO INGRESO\tTERMINO\tMATERIALES ABIERTOS\tTOTAL MATERIALES\tRESPUESTAS CORRECTAS\tRESPUESTAS INCORRECTAS\tPORCENTAJE\tNOTA\tAPROBADO\tDESCARGO CERTIFICADO\n";


                ////Arma contenido
                for (int i = 0; i < lstReporteStatus.Count; i++)
                    csv += (i + 1) + "\t" + lstReporteStatus[i].Trabajador.Nombres + " " + lstReporteStatus[i].Trabajador.PrimerApellido + " " + lstReporteStatus[i].Trabajador.SegundoApellido + "\t" +
                        lstReporteStatus[i].Trabajador.DNI + "\t" +
                        lstReporteStatus[i].Trabajador.Email + "\t" +
                        lstReporteStatus[i].Trabajador.FechaIngreso.Value.ToString("dd/MM/yyy HH:mm") + "\t" +
                        lstReporteStatus[i].Trabajador.Compañia + "\t" +
                        lstReporteStatus[i].Trabajador.Rol + "\t" +
                        (lstReporteStatus[i].Trabajador.Gerencia ?? string.Empty) + "\t" +
                        (lstReporteStatus[i].Trabajador.Area ?? string.Empty) + "\t" +
                        (lstReporteStatus[i].Trabajador.Puesto ?? string.Empty) + "\t" +
                        (lstReporteStatus[i].Trabajador.Jefe == null ? string.Empty : (lstReporteStatus[i].Trabajador.Jefe.Nombres + "" + lstReporteStatus[i].Trabajador.Jefe.PrimerApellido + " " + lstReporteStatus[i].Trabajador.Jefe.SegundoApellido)) + "\t" +
                        (lstReporteStatus[i].Trabajador.GrupoOcupacional == null ? string.Empty : lstReporteStatus[i].Trabajador.GrupoOcupacional.Nombre) + "\t" +
                        (lstReporteStatus[i].Trabajador.Sede ?? string.Empty) + "\t" +
                        (lstReporteStatus[i].Ingresado ? "Sí" : "No") + "\t" +
                        (lstReporteStatus[i].FechaPrimeraVez == null ? string.Empty : lstReporteStatus[i].FechaPrimeraVez.Value.ToString("dd/MM/yyyy HH:mm")) + "\t" +
                        (lstReporteStatus[i].FechaUltimaVez == null ? string.Empty : lstReporteStatus[i].FechaUltimaVez.Value.ToString("dd/MM/yyyy HH:mm")) + "\t" +
                        (lstReporteStatus[i].TerminoCurso ? "Sí" : "No") + "\t" +
                        lstReporteStatus[i].MaterialesVisto + "\t" +
                        lstReporteStatus[i].TotalMateriales + "\t" +
                        lstReporteStatus[i].RespuestasCorrectas + "\t" +
                        lstReporteStatus[i].RespuestasIncorrectas + "\t" +
                        lstReporteStatus[i].Porcentaje + "%\t" +
                        (lstReporteStatus[i].Nota ?? 0) + "\t" +
                        (lstReporteStatus[i].Aprobado == null ? string.Empty : (lstReporteStatus[i].Aprobado == true ? "Si" : "No")) + "\t" +
                        (lstReporteStatus[i].CertificadoDescargado == true ? "Sí" : "No") + "\n";

                return csv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistrarDescargaCertificado(int IdIntento)
        {
            try
            {
                new IntentoDA().RegistrarDescargaCertificado(IdIntento);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GenerarCSVReporteExamenCurso(int IdCursoSlot)
        {
            try
            {
                CursoBC objCursoBC = new CursoBC();
                MapaBC objMapaBC = new MapaBC();
                CursoSlot objCursoSlot = objMapaBC.ObtenerCursoSlot(IdCursoSlot);

                List<ReporteStatus> lstReporteStatus = new List<ReporteStatus>();
                IQueryable<Trabajador> lstTrabajador = new UsuarioBC().ListarTrabajadores();
                List<Pregunta> lstPreguntas = objCursoSlot.Pregunta.ToList();
                foreach (Trabajador objTrabajador in lstTrabajador)
                {
                    ReporteStatus objReporteStatus = new ReporteStatus();
                    objReporteStatus.Trabajador = objTrabajador;
                    Intento objIntento = objTrabajador.Inscripcion.FirstOrDefault(i => i.IdCursoSlot == IdCursoSlot).Intento.OrderByDescending(i => i.Nota).FirstOrDefault();
                    if (objIntento != null)
                    {
                        objReporteStatus.Ingresado = objIntento.Aprobado != null;
                        objReporteStatus.TerminoCurso = objIntento.Terminado;

                        objReporteStatus.TotalMateriales = objIntento.Inscripcion.MaterialInscripcion.Count;
                        objReporteStatus.MaterialesVisto = objIntento.Inscripcion.MaterialInscripcion.Where(x => x.Visto == true).Count();

                        objReporteStatus.RespuestasIncorrectas = objIntento.Respuesta.Where(x => x.Correcta != true).Count();
                        objReporteStatus.RespuestasCorrectas = objIntento.Respuesta.Where(x => x.Correcta == true).Count();

                        objReporteStatus.Porcentaje = objReporteStatus.RespuestasCorrectas * 100.0 / (objIntento.Respuesta.Count == 0 ? 1 : objIntento.Respuesta.Count);

                        objReporteStatus.Nota = objIntento.Nota;
                        objReporteStatus.Aprobado = objIntento.Aprobado;
                        objReporteStatus.CertificadoDescargado = objIntento.FechaHoraCertificadoDescargado != null;
                        objReporteStatus.TituloCurso = objIntento.Inscripcion.CursoSlot.Titulo;
                        objReporteStatus.FechaPrimeraVez = objIntento.FechaHoraRegistro;
                        objReporteStatus.FechaUltimaVez = objIntento.FechaHoraUltimaActualizacion;
                        objReporteStatus.Respuestas = objIntento.Respuesta.OrderBy(r => r.IdPregunta).ToList();
                        objReporteStatus.Preguntas = lstPreguntas.ToList();
                    }
                    lstReporteStatus.Add(objReporteStatus);
                }


                //Preguntas
                string preguntas = string.Empty;
                for (int i = 0; i < lstPreguntas.Count(); i++)
                {
                    if (i > 0)
                    {
                        preguntas += "\t";
                    }
                    preguntas += lstPreguntas[i].Contenido;

                }
                //Arma cabecera
                string csv = "#\tNOMBRE Y APELLIDOS\tDNI\tCORREO\tFECHA DE INGRESO\tCOMPAÑIA\tROL\tGERENCIA\tAREA\tPUESTO\tJEFE\tGRUPO OCUPACIONAL\tSEDE\t" + preguntas + "\tTOTAL CORRECTAS\tTOTAL INCORRECTAS\tPORCENTAJE\tNOTA\tAPROBADO\tDESCARGO CERTIFICADO\n";
                ////Arma contenido

                for (int i = 0; i < lstReporteStatus.Count; i++)
                {
                    string respuesta = string.Empty;
                    foreach (Pregunta objPregunta in lstPreguntas)
                    {
                        Respuesta objRespuesta = lstReporteStatus[i].Respuestas == null ? null : lstReporteStatus[i].Respuestas.FirstOrDefault(r => r.IdPregunta == objPregunta.IdPregunta);
                        respuesta += (objRespuesta == null ? "-" : (objRespuesta.Correcta == true ? "O" : "X"));
                        respuesta += "\t";
                    }

                    csv += (i + 1) + "\t" + lstReporteStatus[i].Trabajador.Nombres + " " + lstReporteStatus[i].Trabajador.PrimerApellido + " " + lstReporteStatus[i].Trabajador.SegundoApellido + "\t" +
                     lstReporteStatus[i].Trabajador.DNI + "\t" +
                     lstReporteStatus[i].Trabajador.Email + "\t" +
                     lstReporteStatus[i].Trabajador.FechaIngreso.Value.ToString("dd/MM/yyy HH:mm") + "\t" +
                     lstReporteStatus[i].Trabajador.Compañia + "\t" +
                     lstReporteStatus[i].Trabajador.Rol + "\t" +
                     lstReporteStatus[i].Trabajador.Gerencia + "\t" +
                     lstReporteStatus[i].Trabajador.Area + "\t" +
                     lstReporteStatus[i].Trabajador.Puesto + "\t" +
                     (lstReporteStatus[i].Trabajador.Jefe == null ? string.Empty : lstReporteStatus[i].Trabajador.Jefe.Nombres + "" + lstReporteStatus[i].Trabajador.Jefe.PrimerApellido + " " + lstReporteStatus[i].Trabajador.Jefe.SegundoApellido) + "\t" +
                     (lstReporteStatus[i].Trabajador.GrupoOcupacional == null ? string.Empty : lstReporteStatus[i].Trabajador.GrupoOcupacional.Nombre) + "\t" +
                     lstReporteStatus[i].Trabajador.Sede + "\t" +
                     respuesta +
                     lstReporteStatus[i].RespuestasCorrectas + "\t" +
                     lstReporteStatus[i].RespuestasIncorrectas + "\t" +
                     lstReporteStatus[i].Porcentaje + "%\t" +
                     lstReporteStatus[i].Nota + "\t" +
                     (lstReporteStatus[i].Aprobado == null ? string.Empty : (lstReporteStatus[i].Aprobado == true ? "Si" : "No")) + "\t" +
                     (lstReporteStatus[i].CertificadoDescargado == true ? "Sí" : "No") + "\n";
                }

                return csv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerarCSVReporteStatusUsuario(int IdUsuario)
        {
            try
            {
                CursoBC objCursoBC = new CursoBC();
                UsuarioBC objUsuarioBC = new UsuarioBC();
                Usuario objUsuario = objUsuarioBC.ObtenerUsuario(IdUsuario);
                Trabajador objTrabajador = objUsuarioBC.ObtenerTrabajador((int)objUsuario.IdTrabajador);
                List<Inscripcion> lstInscripcion = objCursoBC.ListarInscripcionPorUsuario(objTrabajador.IdTrabajador).ToList();


                //Arma cabecera
                string csv = "#\tCURSO\tINGRESADO\tFECHA PRIMER INGRESO\tFECHA ULTIMO INGRESO\tTERMINO\tMATERIALES ABIERTOS\tTOTAL MATERIALES\tRESPUESTAS CORRECTAS\tRESPUESTAS INCORRECTAS\tPORCENTAJE\tNOTA\tAPROBADO\tDESCARGO CERTIFICADO\n";


                ////Arma contenido
                foreach (Inscripcion objIncripcion in lstInscripcion)
                {
                     List<Intento> lstIntento = objIncripcion.Intento.ToList();
                    for (int i = 0; i < lstIntento.Count(); i++)
                    {
                        csv += (i + 1) + "\t" + lstIntento[i].Inscripcion.CursoSlot.Titulo + "\t" +
                        //(lstInscripcion[i].Ingresado ? "Sí" : "No") + "\t" +
                        (lstIntento[i].FechaHoraRegistro == null ? string.Empty : lstIntento[i].FechaHoraRegistro.Value.ToString("dd/MM/yyyy HH:mm")) + "\t" +
                        (lstIntento[i].FechaHoraUltimaActualizacion == null ? string.Empty : lstIntento[i].FechaHoraUltimaActualizacion.ToString("dd/MM/yyyy HH:mm")) + "\t" +
                        (lstIntento[i].Terminado ? "Sí" : "No") + "\t" +
                        (lstIntento[i].Inscripcion.MaterialInscripcion.Count > 0 ? lstIntento[i].Inscripcion.MaterialInscripcion.Where(m => m.Visto == true).Count() : 0) + "\t" +
                        lstIntento[i].Inscripcion.MaterialInscripcion.Count + "\t" +
                        (lstIntento[i].Respuesta.Count > 0 ? lstIntento[i].Respuesta.Where(r => r.Correcta == true).Count() : 0) + "\t" +
                        (lstIntento[i].Respuesta.Count > 0 ? lstIntento[i].Respuesta.Where(r => r.Correcta == false).Count() : 0) + "\t" +
                        (lstIntento[i].Respuesta.Count > 0 ? lstIntento[i].Respuesta.Where(r => r.Correcta == true).Count() * 100.0 / lstIntento[i].Respuesta.Count() : 0) + "%\t" +
                        lstIntento[i].Nota + "\t" +
                        (lstIntento[i].Aprobado == null ? string.Empty : (lstIntento[i].Aprobado == true ? "Si" : "No")) + "\t" +
                        (lstIntento[i].FechaHoraCertificadoDescargado.Value != null ? "Sí" : "No") + "\n";
                    }
                }
                    

                return csv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<IDictionary<string, object>> ObtenerReporteGeneral(string NombreCampo, int Ano, string[] Intentos, string FechaInicio, string FechaFin, string[] OpcionSeleccionada, int?[] IdCursoSlot, string[] Estatus)
        {
            try
            {
                ReporteDA objReporteDA = new ReporteDA();
                int[] listaTrabajadores = new TrabajadorDA().ListarTrabajadoresFiltro(NombreCampo, OpcionSeleccionada, Ano);
                return objReporteDA.ObtenerReporteGeneral(Ano, Intentos, FechaInicio, FechaFin, listaTrabajadores, IdCursoSlot, Estatus);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
