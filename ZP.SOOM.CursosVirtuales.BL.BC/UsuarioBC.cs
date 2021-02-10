using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

using ZP.SOOM.CursosVirtuales.DL.DA;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.BL.BC
{
    public class UsuarioBC
    {

        static double totalProgress;
        static double currentProgress;
        static string mensajeError;

        public void EnviarNuevoPassword(String Email, String Nombre, String Username, String ContraseñaNueva, int IdUsuario)
        {
            String Contenido = "<div style='text-align: center;'><img src='" + Path.Combine(ConfigurationManager.AppSettings["UrlSOOM"], "Images/Mail/logo_ciudad_positiva.png") + "' /><br /><br />";
            Contenido += "<h2>Estimado(a) " + Nombre + "</h2>";
            Contenido += "<h1 style='color:#00c3c5;'>Has recibido tu nueva contraseña</h1><br/>";
            Contenido += "<center><h3>Usuario: " + Username + "</h3>";
            Contenido += "<h3>Contrase&ntilde;a: " + ContraseñaNueva + "</h3><br /><br />";
            Contenido += "<a style='display: inline-block;' href='" + Path.Combine(ConfigurationManager.AppSettings["UrlSOOM"], "Login")+"'><img style='width: 250px;' src='" + Path.Combine(ConfigurationManager.AppSettings["UrlSOOM"], "Images/Mail/btn_plataforma.png")+ "' /></a></div>";

            new NotificacionBC().EnviarEmail("¡Has recibido tu nueva contraseña!", Contenido, new String[] { Email });
        }

        public string ContrasenaRandom()
        {
            try
            {
                Guid obj = Guid.NewGuid();
                string Cadena = obj.ToString().Replace("-", "");
                string ContrasenaAleatoria = Cadena.Substring(0, 8);
                return ContrasenaAleatoria;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Usuario> ListarUsuario()
        {
            try
            {
                return new UsuarioDA().ListarUsuario();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<GrupoTrabajador> ListarGrupoTrabajador(int IdTrabajador)
        {
            try
            {
                return new TrabajadorDA().ListarGrupoTrabajador(IdTrabajador);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<Usuario> ListarUsuarioNoEliminado()
        {
            try
            {

                return new UsuarioDA().ListarUsuarioNoEliminado();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario EncontrarUsuario(string UsernameoCorreo)
        {
            try
            {
                return new UsuarioDA().EncontrarUsuario(UsernameoCorreo);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<GrupoOcupacional> ListarGrupoOcupacional()
        {

            try
            {
                return new GrupoOcupacionalDA().ListarGrupoOcupacional();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<string> ListarGrupoOcupacionalUsuario()
        {
            try
            {
                return new UsuarioDA().ListarGrupoOcupacionalUsuario();

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
                return new TrabajadorDA().ListarCompanias();

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
                return new TrabajadorDA().ListarSede();

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
                return new TrabajadorDA().ListarGerencia();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<string> ListarGrupoOcupacionales()
        {
            try
            {
                return new GrupoOcupacionalDA().ListarGrupoOcupacionales();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Usuario> ListarUsuarios(int IdUsuario)
        {
            try
            {
                return new UsuarioDA().ListarUsuarios(IdUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Usuario Login(string Username, string Password)
        {
            try
            {
                return new UsuarioDA().Login(Username, Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Trabajador> ListarPosibleJefe()
        {
            try
            {
                return new UsuarioDA().ListarPosibleJefe();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> ListarAreaUsuario()
        {
            try
            {
                return new UsuarioDA().ListarAreaUsuario();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GuardarUsuario(Usuario objUsuario, Trabajador objTrabajador, string GrupoOcupacionalNombre, bool CargaMasiva)
        {
            try
            {
                //return new TrabajadorReportDA().ObtenerTrabajadorReportCabeza(IdSprintOriginal);
                UsuarioDA objUsuarioDA = new UsuarioDA();
                TrabajadorDA objTrabajadorDA = new TrabajadorDA();
                if (objTrabajador.IdTrabajador > 0)
                {
                    GrupoOcupacional objGrupoOcupacional = new GrupoOcupacionalDA().ObtenerNombreGrupoOcupacional(GrupoOcupacionalNombre);
                    if (objGrupoOcupacional != null)
                    {
                        objTrabajador.IdGrupoOcupacional = objGrupoOcupacional.IdGrupoOcupacional;
                    }
                    else
                    {
                        GrupoOcupacional objGrupoOcupacionalNuevo = new GrupoOcupacional();
                        objGrupoOcupacionalNuevo.Nombre = GrupoOcupacionalNombre;
                        int IdGrupoOcupacional = new GrupoOcupacionalDA().InsertarGrupoOcupacional(objGrupoOcupacionalNuevo);
                        objTrabajador.IdGrupoOcupacional = IdGrupoOcupacional;
                    }
                    objTrabajadorDA.ActualizarTrabajador(objTrabajador);
                    objUsuarioDA.ActualizarUsuario(objUsuario, CargaMasiva);
                    //TrabajadorReportGuardado(objTrabajador);
                    return objUsuario.IdUsuario;
                }
                else
                {
                    //objTrabajador.EsCoach = false;
                    GrupoOcupacional objGrupoOcupacional = new GrupoOcupacionalDA().ObtenerNombreGrupoOcupacional(GrupoOcupacionalNombre);
                    if (objGrupoOcupacional != null)
                    {
                        objTrabajador.IdGrupoOcupacional = objGrupoOcupacional.IdGrupoOcupacional;
                    }
                    else
                    {
                        GrupoOcupacional objGrupoOcupacionalNuevo = new GrupoOcupacional();
                        objGrupoOcupacionalNuevo.Nombre = GrupoOcupacionalNombre;
                        int IdGrupoOcupacional = new GrupoOcupacionalDA().InsertarGrupoOcupacional(objGrupoOcupacionalNuevo);
                        objTrabajador.IdGrupoOcupacional = IdGrupoOcupacional;
                    }
                    int IdTrabajador = objTrabajadorDA.InsertarTrabajador(objTrabajador);
                    objUsuario.IdTrabajador = IdTrabajador;
                    int IdUsuario = objUsuarioDA.InsertarUsuario(objUsuario);

                    if (objTrabajador.IdTrabajadorJefe != null)
                    {
                        Trabajador objTrabajadorJefe = new TrabajadorDA().ObtenerTrabajador(objTrabajador.IdTrabajadorJefe.Value);
                    }

                    return IdUsuario;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ActualizarUsuario(Usuario objUsuario)
        {
            try
            {
                new UsuarioDA().ActualizarUsuario(objUsuario, false);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarGrupoOcupacionaldeTrabajador(int IdTrabajador)
        {
            try
            {
                Trabajador objTrabajador = new TrabajadorDA().ObtenerTrabajador(IdTrabajador);
                if (objTrabajador.IdGrupoOcupacional != null)
                {
                    List<Trabajador> lstTrabajador = new TrabajadorDA().ListarTrabajdoresporGrupoOcupacional(objTrabajador.IdGrupoOcupacional);
                    if (lstTrabajador.Count == 1)
                        new GrupoOcupacionalDA().EliminarGrupoOcupacional((int)objTrabajador.IdGrupoOcupacional);

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IQueryable<Trabajador> ListarTrabajadores()
        {

            try
            {
                return new TrabajadorDA().ListarTrabajadores();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Trabajador> ListarTrabajadoresPorGrupo(int IdGrupo)
        {

            try
            {
                return new TrabajadorDA().ListarTrabajadoresPorGrupo(IdGrupo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario ObtenerUsuario(int IdUsuario)
        {
            try
            {
                return new UsuarioDA().ObtenerUsuario(IdUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario ObtenerUsuario(String Username)
        {
            try
            {
                return new UsuarioDA().ObtenerUsuario(Username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Trabajador ObtenerTrabajadorJefeporNombre(string NombreJefe)
        {
            try
            {
                return new TrabajadorDA().ObtenerTrabajadorJefeporNombre(NombreJefe);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void EliminarUsuario(int IdUsuario)
        {

            try
            {

                UsuarioDA objUsuarioDA = new UsuarioDA();
                //PostItDA objPostitDA = new PostItDA();
                Usuario objUsuario = objUsuarioDA.ObtenerUsuario(IdUsuario);
                int? IdGrupoOcupacional = objUsuario.Trabajador.IdGrupoOcupacional;
                new SuscripcionNotificacionDA().EliminarSuscripcionNotificacionUsuario(objUsuario.IdUsuario);

                EliminarGrupoOcupacionaldeTrabajador(objUsuario.IdTrabajador.Value);

                EliminarTrabajador(objUsuario.IdTrabajador.Value);
                objUsuarioDA.EliminarUsuario(IdUsuario);
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
                return new TrabajadorDA().ObtenerTrabajador(IdTrabajador);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Trabajador> ListarSubordinados(int IdTrabajador)
        {
            try
            {
                return new TrabajadorDA().ListarSubordinados(IdTrabajador);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Trabajador ObtenerTrabajador(String DNI)
        {
            try
            {
                return new TrabajadorDA().ObtenerTrabajador(DNI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Trabajador ObtenerTrabajadorPorCodigo(String Codigo)
        {
            try
            {
                return new TrabajadorDA().ObtenerTrabajadorPorCodigo(Codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Trabajador ObtenerTrabajadorCabeza()
        {
            try
            {
                return new TrabajadorDA().ObtenerTrabajadorCabeza();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarTrabajador(int IdTrabajador)
        {
            try
            {
                Trabajador objTrabajador = new TrabajadorDA().ObtenerTrabajador(IdTrabajador);

                EliminarGrupoOcupacionaldeTrabajador(objTrabajador.IdTrabajador);

                new TrabajadorDA().EliminarTrabajador(IdTrabajador);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Usuario> BuscarUsuarios(string Codigo, string Nombre, string Puesto, string Area, string GrupoOcupacional, ObjectContainer Pages, int? Page, int? PageSize)
        {
            try
            {
                return new UsuarioDA().BuscarUsuarios(Codigo, Nombre, Puesto, Area, GrupoOcupacional, Pages, Page, PageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Trabajador> BuscarTrabajadorPorGrupo(string Codigo, string Nombre, string Puesto, string Area, string GrupoOcupacional, ObjectContainer Pages, int? Page, int? PageSize, int IdGrupo)
        {
            try
            {
                return new UsuarioDA().BuscarTrabajadorPorGrupo(Codigo, Nombre, Puesto, Area, GrupoOcupacional, Pages, Page, PageSize, IdGrupo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Usuario> BuscarUsuariosSinPaginacion(string Codigo, string Nombres, string Puesto, string Area, string GrupoOcupacional)
        {
            try
            {
                return new UsuarioDA().BuscarUsuariosSinPaginacion(Codigo, Nombres, Puesto, Area, GrupoOcupacional);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Usuario> BuscarUsuarios(string Codigo, string Nombre, string Puesto, string Area, int? IdTrabajadorJefe)
        {
            try
            {
                return new UsuarioDA().BuscarUsuarios(Codigo, Nombre, Puesto, Area, IdTrabajadorJefe);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarPassword(int IdUsuario, string Password)
        {
            try
            {
                new UsuarioDA().ActualizarPassword(IdUsuario, Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarFoto(Trabajador objTrabajador)
        {
            try
            {
                new TrabajadorDA().ActualizarFoto(objTrabajador);
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
                new TrabajadorDA().ActualizarEstado(IdTrabajador, Estado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarConReemplazoNuevo(Usuario objUsuario, Trabajador objTrabajador, int IdUsuarioEliminado, string GrupoOcupacionalNombre)
        {
            try
            {
                TrabajadorDA objTrabajadorDA = new TrabajadorDA();
                GuardarUsuario(objUsuario, objTrabajador, GrupoOcupacionalNombre, false);
                Usuario objUsuarioEliminado = new UsuarioDA().ObtenerUsuario(IdUsuarioEliminado);
                Usuario objUsuarioReemplazante = new UsuarioDA().ObtenerUsuario(objUsuario.IdUsuario);
                Trabajador objTrabajadorEliminado = objUsuarioEliminado.Trabajador;
                Trabajador objTrabajdorReemplazante = objUsuarioReemplazante.Trabajador;

                if (objTrabajadorEliminado.IdTrabajadorJefe != null)
                {
                    objTrabajdorReemplazante.IdTrabajadorJefe = objTrabajadorEliminado.IdTrabajadorJefe;
                    objTrabajadorDA.ActualizarTrabajador(objTrabajdorReemplazante);
                }

                EliminarGrupoOcupacionaldeTrabajador(objTrabajadorEliminado.IdTrabajador);

                EliminarUsuario(objUsuarioEliminado.IdUsuario);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void EnviarMailNuevoUsuario(String Email, String Nombre, String Username, String Password)
        {
            String Contenido = "<div style='text-align: center;'><img src='" + Path.Combine(ConfigurationManager.AppSettings["UrlSOOM"], "Images/Mail/logo_ciudad_positiva.png") + "' /><br /><br />";
            Contenido += "<h2>Estimado(a) " + Nombre + "</h2>";
            Contenido += "<h1 style='color: #00c3c5;'>&iexcl;Bienvenido(a)!</h1><br/>";
            Contenido += "<label>Hemos registrado tu cuenta en nuestra aula virtual.</label><br />";
            Contenido += "<label>Tus datos de acceso son:</label><br /><br />";
            Contenido += "<h3>Usuario: " + Username + "</h3>";
            Contenido += "<h3>Contrase&ntilde;a: " + Password + "</h3><br /><br />";
            Contenido += "<label>Si tienes alguna consulta comun&iacute;cate con el equipo de Formaci&oacute;n y Desarrollo.</label><br /><br />";
            Contenido += "<a style='display: inline-block;' href='" + ConfigurationManager.AppSettings["UrlSOOMClient"] + "'><img style='width: 250px;' src='" + ConfigurationManager.AppSettings["UrlSOOM"] + "Images/Mail/btn_plataforma.png' /></a></div>";

            new NotificacionBC().EnviarEmail("Nuevo usuario registrado", Contenido, new String[] { Email });
        }

        public void EliminarconReemplazoExistente(int IdUsuarioEliminado, int IdUsuarioReemplazante)
        {
            try
            {
                TrabajadorDA objTrabajadorDA = new TrabajadorDA();

                Usuario objUsuarioEliminado = new UsuarioDA().ObtenerUsuario(IdUsuarioEliminado);
                Usuario objUsuarioReemplazante = new UsuarioDA().ObtenerUsuario(IdUsuarioReemplazante);
                Trabajador objTrabajadorEliminado = objUsuarioEliminado.Trabajador;
                Trabajador objTrabajdorReemplazante = objUsuarioReemplazante.Trabajador;

                if (objTrabajadorEliminado.IdTrabajadorJefe != null)
                {
                    objTrabajdorReemplazante.IdTrabajadorJefe = objTrabajadorEliminado.IdTrabajadorJefe;
                    objTrabajadorDA.ActualizarTrabajador(objTrabajdorReemplazante);
                }

                RestablecerDependencias(objTrabajdorReemplazante);

                EliminarGrupoOcupacionaldeTrabajador(objTrabajadorEliminado.IdTrabajador);

                EliminarUsuario(objUsuarioEliminado.IdUsuario);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void EliminarSinReemplazo(int IdUsuarioEliminado)
        {
            try
            {
                TrabajadorDA objTrabajadorDA = new TrabajadorDA();
                Usuario objUsuarioEliminado = new UsuarioDA().ObtenerUsuario(IdUsuarioEliminado);
                Trabajador objTrabajadorEliminado = objUsuarioEliminado.Trabajador;

                EliminarUsuario(objUsuarioEliminado.IdUsuario);
                EliminarGrupoOcupacionaldeTrabajador(objTrabajadorEliminado.IdTrabajador);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void RestablecerDependencias(Trabajador objTrabajador)
        {
            try
            {
                Trabajador objTrabajadorJefe = new TrabajadorDA().ObtenerTrabajador(objTrabajador.IdTrabajadorJefe.Value);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<string> InsertarActualizar(DataTable dtUsuario)
        {
            return new UsuarioDA().InsertarActualizar(dtUsuario);
        }

        public Trabajador BuscarPorDNI(string dniUsuario)
        {
            try
            {
                return new UsuarioDA().BuscarPorDNI(dniUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Trabajador BuscarUsuarioPorCodigo(string Codigo)
        {
            try
            {
                return new UsuarioDA().BuscarUsuarioPorCodigo(Codigo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarTrabajador(Trabajador objTrabajador)
        {
            try
            {
                new TrabajadorDA().ActualizarTrabajador(objTrabajador);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Usuario BuscarUsuarioNombre(string nombreUsuario)
        {
            try
            {
                return new UsuarioDA().BuscarUsuarioNombre(nombreUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int? BuscarGrupoOcupacionalNombre(string nombreGrupoOcupacional)
        {
            try
            {
                return new UsuarioDA().BuscarGrupoOcupacionalNombre(nombreGrupoOcupacional);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int? BuscarTrabajadorPorUsuario(string nombreUsuario)
        {
            try
            {
                return new UsuarioDA().BuscarTrabajadorPorUsuario(nombreUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void InsertarGrupoOcupacional(GrupoOcupacional objGrupoOcupacional)
        {
            try
            {
                new UsuarioDA().InsertarGrupoOcupacional(objGrupoOcupacional);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AsignarGrupoATrabajador(int IdTrabajador, int[] IdGrupo)
        {
            try
            {
                new GrupoTrabajadorDA().AsignarGrupoATrabajador(IdTrabajador, IdGrupo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int RegistrarFechaHoraIngresoCurso(ActividadUsuario objActividadUsuario)
        {
            try
            {
                return new ActividadUsuarioDA().RegistrarFechaHoraIngresoCurso(objActividadUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int RegistrarVisita(ActividadUsuario objActividadUsuario)
        {
            try
            {
                return new ActividadUsuarioDA().RegistrarVisita(objActividadUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<ActividadUsuario> ListarActividadUsuarioCurso()
        {
            try
            {
                return new ActividadUsuarioDA().ListarActividadUsuarioCurso();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<IDictionary<string, object>> DescargarBaseTrabajadores()
        {
            try
            {
                TrabajadorDA objTrabajadorDA = new TrabajadorDA();
                return objTrabajadorDA.DescargarBaseTrabajadores();
            }
            catch (Exception)
            {
                throw;
            }

        }

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

        public void CargaMasivaUsuariosExcel(string fileExtension, string fileRuta)
        {
            try
            {
                //Conexion a Excel
                OleDbConnection miConexion = new OleDbConnection();
                //Excel 97-2003
                if (fileExtension == ".xls")
                    miConexion.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileRuta + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                //Excel 2010 en adelante
                if (fileExtension == ".xlsx")
                    miConexion.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileRuta + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                miConexion.Open();

                //Nombre de las hojas del Excel
                DataTable dtHoja = null;
                dtHoja = miConexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string hojaExcel = dtHoja.Rows[0]["TABLE_NAME"].ToString();

                currentProgress = 0;

                OleDbCommand cmdExcel = new OleDbCommand("select * from [" + hojaExcel + "]", miConexion);
                OleDbDataAdapter daExcel = new OleDbDataAdapter(cmdExcel);
                DataTable dtExcel = new DataTable();
                daExcel.Fill(dtExcel);

                //Cantidad de validaciones por las columnas
                totalProgress = dtExcel.Rows.Count + 1;

                UsuarioBC objModel = new UsuarioBC();

                mensajeError = string.Empty;

                Task.Run(() => EsperarCargaMasiva(cmdExcel, miConexion, fileRuta));

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void EsperarCargaMasiva(OleDbCommand cmdExcel, OleDbConnection miConexion, string fileRuta)
        {
            try
            {
                List<Usuario> lstUsuario = new List<Usuario>();
                List<Trabajador> lstTrabajador = new List<Trabajador>();
                List<string> lstCodigoTrabajadorJefe = new List<string>();
                List<string> lstGrupoOcupacional = new List<string>();

                OleDbDataReader objReaderExcel = cmdExcel.ExecuteReader();

                //Verficar si en el excel todos los campos tiene Código
                while (objReaderExcel.Read())
                {
                    currentProgress += 1;
                    var celdaCodigo = objReaderExcel[0].ToString().Trim();
                    var celdaDNI = objReaderExcel[1].ToString().Trim();
                    var celdaNombre = objReaderExcel[2].ToString().Trim();
                    var celdaPrimerApellido = objReaderExcel[3].ToString().Trim();
                    var celdaSegundoApellido = objReaderExcel[4].ToString().Trim();
                    var celdaEmail = objReaderExcel[8].ToString().Trim();
                    var celdaArea = objReaderExcel[16].ToString().Trim();
                    var celdaCategoria = objReaderExcel[19].ToString().Trim();
                    var celdaCompania = objReaderExcel[21].ToString().Trim();
                    var celdaUsuario = objReaderExcel[23].ToString().Trim();
                    
                    if (string.IsNullOrEmpty(celdaCodigo) || string.IsNullOrEmpty(celdaDNI) || string.IsNullOrEmpty(celdaNombre) ||
                        string.IsNullOrEmpty(celdaPrimerApellido) || string.IsNullOrEmpty(celdaSegundoApellido) || string.IsNullOrWhiteSpace(celdaEmail) &&
                        string.IsNullOrEmpty(celdaArea) || string.IsNullOrEmpty(celdaCategoria) ||
                        string.IsNullOrEmpty(celdaCompania) || string.IsNullOrEmpty(celdaUsuario))
                    {
                        miConexion.Close();
                        new FileInfo(fileRuta).Delete();
                        currentProgress = -1;
                        return;
                    }

                    Usuario objUsuario = new Usuario();
                    Trabajador objTrabajador = new Trabajador();

                    objTrabajador.Codigo = celdaCodigo;

                    objTrabajador.DNI = celdaDNI;

                    objTrabajador.Nombres = celdaNombre;

                    objTrabajador.PrimerApellido = celdaPrimerApellido;

                    objTrabajador.SegundoApellido = celdaSegundoApellido;

                    try
                    {
                        var fechaNacimiento = objReaderExcel[6].ToString();
                        objTrabajador.FechaNacimiento = string.IsNullOrEmpty(fechaNacimiento) ? (DateTime?)null : DateTime.Parse(fechaNacimiento);
                    }
                    catch (Exception ex)
                    {
                        mensajeError = "Las fechas de nacimiento deben tener el formato correcto: día/mes/año.";
                        miConexion.Close();
                        new FileInfo(fileRuta).Delete();
                        return;
                    }

                    var genero = objReaderExcel[7].ToString().Trim();
                    if (genero == "Hombre" || genero == "HOMBRE" || genero == "hombre")
                        objTrabajador.Genero = Constants.Trabajador.Genero.MASCULINO;
                    else if (genero == "Mujer" || genero == "MUJER" || genero == "mujer")
                        objTrabajador.Genero = Constants.Trabajador.Genero.FEMENINO;
                    else
                        objTrabajador.Genero = Constants.Trabajador.Genero.OTROS;

                    var Email = objReaderExcel[8].ToString().Trim();
                    if (!StringHelper.ComprobarFormatoEmail(Email))
                    {
                        mensajeError = "Los Correos electrónicos deben tener el formato correcto: ejemplo@gmail.com";
                        miConexion.Close();
                        new FileInfo(fileRuta).Delete();
                        return;
                    }
                    else
                        objTrabajador.Email = Email;

                    var celular = objReaderExcel[9].ToString().Trim();
                    objTrabajador.Celular = (string.IsNullOrWhiteSpace(celular) ? null : celular);

                    var sede = objReaderExcel[10].ToString().Trim();
                    objTrabajador.Sede = (string.IsNullOrWhiteSpace(sede) ? null : sede);

                    var provincia = objReaderExcel[11].ToString().Trim();
                    objTrabajador.Provincia = (string.IsNullOrWhiteSpace(provincia) ? null : provincia);

                    var region = objReaderExcel[12].ToString().Trim();
                    objTrabajador.Region = (string.IsNullOrWhiteSpace(region) ? null : region);

                    var pais = objReaderExcel[13].ToString().Trim();
                    objTrabajador.Pais = (string.IsNullOrWhiteSpace(pais) ? null : pais);

                    try
                    {
                        objTrabajador.FechaIngreso = string.IsNullOrEmpty(objReaderExcel[14].ToString().Trim()) ? (DateTime?)null : DateTime.Parse(objReaderExcel[14].ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        mensajeError = "Las fechas de ingreso deben tener el formato correcto: día/mes/año.";
                        miConexion.Close();
                        new FileInfo(fileRuta).Delete();
                        return;
                    }

                    var gerencia = objReaderExcel[15].ToString().Trim();
                    objTrabajador.Gerencia = (string.IsNullOrWhiteSpace(gerencia) ? null : gerencia);

                    var area = objReaderExcel[16].ToString().Trim();
                    objTrabajador.Area = area;

                    var rol = objReaderExcel[20].ToString().Trim();
                    objTrabajador.Rol = rol;

                    var compania = objReaderExcel[21].ToString().Trim();
                    objTrabajador.Compañia = compania;

                    var puesto = objReaderExcel[22].ToString().Trim();
                    objTrabajador.Puesto = (string.IsNullOrWhiteSpace(puesto) ? null : puesto);

                    objUsuario.Username = objReaderExcel[23].ToString().Trim();

                    var Password = objReaderExcel[24].ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(Password))
                    {
                        objUsuario.Password = Encryptor.SHA256Hash(Password);
                    }

                    var inactivo = objReaderExcel[25].ToString().Trim();
                    if (inactivo == "X" || inactivo == "x")
                        objTrabajador.Estado = Constants.Trabajador.Estado.INACTIVO;
                    else if (string.IsNullOrWhiteSpace(inactivo))
                        objTrabajador.Estado = Constants.Trabajador.Estado.ACTIVO;

                    var eliminar = objReaderExcel[26].ToString().Trim();
                    if (eliminar == "X" || eliminar == "x")
                    {
                        objTrabajador.Eliminado = true;
                        objUsuario.Eliminado = true;
                    }
                    else if (string.IsNullOrWhiteSpace(eliminar))
                    {
                        objTrabajador.Eliminado = false;
                        objUsuario.Eliminado = false;
                    }

                    String codigoTrabajadorJefe = (string.IsNullOrEmpty(objReaderExcel[17].ToString().Trim()) ? null : objReaderExcel[17].ToString().Trim());
                    String categorias = (string.IsNullOrEmpty(objReaderExcel[19].ToString().Trim()) ? null : objReaderExcel[19].ToString().Trim().ToUpper());
                    
                    lstTrabajador.Add(objTrabajador);
                    lstUsuario.Add(objUsuario);
                    lstCodigoTrabajadorJefe.Add(codigoTrabajadorJefe);
                    lstGrupoOcupacional.Add(categorias);
                }
                objReaderExcel.Dispose();

                string outMensajeError;
                new CargaMasivaDA().GuardarUsuarioCargaMasiva(lstUsuario, lstTrabajador, lstCodigoTrabajadorJefe, lstGrupoOcupacional, out outMensajeError);

                mensajeError = outMensajeError;

                miConexion.Close();
                new FileInfo(fileRuta).Delete();

                currentProgress += 1;
            }
            catch (Exception ex)
            {
                mensajeError = "Verifique que el archivo Excel que ha seleccionado sea el correcto.";
            }
        }

        public void AsignacionMasivaUsuarios(string fileExtension, string fileRuta, int IdGrupo)
        {
            try
            {
                //Conexion a Excel
                OleDbConnection miConexion = new OleDbConnection();
                //Excel 97-2003
                if (fileExtension == ".xls")
                    miConexion.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileRuta + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                //Excel 2010 en adelante
                if (fileExtension == ".xlsx")
                    miConexion.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileRuta + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                miConexion.Open();

                //Nombre de las hojas del Excel
                DataTable dtHoja = null;
                dtHoja = miConexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string hojaExcel = dtHoja.Rows[0]["TABLE_NAME"].ToString();

                currentProgress = 0;

                OleDbCommand cmdExcel = new OleDbCommand("select * from [" + hojaExcel + "]", miConexion);
                OleDbDataAdapter daExcel = new OleDbDataAdapter(cmdExcel);
                DataTable dtExcel = new DataTable();
                daExcel.Fill(dtExcel);

                //Cantidad de validaciones por las columnas
                totalProgress = dtExcel.Rows.Count + 1;

                UsuarioBC objModel = new UsuarioBC();

                mensajeError = string.Empty;

                Task.Run(() => EsperarAsignacionMasiva(cmdExcel, miConexion, fileRuta, IdGrupo));

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void EsperarAsignacionMasiva(OleDbCommand cmdExcel, OleDbConnection miConexion, string fileRuta, int IdGrupo)
        {
            try
            {
                List<string> lstCodigo = new List<string>();

                OleDbDataReader objReaderExcel = cmdExcel.ExecuteReader();

                //Verficar si en el excel todos los campos tiene Código
                while (objReaderExcel.Read())
                {
                    currentProgress += 1;
                    var celdaCodigo = objReaderExcel[0].ToString().Trim();

                    if (string.IsNullOrEmpty(celdaCodigo))
                    {
                        miConexion.Close();
                        new FileInfo(fileRuta).Delete();
                        currentProgress = -1;
                        return;
                    }

                    String codigos = celdaCodigo;

                    lstCodigo.Add(codigos);
                }
                objReaderExcel.Dispose();

                string outMensajeError;
                new CargaMasivaDA().GuardarAsignacionMasiva(lstCodigo, IdGrupo, out outMensajeError);

                mensajeError = outMensajeError;

                miConexion.Close();
                new FileInfo(fileRuta).Delete();

                currentProgress += 1;
            }
            catch (Exception ex)
            {
                mensajeError = "Verifique que el archivo Excel que ha seleccionado sea el correcto.";
            }
        }

        public string GetError()
        {
            try
            {
                return mensajeError;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
