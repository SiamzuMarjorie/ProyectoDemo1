using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DA;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class CargaMasivaDA
    {

        public void GuardarUsuarioCargaMasiva(List<Usuario> lstUsuario, List<Trabajador> lstTrabajador, List<string> lstCodigoTrabajadorJefe, List<string> lstGrupoOcupacional, out string outMensajeError)
        {
            SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
            using (var transaction = objModel.Database.BeginTransaction())
            {
                try
                {
                    string[] Codigos = lstTrabajador.Select(t => t.Codigo).ToArray();
                    List<Trabajador> lstTrabajadorExistente = objModel.Trabajador.Where(t => Codigos.Contains(t.Codigo)).ToList();
                    List<Trabajador> lstTrabajadorNuevos = lstTrabajador.Where(t=> !objModel.Trabajador.Any(tr => tr.Codigo == t.Codigo) && !t.Eliminado).ToList();

                    foreach (Trabajador objTrabajador in lstTrabajadorExistente)
                    {
                        Trabajador objTrabajadorActualizado = lstTrabajador.FirstOrDefault(t => t.Codigo.ToLower() == objTrabajador.Codigo.ToLower());
                        objTrabajador.Nombres = objTrabajadorActualizado.Nombres;
                        objTrabajador.PrimerApellido = objTrabajadorActualizado.PrimerApellido;
                        objTrabajador.SegundoApellido = objTrabajadorActualizado.SegundoApellido;
                        objTrabajador.Codigo = objTrabajadorActualizado.Codigo;
                        objTrabajador.DNI = objTrabajadorActualizado.DNI;
                        objTrabajador.Email = objTrabajadorActualizado.Email;
                        objTrabajador.Puesto = objTrabajadorActualizado.Puesto;
                        objTrabajador.Area = objTrabajadorActualizado.Area;
                        objTrabajador.Estado = objTrabajadorActualizado.Estado;
                        objTrabajador.FechaNacimiento = objTrabajadorActualizado.FechaNacimiento;
                        objTrabajador.Genero = objTrabajadorActualizado.Genero;
                        objTrabajador.Celular = objTrabajadorActualizado.Celular;
                        objTrabajador.Sede = objTrabajadorActualizado.Sede;
                        objTrabajador.Provincia = objTrabajadorActualizado.Provincia;
                        objTrabajador.Region = objTrabajadorActualizado.Region;
                        objTrabajador.Pais = objTrabajadorActualizado.Pais;
                        objTrabajador.FechaIngreso = objTrabajadorActualizado.FechaIngreso;
                        objTrabajador.IdGrupoOcupacional = objTrabajadorActualizado.IdGrupoOcupacional;
                        objTrabajador.Gerencia = objTrabajadorActualizado.Gerencia;
                        objTrabajador.Rol = objTrabajadorActualizado.Rol;
                        objTrabajador.Compañia = objTrabajadorActualizado.Compañia;
                        objTrabajador.Eliminado = objTrabajadorActualizado.Eliminado;

                        Usuario objUsuario = objTrabajador.Usuario.FirstOrDefault();
                        int Indice = lstTrabajador.IndexOf(objTrabajadorActualizado);
                        objUsuario.Username = lstUsuario[Indice].Username;
                        if (lstUsuario[Indice].Password != null)
                            objUsuario.Password = lstUsuario[Indice].Password;
                        objUsuario.Eliminado = lstUsuario[Indice].Eliminado;

                    }

                    foreach (Trabajador objTrabajadorNuevo in lstTrabajadorNuevos)
                    {
                        int Indice = lstTrabajador.IndexOf(objTrabajadorNuevo);
                        if (lstUsuario[Indice].Password == null)
                        {
                            outMensajeError = "Las contraseñas para los usuarios nuevos son obligatorias. Por favor, complete los campos necesarios.";
                            transaction.Rollback();
                            return;
                        }
                    }
                    objModel.Trabajador.AddRange(lstTrabajadorNuevos);

                    // INSERTAR TODOS LO GO
                    List<string> lstGrupoOcupacionalNuevos = lstGrupoOcupacional.Where(g => g != null && !objModel.GrupoOcupacional.Any(go => go.Nombre == g)).Distinct().ToList();
                    List<GrupoOcupacional> lstGrupoOcupacionalBD = new List<GrupoOcupacional>();
                    foreach (string Nombre in lstGrupoOcupacionalNuevos)
                    {
                        GrupoOcupacional objGrupoOcupacional = new GrupoOcupacional { Nombre = Nombre};
                        lstGrupoOcupacionalBD.Add(objGrupoOcupacional);
                    }
                    objModel.GrupoOcupacional.AddRange(lstGrupoOcupacionalBD);

                    objModel.SaveChanges();

                    List<Usuario> lstUsuariosNuevos = new List<Usuario>();
                    foreach (Trabajador objTrabajadorNuevo in lstTrabajadorNuevos)
                    {
                        int Indice = lstTrabajador.IndexOf(objTrabajadorNuevo);
                        lstUsuario[Indice].IdTrabajador = objTrabajadorNuevo.IdTrabajador;
                        lstUsuario[Indice].Perfil = Constants.Usuario.Perfil.USUARIO;
                        lstUsuariosNuevos.Add(lstUsuario[Indice]);
                    }
                    objModel.Usuario.AddRange(lstUsuariosNuevos);
                    
                    //INSERTAR GO A TRABAJADORES
                    List<GrupoOcupacional> lstGruposOcupacionales = objModel.GrupoOcupacional.Where(go => lstGrupoOcupacional.Contains(go.Nombre.ToUpper())).ToList();
                    
                    //INSERTAR JEFES A TRABAJADORES
                    string[] CodigosJefe = lstCodigoTrabajadorJefe.Where(c => c != null).Distinct().ToArray();
                    List<Trabajador> lstTrabajorJefeExistente = objModel.Trabajador.Where(t => t.Eliminado != true && CodigosJefe.Contains(t.Codigo)).ToList();
                    if (CodigosJefe.Length != lstTrabajorJefeExistente.Count)
                    {
                        outMensajeError = "Algunos códigos de jefe no existen. Por favor, verifique que los códigos sean correctos.";
                        transaction.Rollback();
                        return;
                    }
                    foreach (Trabajador objTrabajador in lstTrabajadorExistente.Concat(lstTrabajadorNuevos))
                    {
                        int Indice = lstTrabajador.IndexOf(lstTrabajador.FirstOrDefault(t=>t.Codigo == objTrabajador.Codigo));
                        if (lstCodigoTrabajadorJefe[Indice] != null)
                        {
                            Trabajador objTrabajadorJefe = lstTrabajorJefeExistente.FirstOrDefault(t => t.Codigo == lstCodigoTrabajadorJefe[Indice]);
                            objTrabajador.IdTrabajadorJefe = objTrabajadorJefe.IdTrabajador;
                        }
                        else
                        {
                            objTrabajador.IdTrabajadorJefe = null;
                        }

                        if (lstGrupoOcupacional[Indice] != null)
                        {
                            GrupoOcupacional objGrupoOcupacional = lstGruposOcupacionales.FirstOrDefault(go => go.Nombre.ToUpper() == lstGrupoOcupacional[Indice]);
                            objTrabajador.IdGrupoOcupacional = objGrupoOcupacional.IdGrupoOcupacional;
                        }
                    }

                    objModel.SaveChanges();
                    transaction.Commit();
                    outMensajeError = null;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            
        }

        public void GuardarAsignacionMasiva(List<string> lstCodigos, int IdGrupo, out string outMensajeError)
        {
            SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
            using (var transaction = objModel.Database.BeginTransaction())
            {
                try
                {
                    string[] Codigos = lstCodigos.Select(t => t).ToArray();
                    List<Trabajador> lstCodigoExistente = objModel.Trabajador.Where(t => Codigos.Contains(t.Codigo)).ToList();

                    if (lstCodigos.Count == lstCodigoExistente.Count)
                    {
                        foreach (Trabajador objTrabajador in lstCodigoExistente)
                        {
                            GrupoTrabajador objGrupoTrabajador = new GrupoTrabajador();
                            objGrupoTrabajador.IdTrabajador = objTrabajador.IdTrabajador;
                            objGrupoTrabajador.IdGrupo = IdGrupo;
                            objGrupoTrabajador.FechaHoraRegistro = DateTimeHelper.PeruDateTime;
                            objModel.GrupoTrabajador.Add(objGrupoTrabajador);
                        }
                    }
                    else
                    {
                        outMensajeError = "Hay códigos de usuarios que no están registrados en la plataforma.";
                        transaction.Rollback();
                        return;
                    }
                    objModel.SaveChanges();
                    transaction.Commit();
                    outMensajeError = null;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
    }
}
