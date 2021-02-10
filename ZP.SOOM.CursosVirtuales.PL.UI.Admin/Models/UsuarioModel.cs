using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Newtonsoft.Json.Linq;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public int IdTrabajador { get; set; }
        public int? IdGrupoOcupacional { get; set; }
        public string Foto { get; set; }
        public string Nombres { get; set; }
        public string DNI { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Email { get; set; }
        public string Puesto { get; set; }
        public string Jefe { get; set; }
        public int? IdJefe { get; set; }
        public string FotoJefe { get; set; }
        public string Area { get; set; }
        public string AreaSuperior { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Estado { get; set; }
        public string Perfil { get; set; }
        public bool Eliminado { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaHoraLogin { get; set; }
        public bool Eliminable { get; set; }
        public bool TieneSubordinados { get; set; }
        public bool TieneJefe { get; set; }
        public int? IdCoach { get; set; }
        public List<UsuarioModel> Subordinados { get; set; }
        public int Superiores { get; set; }
        public int Subordinado { get; set; }
        public int Par { get; set; }
        public string FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Celular { get; set; }
        public string Sede { get; set; }
        public string Provincia { get; set; }
        public string Region { get; set; }
        public string Pais { get; set; }
        public string FechaIngreso { get; set; }
        public string GrupoOcupacional { get; set; }
        public string TipoContrato { get; set; }
        public int Nivel { get; set; }
        public string Rol { get; set; }
        public string Compañia { get; set; }
        public string Etiqueta { get; set; }
        public int CantidadSubordinados { get; set; }
        public string Gerencia { get; set; }
        public bool TieneObjetivosPendientes { get; set; }
        public HttpPostedFileBase UploadImage { get; set; }
        public bool EvaluacionObjetivo { get; set; }
        public bool EvaluacionCompetencias { get; set; }

        public static UsuarioModel FromUsuario(Usuario objUsuario)
        {
            try
            {
                UsuarioModel objUsuarioModel = new UsuarioModel();
                objUsuarioModel.IdUsuario = objUsuario.IdUsuario;
                objUsuarioModel.Username = objUsuario.Username;
                objUsuarioModel.Password = objUsuario.Password;
                objUsuarioModel.Perfil = objUsuario.Perfil;

                if (objUsuario.Trabajador != null)
                {
                    objUsuarioModel.IdTrabajador = objUsuario.IdTrabajador.Value;
                    objUsuarioModel.Nombres = objUsuario.Trabajador.Nombres;
                    objUsuarioModel.PrimerApellido = objUsuario.Trabajador.PrimerApellido;
                    objUsuarioModel.SegundoApellido = objUsuario.Trabajador.SegundoApellido;
                    objUsuarioModel.DNI = objUsuario.Trabajador.DNI;
                    objUsuarioModel.Email = objUsuario.Trabajador.Email;
                    objUsuarioModel.Puesto = objUsuario.Trabajador.Puesto;
                    objUsuarioModel.Area = objUsuario.Trabajador.Area;
                    objUsuarioModel.Estado = objUsuario.Trabajador.Estado;
                    objUsuarioModel.Codigo = objUsuario.Trabajador.Codigo;
                    objUsuarioModel.Foto = objUsuario.Trabajador.Foto;
                    objUsuarioModel.Eliminable = objUsuario.Trabajador.Subordinado.Where(t => t.Eliminado != true).Count() == 0;
                    objUsuarioModel.CantidadSubordinados = objUsuario.Trabajador.Subordinado.Where(t => t.Eliminado != true).Count();
                    if (objUsuario.Trabajador.IdGrupoOcupacional != null)
                        objUsuarioModel.IdGrupoOcupacional = objUsuario.Trabajador.IdGrupoOcupacional;
                    if (objUsuario.Trabajador.GrupoOcupacional != null)
                        objUsuarioModel.GrupoOcupacional = objUsuario.Trabajador.GrupoOcupacional.Nombre;

                    if (objUsuario.Trabajador.FechaNacimiento != null)
                        objUsuarioModel.FechaNacimiento = objUsuario.Trabajador.FechaNacimiento.Value.ToString("dd/MM/yyyy");

                    objUsuarioModel.Gerencia = objUsuario.Trabajador.Gerencia;
                    objUsuarioModel.Genero = objUsuario.Trabajador.Genero;
                    objUsuarioModel.Celular = objUsuario.Trabajador.Celular;
                    objUsuarioModel.Sede = objUsuario.Trabajador.Sede;
                    objUsuarioModel.Provincia = objUsuario.Trabajador.Provincia;
                    objUsuarioModel.Region = objUsuario.Trabajador.Region;
                    objUsuarioModel.Pais = objUsuario.Trabajador.Pais;

                    if (objUsuario.Trabajador.FechaIngreso != null)
                        objUsuarioModel.FechaIngreso = objUsuario.Trabajador.FechaIngreso.Value.ToString("dd/MM/yyyy");

                    objUsuarioModel.TipoContrato = objUsuario.Trabajador.TipoContrato;
                    if (objUsuario.Trabajador.Nivel != null)
                        objUsuarioModel.Nivel = objUsuario.Trabajador.Nivel.Value;
                    objUsuarioModel.Rol = objUsuario.Trabajador.Rol;
                    objUsuarioModel.Compañia = objUsuario.Trabajador.Compañia;


                    if (objUsuario.Trabajador.Jefe != null)
                    {
                        objUsuarioModel.Jefe = objUsuario.Trabajador.Jefe.Nombres + " " + objUsuario.Trabajador.Jefe.PrimerApellido + " " + objUsuario.Trabajador.Jefe.SegundoApellido;
                        objUsuarioModel.IdJefe = objUsuario.Trabajador.IdTrabajadorJefe.Value;
                        objUsuarioModel.AreaSuperior = objUsuario.Trabajador.Jefe.Area;
                        objUsuarioModel.FotoJefe = objUsuario.Trabajador.Jefe.Foto;
                    }
                    objUsuarioModel.TieneSubordinados = objUsuario.Trabajador.Subordinado.Where(t => t.Eliminado != true).Count() > 0;

                }
                else
                {
                    objUsuarioModel.Nombres = "Master";
                    objUsuarioModel.PrimerApellido = "Administrator";
                    objUsuarioModel.SegundoApellido = "";
                    objUsuarioModel.DNI = "";
                    objUsuarioModel.Email = "";
                    objUsuarioModel.Puesto = "";
                    objUsuarioModel.Area = "";
                    objUsuarioModel.Estado = "A";
                }

                return objUsuarioModel;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UsuarioModel FromTrabajador(Trabajador objTrabajador, bool Subordinados, bool Objetivos)
        {
            try
            {
                UsuarioModel objUsuarioModel = new UsuarioModel();

                objUsuarioModel.IdUsuario = objTrabajador.Usuario.FirstOrDefault().IdUsuario;
                objUsuarioModel.IdTrabajador = objTrabajador.IdTrabajador;
                objUsuarioModel.Nombres = objTrabajador.Nombres;
                objUsuarioModel.PrimerApellido = objTrabajador.PrimerApellido;
                objUsuarioModel.SegundoApellido = objTrabajador.SegundoApellido;
                objUsuarioModel.DNI = objTrabajador.DNI;
                objUsuarioModel.Email = objTrabajador.Email;
                objUsuarioModel.Puesto = objTrabajador.Puesto;
                objUsuarioModel.Area = objTrabajador.Area;

                if (objTrabajador.IdGrupoOcupacional != null)
                    objUsuarioModel.IdGrupoOcupacional = objTrabajador.IdGrupoOcupacional;
                objUsuarioModel.Estado = objTrabajador.Estado;
                objUsuarioModel.Codigo = objTrabajador.Codigo;
                objUsuarioModel.Foto = objTrabajador.Foto;

                if (objTrabajador.FechaNacimiento != null)
                    objUsuarioModel.FechaNacimiento = objTrabajador.FechaNacimiento.Value.ToString("dd/MM/yyyy");

                objUsuarioModel.Genero = objTrabajador.Genero;
                objUsuarioModel.Celular = objTrabajador.Celular;
                objUsuarioModel.Sede = objTrabajador.Sede;
                objUsuarioModel.Provincia = objTrabajador.Provincia;
                objUsuarioModel.Region = objTrabajador.Region;
                objUsuarioModel.Pais = objTrabajador.Pais;

                if (objTrabajador.FechaIngreso != null)
                    objUsuarioModel.FechaIngreso = objTrabajador.FechaIngreso.Value.ToString("dd/MM/yyyy");

                if (objTrabajador.GrupoOcupacional != null)
                    objUsuarioModel.GrupoOcupacional = objTrabajador.GrupoOcupacional.Nombre;
                objUsuarioModel.TipoContrato = objTrabajador.TipoContrato;
                if (objTrabajador.Nivel != null)
                    objUsuarioModel.Nivel = objTrabajador.Nivel.Value;
                objUsuarioModel.Rol = objTrabajador.Rol;
                objUsuarioModel.Compañia = objTrabajador.Compañia;

                if (objTrabajador.Jefe != null)
                {
                    objUsuarioModel.Jefe = objTrabajador.Jefe.Nombres + " " + objTrabajador.Jefe.PrimerApellido + " " + objTrabajador.Jefe.SegundoApellido;
                    objUsuarioModel.IdJefe = objTrabajador.IdTrabajadorJefe.Value;
                    objUsuarioModel.AreaSuperior = objTrabajador.Jefe.Area;
                    objUsuarioModel.FotoJefe = objTrabajador.Jefe.Foto;
                }
                objUsuarioModel.TieneSubordinados = objTrabajador.Subordinado.Where(t => t.Eliminado != true).Count() > 0;

                return objUsuarioModel;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario ToUsuario()
        {
            try
            {
                Usuario objUsuario = new Usuario();
                objUsuario.IdUsuario = IdUsuario;
                objUsuario.IdTrabajador = IdTrabajador;
                objUsuario.Username = Username;
                if (!String.IsNullOrEmpty(Password))
                    objUsuario.Password = Encryptor.SHA256Hash(Password);
                objUsuario.Perfil = Perfil;
                objUsuario.Eliminado = Eliminado;

                return objUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Trabajador ToTrabajador()
        {
            try
            {
                Trabajador objTrabajador = new Trabajador();
                if (IdJefe != 0)
                    objTrabajador.IdTrabajadorJefe = IdJefe;
                objTrabajador.IdTrabajador = IdTrabajador;
                objTrabajador.Nombres = Nombres;
                objTrabajador.PrimerApellido = PrimerApellido;
                objTrabajador.SegundoApellido = SegundoApellido;
                objTrabajador.DNI = DNI;
                objTrabajador.Email = Email;
                objTrabajador.Puesto = Puesto;
                objTrabajador.Area = Area;
                if (FechaNacimiento != null)
                    objTrabajador.FechaNacimiento = DateTime.ParseExact(FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                objTrabajador.Genero = Genero;
                objTrabajador.Celular = Celular;
                objTrabajador.Sede = Sede;
                objTrabajador.Genero = Genero;
                objTrabajador.Provincia = Provincia;
                objTrabajador.Region = Region;
                objTrabajador.Pais = Pais;
                if (FechaIngreso != null)
                    objTrabajador.FechaIngreso = DateTime.ParseExact(FechaIngreso, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (IdGrupoOcupacional != null)
                    objTrabajador.IdGrupoOcupacional = (int)IdGrupoOcupacional;
                objTrabajador.TipoContrato = TipoContrato;
                if (Estado != null)
                    objTrabajador.Estado = Constants.Trabajador.Estado.ACTIVO;
                else
                    objTrabajador.Estado = Constants.Trabajador.Estado.INACTIVO;

                objTrabajador.Codigo = Codigo;
                objTrabajador.Foto = Foto;
                objTrabajador.Nivel = (int)Nivel;
                objTrabajador.Gerencia = Gerencia;
                objTrabajador.AreaSuperior = AreaSuperior;
                objTrabajador.Rol = Rol;
                objTrabajador.Compañia = Compañia;

                return objTrabajador;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override String ToString()
        {
            JObject jUsuario = new JObject();
            jUsuario["IdUsuario"] = IdUsuario;
            jUsuario["IdTrabajador"] = IdTrabajador;
            jUsuario["IdJefe"] = IdJefe;
            jUsuario["Username"] = Username;
            jUsuario["Nombres"] = Nombres;
            jUsuario["PrimerApellido"] = PrimerApellido;
            jUsuario["SegundoApellido"] = SegundoApellido;
            jUsuario["Perfil"] = Perfil;
            jUsuario["FechaHoraLogin"] = DateTimeHelper.PeruDateTime.ToString("dd/MM/yyyy HH:mm:ss");
            jUsuario["TieneSubordinados"] = TieneSubordinados;
            jUsuario["Foto"] = Foto;

            return jUsuario.ToString();
        }

        public static UsuarioModel FromString(String sUsuario)
        {
            JObject jUsuario = JObject.Parse(sUsuario);
            UsuarioModel objUsuarioModel = new UsuarioModel();
            objUsuarioModel.IdUsuario = (int)jUsuario["IdUsuario"];
            objUsuarioModel.IdTrabajador = (int)jUsuario["IdTrabajador"];
            objUsuarioModel.IdJefe = (int?)jUsuario["IdJefe"];
            objUsuarioModel.Username = (string)jUsuario["Username"];
            objUsuarioModel.Nombres = (string)jUsuario["Nombres"];
            objUsuarioModel.PrimerApellido = (string)jUsuario["PrimerApellido"];
            objUsuarioModel.SegundoApellido = (string)jUsuario["SegundoApellido"];
            objUsuarioModel.Perfil = (string)jUsuario["Perfil"];
            objUsuarioModel.TieneSubordinados = (bool)jUsuario["TieneSubordinados"];
            objUsuarioModel.Foto = (string)jUsuario["Foto"];
            objUsuarioModel.FechaHoraLogin = DateTime.ParseExact((String)jUsuario["FechaHoraLogin"], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            return objUsuarioModel;
        }
    }
}