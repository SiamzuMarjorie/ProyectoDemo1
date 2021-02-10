using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Models
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
        public string FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Celular { get; set; }
        public string Sede { get; set; }
        public string Provincia { get; set; }
        public string Region { get; set; }
        public string Pais { get; set; }
        public string FechaIngreso { get; set; }
        public string GrupoOcupacional { get; set; }
        public string Direccion { get; set; }
        public string Marca { get; set; }
        public string TipoContrato { get; set; }
        public int? Nivel { get; set; }

        public HttpPostedFileBase UploadImage { get; set; }

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
                    objUsuarioModel.Email = objUsuario.Trabajador.Email;
                    objUsuarioModel.Puesto = objUsuario.Trabajador.Puesto;
                    objUsuarioModel.Area = objUsuario.Trabajador.Area;
                    objUsuarioModel.Estado = objUsuario.Trabajador.Estado;
                    objUsuarioModel.Foto = objUsuario.Trabajador.Foto;
                    objUsuarioModel.Eliminable = objUsuario.Trabajador.Subordinado.Where(t => t.Eliminado != true).Count() == 0;
                    if (objUsuario.Trabajador.IdGrupoOcupacional != null)
                        objUsuarioModel.IdGrupoOcupacional = objUsuario.Trabajador.IdGrupoOcupacional;
                    if (objUsuario.Trabajador.GrupoOcupacional != null)
                        objUsuarioModel.GrupoOcupacional = objUsuario.Trabajador.GrupoOcupacional.Nombre;
                    if (objUsuario.Trabajador.FechaNacimiento != null)
                        objUsuarioModel.FechaNacimiento = objUsuario.Trabajador.FechaNacimiento.Value.ToString("dd/MM/yyyy");

                    objUsuarioModel.Genero = objUsuario.Trabajador.Genero;
                    objUsuarioModel.Celular = objUsuario.Trabajador.Celular;
                    objUsuarioModel.Provincia = objUsuario.Trabajador.Provincia;
                    objUsuarioModel.Region = objUsuario.Trabajador.Region;
                    objUsuarioModel.Pais = objUsuario.Trabajador.Pais;


                    objUsuarioModel.TipoContrato = objUsuario.Trabajador.TipoContrato;
                    objUsuarioModel.Nivel = objUsuario.Trabajador.Nivel;



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
                throw;
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
                throw;
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

            return objUsuarioModel;
        }
    }
}