using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class UsuarioDA
    {
        public IQueryable<Usuario> ListarUsuario()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Usuario.Where(u => u.Eliminado != true);
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
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Usuario.Where(u => u.Eliminado != true && u.Trabajador.Eliminado != true && u.Username != "masteradministrator").ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Usuario> ListarUsuarios(int Idusuario)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Usuario.Where(u => u.Eliminado != true && u.IdUsuario == Idusuario).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int InsertarUsuario(Usuario objUsuario)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.Usuario.Add(objUsuario);
                objModel.SaveChanges();
                return objUsuario.IdUsuario;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Usuario ObtenerUsuario(int IdUsuario)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Usuario.SingleOrDefault(t => t.IdUsuario == IdUsuario && t.Eliminado != true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public Usuario ObtenerUsuario(String Username)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Usuario.FirstOrDefault(t => t.Username == Username && t.Eliminado != true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ActualizarUsuario(Usuario objUsuario, bool CargaMasiva)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                var objUsuarioBd = objModel.Usuario.SingleOrDefault(t => t.IdUsuario == objUsuario.IdUsuario);
                objUsuarioBd.IdTrabajador = objUsuario.IdTrabajador;
                objUsuarioBd.Username = objUsuario.Username;
                if (objUsuario.Password != null)
                {
                    objUsuarioBd.Password = objUsuario.Password;
                    objUsuarioBd.PasswordAntiguo = objUsuario.PasswordAntiguo;

                    if (!CargaMasiva)
                        objUsuarioBd.UltimaActualizacion = DateTimeHelper.PeruDateTime;
                }
                objUsuarioBd.Eliminado = objUsuario.Eliminado;
                objUsuarioBd.Perfil = objUsuario.Perfil;
                objModel.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EliminarUsuario(int IdUsuario)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                var objUsuariobd = objModel.Usuario.SingleOrDefault(t => t.IdUsuario == IdUsuario);
                objUsuariobd.Eliminado = true;
                objUsuariobd.SuscripcionNotificacion.Clear();

                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Usuario Login(string Username, string Password)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Usuario objUsuario = (from u in objModel.Usuario
                                      join t in objModel.Trabajador on u.IdTrabajador equals t.IdTrabajador into ut
                                      from t in ut.DefaultIfEmpty()
                                      where u.Username == Username && u.Password == Password &&
                                      u.Eliminado != true && (u.IdTrabajador == null || t.Estado == Constants.Trabajador.Estado.ACTIVO)
                                      select u).SingleOrDefault();

                return objUsuario;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Usuario EncontrarUsuario(string UsernameoCorreo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Usuario objUsuario = (from u in objModel.Usuario
                                      join t in objModel.Trabajador on u.IdTrabajador equals t.IdTrabajador into ut
                                      from t in ut.DefaultIfEmpty()
                                      where u.Username == UsernameoCorreo || t.Email == UsernameoCorreo &&
                                      u.Eliminado != true && t.Eliminado != true && (u.IdTrabajador == null || t.Estado == Constants.Trabajador.Estado.ACTIVO)
                                      select u).FirstOrDefault();

                return objUsuario;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Usuario> BuscarUsuarios(string Codigo, string Nombres, string Puesto, string Area, int? IdTrabajadorJefe)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();

                List<string> Search = null;

                if (Nombres != null)
                    Search = Nombres.Split(' ').ToList();
                else
                    Search = new List<string>();

                IQueryable<Usuario> lstUsuario = (from p in objModel.Usuario
                                                  join t in objModel.Trabajador on p.IdTrabajador equals t.IdTrabajador
                                                  where (Nombres == null || Nombres == "" || Search.All(s => t.Nombres.Contains(s) || t.PrimerApellido.Contains(s) || t.SegundoApellido.Contains(s))) &&
                                                  (Codigo == null || Codigo == "" || t.Codigo.Contains(Codigo)) &&
                                                  (Puesto == null || Puesto == "" || t.Puesto.Contains(Puesto)) &&
                                                  (Area == null || Area == "" || t.Area.Contains(Area)) &&
                                                  (IdTrabajadorJefe == null || t.IdTrabajadorJefe == IdTrabajadorJefe) &&
                                                  p.Eliminado != true
                                                  select p);
                return lstUsuario.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<Trabajador> ListarPosibleJefe()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.Where(e => e.Estado != Constants.Trabajador.Estado.INACTIVO && e.Eliminado != true).OrderBy(e => e.Nombres).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<string> ListarAreaUsuario()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Trabajador.Where(x => x.Eliminado != true).Select(t => t.Area).Distinct().ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<string> ListarGrupoOcupacionalUsuario()
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoOcupacional.Select(t => t.Nombre).Distinct().ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Usuario> BuscarUsuarios(string Codigo, string Nombres, string Puesto, string Area, string GrupoOcupacional, ObjectContainer Pages, int? Page, int? PageSize)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();

                List<string> Search = null;

                if (Nombres != null)
                    Search = Nombres.Split(' ').ToList();
                else
                    Search = new List<string>();

                IQueryable<Usuario> lstUsuario = (from p in objModel.Usuario
                                                  join t in objModel.Trabajador on p.IdTrabajador equals t.IdTrabajador
                                                  join g in objModel.GrupoOcupacional on t.IdGrupoOcupacional equals g.IdGrupoOcupacional into ptg
                                                  from g in ptg.DefaultIfEmpty()
                                                  where (Nombres == null || Search.All(s => t.Nombres.Contains(s) || t.PrimerApellido.Contains(s) || t.SegundoApellido.Contains(s))) &&
                                                  ((p.IdTrabajador == null && Codigo == null && Nombres == null && Puesto == null && Area == null && GrupoOcupacional == null) || (
                                                  (t.Codigo.Contains(Codigo) || Codigo == null) &&
                                                  (t.Puesto.Contains(Puesto) || Puesto == null) &&
                                                  (t.Area.Contains(Area) || Area == null) &&
                                                  (g.Nombre.Contains(GrupoOcupacional) || GrupoOcupacional == null))) &&
                                                  p.Eliminado != true
                                                  select p);


                List<Usuario> lstUsuarioPage = null;
                if (Pages != null && Page != null && PageSize != null)
                {
                    Pages.Value = (int)Math.Ceiling(lstUsuario.Count() / (double)PageSize.Value);
                    lstUsuarioPage = lstUsuario.OrderBy(u => u.IdUsuario).Skip((Page.Value - 1) * PageSize.Value).Take(PageSize.Value).ToList();
                }
                else
                {
                    Pages.Value = 1;
                    lstUsuarioPage = lstUsuario.ToList();
                }

                return lstUsuarioPage;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Trabajador> BuscarTrabajadorPorGrupo(string Codigo, string Nombres, string Puesto, string Area, string GrupoOcupacional, ObjectContainer Pages, int? Page, int? PageSize, int IdGrupo)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();

                List<string> Search = null;

                if (Nombres != null)
                    Search = Nombres.Split(' ').ToList();
                else
                    Search = new List<string>();

                List<Trabajador> lstTrabajadorGrupo = objModel.GrupoTrabajador.Where(gr => gr.IdGrupo == IdGrupo && gr.FechaHoraDesasignacion == null).Select(t=>t.Trabajador).ToList();
                IEnumerable<Trabajador> lstTrabajador = (from t in lstTrabajadorGrupo
                                                  join g in objModel.GrupoOcupacional on t.IdGrupoOcupacional equals g.IdGrupoOcupacional into ptg
                                                  from g in ptg.DefaultIfEmpty()
                                                       where (Nombres == null || Search.All(s => t.Nombres.ToUpper().Contains(s.ToUpper()) || t.PrimerApellido.ToUpper().Contains(s.ToUpper()) || t.SegundoApellido.ToUpper().Contains(s.ToUpper()))) &&
                                                  ((Codigo == null && Nombres == null && Puesto == null && Area == null && GrupoOcupacional == null) || (
                                                  (Codigo == null || t.Codigo.Contains(Codigo)) &&
                                                  (Puesto == null || (t.Puesto != null && t.Puesto.Contains(Puesto.ToUpper()))) &&
                                                  (Area == null || t.Area.Contains(Area.ToUpper())) &&
                                                  (GrupoOcupacional == null || g.Nombre.Contains(GrupoOcupacional.ToUpper())))) &&
                                                  t.Eliminado != true
                                                   select t);


                List<Trabajador> lstTrabajadorPage = null;
                if (Pages != null && Page != null && PageSize != null)
                {
                    int TotalTrabajadores = lstTrabajador.Count();
                    Pages.Value = (int)Math.Ceiling(TotalTrabajadores / (double)PageSize.Value);
                    lstTrabajadorPage = lstTrabajador.OrderBy(t => t.IdTrabajador).Skip((Page.Value - 1) * PageSize.Value).Take(PageSize.Value).ToList();
                }
                else
                {
                    Pages.Value = 1;
                    lstTrabajadorPage = lstTrabajador.ToList();
                }

                return lstTrabajadorPage;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ActualizarPassword(int IdUsuario, string Password)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Usuario objUsuarioBd = objModel.Usuario.SingleOrDefault(t => t.IdUsuario == IdUsuario);
                objUsuarioBd.Password = Password;
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void EliminarMoviendoUsuario(int IdTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Trabajador objTrabajador = objModel.Trabajador.SingleOrDefault(t => t.IdTrabajador == IdTrabajador);



            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<string> InsertarActualizar(DataTable dtUsuario)
        {
            try
            {
                List<string> sadas = new List<string>();
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                List<string> lstDni = dtUsuario.AsEnumerable().ToList().ConvertAll(x => x.ItemArray[1].ToString()).ToList();
                sadas = lstDni;
                return sadas;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Trabajador BuscarPorDNI(string dniUsuario)
        {
            Trabajador UsuarioTrabajador = new Trabajador();
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                UsuarioTrabajador = objModel.Trabajador.FirstOrDefault(t => t.DNI == dniUsuario && t.Eliminado != true);
                return UsuarioTrabajador;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Trabajador BuscarUsuarioPorCodigo(string Codigo)
        {
            Trabajador UsuarioTrabajador = new Trabajador();
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                UsuarioTrabajador = objModel.Trabajador.Where(x => x.Eliminado != true && x.Codigo == Codigo).FirstOrDefault(t => t.Codigo == Codigo && t.Eliminado != true);
                return UsuarioTrabajador;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ActualizarUsuarioPorCargaMasiva(Usuario objUsuario, int idTrabajador)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Usuario objUsuarioBd = objModel.Usuario.SingleOrDefault(t => t.IdTrabajador == idTrabajador);

                objModel.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usuario BuscarUsuarioNombre(string nombreUsuario)
        {
            Usuario objUsuario = new Usuario();
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objUsuario = objModel.Usuario.Where(x => x.Username == nombreUsuario).FirstOrDefault();
                return objUsuario;
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
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.GrupoOcupacional.Where(z => z.Nombre == nombreGrupoOcupacional).Select(z => z.IdGrupoOcupacional).FirstOrDefault();
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
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                return objModel.Usuario.Where(z => z.Username == nombreUsuario).Select(z => z.IdTrabajador).FirstOrDefault();
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
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                objModel.GrupoOcupacional.Add(objGrupoOcupacional);
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IQueryable<Usuario> BuscarUsuariosSinPaginacion(string Codigo, string Nombres, string Puesto, string Area, string GrupoOcupacional)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();

                List<string> Search = null;

                if (Nombres != null)
                    Search = Nombres.Split(' ').ToList();
                else
                    Search = new List<string>();

                IQueryable<Usuario> lstUsuario = (from p in objModel.Usuario
                                                  join t in objModel.Trabajador on p.IdTrabajador equals t.IdTrabajador
                                                  join g in objModel.GrupoOcupacional on t.IdGrupoOcupacional equals g.IdGrupoOcupacional into ptg
                                                  from g in ptg.DefaultIfEmpty()
                                                  where (Nombres == null || Search.All(s => t.Nombres.Contains(s) || t.PrimerApellido.Contains(s) || t.SegundoApellido.Contains(s))) &&
                                                  ((p.IdTrabajador == null && Codigo == null && Nombres == null && Puesto == null && Area == null && GrupoOcupacional == null) || (
                                                  (t.Codigo.Contains(Codigo) || Codigo == null) &&
                                                  (t.Puesto.Contains(Puesto) || Puesto == null) &&
                                                  (t.Area.Contains(Area) || Area == null) &&
                                                  (g.Nombre.Contains(GrupoOcupacional) || GrupoOcupacional == null))) &&
                                                  p.Eliminado != true
                                                  select p);
                return lstUsuario;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
