using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Configuration;
using Dapper;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class TrabajadorDA
    {

        public IQueryable<Trabajador> ListarTrabajadores()
        {
            try
            {

                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.Where(t => t.Eliminado != true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Trabajador> ListarTrabajadoresPorGrupo(int IdGrupo)
        {
            try
            {

                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                List<Trabajador> lstTrabajadorGrupo = objModel.GrupoTrabajador.Where(g => g.IdGrupo == IdGrupo && g.Trabajador.Eliminado != true).Select(t => t.Trabajador).ToList();
                return lstTrabajadorGrupo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<GrupoTrabajador> ListarGrupoTrabajador(int IdTrabajador)
        {
            try
            {

                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoTrabajador.Where(t => t.IdTrabajador == IdTrabajador && t.Grupo.Nombre != "Todos").OrderBy(t => t.Grupo.Nombre);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int[] ListarTrabajadoresFiltro(string NombreCampo, string[] OpcionSeleccionada, int Ano)
        {
            try
            {

                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                switch (NombreCampo)
                {
                    case Constants.ReporteGeneral.Campos.CATEGORIA:
                        return objModel.Trabajador.Where(t => OpcionSeleccionada.Contains(t.GrupoOcupacional.Nombre)).Select(t => t.IdTrabajador).ToArray();
                    case Constants.ReporteGeneral.Campos.COMPANIA:
                        return objModel.Trabajador.Where(t => OpcionSeleccionada.Contains(t.Compañia)).Select(t => t.IdTrabajador).ToArray();
                    case Constants.ReporteGeneral.Campos.GERENCIA:
                        return objModel.Trabajador.Where(t => OpcionSeleccionada.Contains(t.Gerencia)).Select(t => t.IdTrabajador).ToArray();
                    case Constants.ReporteGeneral.Campos.SEDE:
                        return objModel.Trabajador.Where(t => OpcionSeleccionada.Contains(t.Sede)).Select(t => t.IdTrabajador).ToArray();
                    case Constants.ReporteGeneral.Campos.SEGMENTO:
                        return objModel.Trabajador.Where(t => OpcionSeleccionada.Any(op => t.GrupoTrabajador.Any(gt => gt.FechaHoraRegistro.Year <= Ano && (gt.FechaHoraDesasignacion == null || gt.FechaHoraDesasignacion.Value.Year >= Ano) && gt.Grupo.Nombre == op))).Select(t => t.IdTrabajador).ToArray();
                    default:
                        return objModel.Trabajador.Select(t => t.IdTrabajador).ToArray();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> ListarAreas()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.Where(x => x.Eliminado != true).Select(x => x.Area).Distinct().ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<string> ListarCompanias()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<string> lstTrabajador = objModel.Trabajador.Where(c => c.Compañia != null).Select(c => c.Compañia).Distinct();
                return lstTrabajador.OrderBy(c => c).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<string> ListarSede()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<string> lstTrabajador = objModel.Trabajador.Where(s => s.Sede != null).Select(s => s.Sede).Distinct();
                return lstTrabajador.OrderBy(s => s).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<string> ListarGerencia()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                IQueryable<string> lstTrabajador = objModel.Trabajador.Where(g => g.Gerencia != null).Select(g => g.Gerencia).Distinct();
                return lstTrabajador.OrderBy(g => g).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Trabajador> ListarTrabajdoresporGrupoOcupacional(int? IdGrupoOcupacional)
        {
            try
            {

                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.Where(t => t.IdGrupoOcupacional == IdGrupoOcupacional && t.Eliminado != true).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int InsertarTrabajador(Trabajador objTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.Trabajador.Add(objTrabajador);
                objModel.SaveChanges();

                return objTrabajador.IdTrabajador;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Trabajador ObtenerTrabajador(int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.SingleOrDefault(t => t.IdTrabajador == IdTrabajador && t.Eliminado != true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Trabajador ObtenerTrabajadorJefeporNombre(string NombreJefe)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.SingleOrDefault(t => t.Nombres + " " + t.PrimerApellido + " " + t.SegundoApellido == NombreJefe && t.Eliminado != true && t.Estado != Constants.Trabajador.Estado.INACTIVO);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Trabajador ObtenerTrabajadorGrupoOcupacional(int IdGrupoOcupacional)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.SingleOrDefault(t => t.IdGrupoOcupacional == IdGrupoOcupacional);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Trabajador ObtenerTrabajador(String DNI)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.FirstOrDefault(t => t.DNI == DNI && t.Eliminado != true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Trabajador ObtenerTrabajadorPorCodigo(String Codigo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.FirstOrDefault(t => t.Codigo == Codigo && t.Eliminado != true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Trabajador ObtenerTrabajadorCabeza()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.FirstOrDefault(t => t.IdTrabajadorJefe == null && t.Eliminado != true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void ActualizarTrabajador(Trabajador objTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                var objTrabajadorBd = objModel.Trabajador.SingleOrDefault(t => t.IdTrabajador == objTrabajador.IdTrabajador);
                objTrabajadorBd.Nombres = objTrabajador.Nombres;
                objTrabajadorBd.PrimerApellido = objTrabajador.PrimerApellido;
                objTrabajadorBd.SegundoApellido = objTrabajador.SegundoApellido;
                objTrabajadorBd.Codigo = objTrabajador.Codigo;
                objTrabajadorBd.DNI = objTrabajador.DNI;
                objTrabajadorBd.Email = objTrabajador.Email;
                objTrabajadorBd.Puesto = objTrabajador.Puesto;
                objTrabajadorBd.Area = objTrabajador.Area;
                objTrabajadorBd.IdTrabajadorJefe = objTrabajador.IdTrabajadorJefe;
                objTrabajadorBd.Estado = objTrabajador.Estado;
                objTrabajadorBd.FechaNacimiento = objTrabajador.FechaNacimiento;
                objTrabajadorBd.Genero = objTrabajador.Genero;
                objTrabajadorBd.Celular = objTrabajador.Celular;
                objTrabajadorBd.Sede = objTrabajador.Sede;
                objTrabajadorBd.Provincia = objTrabajador.Provincia;
                objTrabajadorBd.Region = objTrabajador.Region;
                objTrabajadorBd.Pais = objTrabajador.Pais;
                objTrabajadorBd.FechaIngreso = objTrabajador.FechaIngreso;
                objTrabajadorBd.IdGrupoOcupacional = objTrabajador.IdGrupoOcupacional;
                objTrabajadorBd.TipoContrato = objTrabajador.TipoContrato;
                objTrabajadorBd.Nivel = objTrabajador.Nivel;
                objTrabajadorBd.Gerencia = objTrabajador.Gerencia;
                objTrabajadorBd.AreaSuperior = objTrabajador.AreaSuperior;
                objTrabajadorBd.Rol = objTrabajador.Rol;
                objTrabajadorBd.Compañia = objTrabajador.Compañia;
                if (objTrabajador.Foto != null)
                    objTrabajadorBd.Foto = objTrabajador.Foto;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public int? InsertarJefeAlTrabajador(int IdTrabajador, int? idTrabajadorJefe)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Trabajador objTrabajador = objModel.Trabajador.SingleOrDefault(t => t.IdTrabajador == IdTrabajador);
                objTrabajador.IdTrabajadorJefe = idTrabajadorJefe;
                objModel.SaveChanges();

                return objTrabajador.IdTrabajadorJefe;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EliminarTrabajador(int IdTrabajador)
        {

            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                var objTrabajador = objModel.Trabajador.SingleOrDefault(t => t.IdTrabajador == IdTrabajador);
                objTrabajador.Eliminado = true;
                objTrabajador.IdTrabajadorJefe = null;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ActualizarFoto(Trabajador objTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                var objModelbd = objModel.Trabajador.SingleOrDefault(t => t.IdTrabajador == objTrabajador.IdTrabajador);
                objModelbd.Foto = objTrabajador.Foto;
                objModel.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<Trabajador> ListarSubordinados(int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.Where(t => t.IdTrabajadorJefe == IdTrabajador && t.Eliminado != true).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void ActualizarEstado(int IdTrabajador, String Estado)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Trabajador objTrabajador = objModel.Trabajador.SingleOrDefault(t => t.IdTrabajador == IdTrabajador);
                objTrabajador.Estado = Estado;

                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<IDictionary<string, object>> DescargarBaseTrabajadores()
        {
            try
            {
                IEnumerable<IDictionary<string, object>> resultado;

                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SOOMCursosVirtuales"].ConnectionString))
                {
                    cnn.Open();

                    resultado = (IEnumerable<IDictionary<string, object>>)
                                cnn.Query(sql: "[DescargarBaseTrabajadores]",
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10000000);
                }

                return resultado;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
