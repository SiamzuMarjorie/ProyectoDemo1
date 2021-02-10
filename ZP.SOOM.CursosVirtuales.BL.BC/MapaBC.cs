using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.Util;
using ZP.SOOM.CursosVirtuales.DL.DA;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.BL.BC
{
    public class MapaBC
    {

        #region Logica Mapa(Slot)

        public IQueryable<Slot> ListarSlot()
        {
            try
            {
                return new SlotDA().ListarSlot();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Grupo> ListarGrupo()
        {
            try
            {
                return new GrupoDA().ListarGrupo();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<string> ListarNombreGrupo()
        {
            try
            {
                return new GrupoDA().ListarNombreGrupo();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Logica CursoSlot

        public IQueryable<CursoSlot> ObtenerListaCursoSlotXIdSlot(int IdSlot, int[] IdsCurso)
        {
            try
            {
                return new CursoSlotDA().ObtenerListaCursoSlotXIdSlot(IdSlot, IdsCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListaCursoSlotXIdSlot(int IdSlot)
        {
            try
            {
                return new CursoSlotDA().ListaCursoSlotXIdSlot(IdSlot);
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
                new InscripcionDA().RegistrarDescargaCertificado(IdInscripcion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<ContenidoCurso> ListarContenidoCurso(int IdCurso)
        {
            try
            {
                return new ContenidoCursoDA().ListarContenidoCurso(IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListarCursoSlot()
        {
            try
            {
                return new CursoSlotDA().ListarCursoSlot();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> ListarCursoSlotPorAno(int? Ano)
        {
            try
            {
                return new CursoSlotDA().ListarCursoSlotPorAno(Ano);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlot(int IdCursoSlot)
        {
            try
            {
                return new CursoSlotDA().ObtenerCursoSlot(IdCursoSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Grupo ObtenerGrupo(int IdGrupo)
        {
            try
            {
                return new GrupoDA().ObtenerGrupo(IdGrupo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<int> ListarIdsCursoXTrabajador(int IdTrabajador)
        {
            try
            {
                return new CursoDA().ListarIdsCursoXTrabajador(IdTrabajador);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public CursoSlot ObtenerOrdenCursoSlot(int Orden, int IdSlot)
        {
            try
            {
                return new CursoSlotDA().ObtenerOrdenCursoSlot(Orden, IdSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotXIdSlot(int IdSlot)
        {
            try
            {
                return new CursoSlotDA().ObtenerCursoSlotXIdSlot(IdSlot);
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
                return new InscripcionDA().ObtenerInscripcion(IdInscripcion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int ObtenerMayorIntento(int IdInscripcion)
        {
            try
            {
                return new IntentoDA().ObtenerMayorIntento(IdInscripcion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion


        public int GuardarCursoSlot(CursoSlot objCursoSlot)
        {
            try
            {
                if (objCursoSlot.IdCursoSlot == 0)
                {
                    CursoSlotDA ObjCursoSlotDA = new CursoSlotDA();
                    int IdCursoSlot = ObjCursoSlotDA.InsertarCursoSlot(objCursoSlot);
                    ObjCursoSlotDA.ActualizarCursoSlotDependiente(objCursoSlot.IdSlot);

                   return IdCursoSlot;
                }
                else
                {
                   new CursoSlotDA().ActualizarCursoSlot(objCursoSlot);
                   return objCursoSlot.IdCursoSlot;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertarCursoSlot(CursoSlot objCursoSlot)
        {
            try
            {
                 return new CursoSlotDA().InsertarCursoSlot(objCursoSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarCursoSlot(CursoSlot objCursoSlot)
        {
            try
            {
                 new CursoSlotDA().ActualizarCursoSlot(objCursoSlot);
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
                new InscripcionDA().ActualizarInscripcion(objInscripcion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarOrdenCursoSlot(int IdCursoSlot, int NumeroOrden, int OrdenIdCursoSlot)
        {
            try
            {
                CursoSlotDA objCursoSlotDA = new CursoSlotDA();
                CursoSlot objCursoSlot = objCursoSlotDA.ObtenerCursoSlot(IdCursoSlot);
                CursoSlot objOrdenCursoSlot = objCursoSlotDA.ObtenerCursoSlot(OrdenIdCursoSlot);
                objOrdenCursoSlot.Orden = NumeroOrden > objCursoSlot.Orden ? NumeroOrden - 1 : NumeroOrden + 1;
                objCursoSlot.Orden = NumeroOrden;
                objCursoSlotDA.ActualizarCursoSlot(objCursoSlot);
                objCursoSlotDA.ActualizarCursoSlot(objOrdenCursoSlot);
                objCursoSlotDA.ActualizarCursoSlotDependiente(objCursoSlot.IdSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarCursoSlotDependiente(int IdSlot, bool CursosDependientes)
        {
            try
            {
                Slot objSlot = new SlotDA().ObtenerSlot(IdSlot);
                objSlot.CursosDependientes = CursosDependientes;
                new SlotDA().GuardarSlot(objSlot);
                new CursoSlotDA().ActualizarCursoSlotDependiente(IdSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Slot ObtenerSlot(int IdSlot)
        {
            try
            {
                return new SlotDA().ObtenerSlot(IdSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotPorCursoActivo(int IdCurso)
        {
            try
            {
                return new CursoSlotDA().ObtenerCursoSlotPorCursoActivo(IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int? ObtenerOrdenMayorCursoSlot(int IdSlot)
        {
            try
            {
                return new CursoBC().ListarCursoSlotAsignado(IdSlot).Max(cs => cs.Orden);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarOrdenCursoSlot(int IdCursoSlot)
        {
            try
            {
                new CursoSlotDA().ActualizarOrdenCursoSlot(IdCursoSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<CursoSlot> FiltarCursoSlot(string Tipo, string Valor, string Estado)
        {
            try
            {
                return new CursoSlotDA().FiltrarCursoSlot(Tipo, Valor, Estado);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Configuracion ObtenerConfiguracionPorNombre(string Nombre)
        {
            try
            {
                return new ConfiguracionDA().ObtenerConfiguracionPorNombre(Nombre);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public MaterialInscripcion ObtenerMaterialInscripcionPorIdMaterialYIdCurso(int IdMaterial, int IdCurso)
        {
            try
            {
                return new MaterialInscripcionDA().ObtenerMaterialInscripcionPorIdMaterialYIdCurso(IdMaterial, IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ActualizarMaterialInscripcion(MaterialInscripcion objMaterialInscripcion)
        {
            try
            {
                new MaterialInscripcionDA().ActualizarMaterialInscripcion(objMaterialInscripcion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotPorIdCurso(int IdCurso)
        {
            try
            {
                return new CursoSlotDA().ObtenerCursoSlotPorIdCurso(IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CursoSlot ObtenerCursoSlotAsignadoPorIdCurso(int IdCurso)
        {
            try
            {
                return new CursoSlotDA().ObtenerCursoSlotAsignadoPorIdCurso(IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarMaterialInscripcion(int IdMaterial, int IdCurso)
        {
            try
            {
                new MaterialInscripcionDA().EliminarMaterialInscripcion(IdMaterial, IdCurso);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarGrupo(int IdGrupo)
        {
            try
            {
                new GrupoDA().EliminarGrupo(IdGrupo);
                IQueryable<GrupoCurso> lstGrupoCurso = new GrupoCursoDA().ListarGrupo(IdGrupo);
                foreach (GrupoCurso objGrupoCurso in lstGrupoCurso)
                {
                    new GrupoCursoDA().ActualizarFechaDesasignacion(objGrupoCurso.IdGrupoCurso);
                }
                IQueryable<GrupoTrabajador> lstGrupoTrabajador = new GrupoTrabajadorDA().ListarGrupo(IdGrupo);
                foreach (GrupoTrabajador objGrupoTrabajador in lstGrupoTrabajador)
                {
                    new GrupoTrabajadorDA().ActualizarFechaDesasignacion(objGrupoTrabajador.IdGrupoTrabajador);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarTrabajadoresGrupo(int IdGrupo)
        {
            try
            {
                new GrupoDA().EliminarTrabajadoresGrupo(IdGrupo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EliminarTrabajadorGrupo(int IdGrupo, int IdTrabajador)
        {
            try
            {
                new GrupoDA().EliminarTrabajadorGrupo(IdGrupo, IdTrabajador);
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
                InscripcionDA objInscripcionDA = new InscripcionDA();
                Inscripcion objInscripcion = objInscripcionDA.ObtenerInscripcion(IdCursoSlot, IdTrabajador);
                if (objInscripcion == null)
                {
                    objInscripcion = objInscripcionDA.GuardarInscripcion(IdCursoSlot, IdTrabajador);

                    CursoSlot obtenerCursoSlot = new MapaBC().ObtenerCursoSlot(IdCursoSlot);
                    List<Material> lstMaterial = obtenerCursoSlot.Curso.Material.ToList();
                    foreach (Material objMaterial in lstMaterial)
                    {
                        MaterialInscripcion objMaterialInscripcion = new MaterialInscripcion();
                        objMaterialInscripcion.IdMaterial = objMaterial.IdMaterial;
                        objMaterialInscripcion.IdInscripcion = objInscripcion.IdInscripcion;
                        objMaterialInscripcion.Visto = false;
                        objMaterialInscripcion.IdCurso = obtenerCursoSlot != null ? obtenerCursoSlot.IdCurso : null;
                        objMaterialInscripcion.Nombre = objMaterial.Nombre;
                        objMaterialInscripcion.TipoMaterial = objMaterial.TipoMaterial;
                        objMaterialInscripcion.Archivo = objMaterial.Archivo;
                        objMaterialInscripcion.Obligatorio = objMaterial.Obligatorio;
                        new CursoBC().IngresarMaterialInscripcion(objMaterialInscripcion);
                    }

                }
                return objInscripcion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistrarPosicionPersonaje(int IdUsuario, int PosicionX, int PosicionY)
        {
            try
            {
                PersonajeDA objPersonajeDA = new PersonajeDA();
                objPersonajeDA.RegistrarPosicionPersonaje(IdUsuario, PosicionX, PosicionY);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Personaje RegistrarAlias(int IdUsuario, string Alias)
        {
            try
            {
                return new PersonajeDA().RegistrarAlias(IdUsuario, Alias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Personaje ObtenerPosicionPersonaje(int IdUsuario)
        {
            try
            {
                return new PersonajeDA().ObtenerPosicionPersonaje(IdUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Personaje RegistrarAvatar(int IdUsuario, string Avatar)
        {
            try
            {                
                return new PersonajeDA().CambiarAvatar(IdUsuario, Avatar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GuardarIntento(Intento objIntento)
        {
            try
            {
                return new IntentoDA().GuardarIntento(objIntento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GuardarGrupo(Grupo objGrupo)
        {
            try
            {
                return new GrupoDA().GuardarGrupo(objGrupo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Personaje ObtenerPersonaje(int IdUsuario)
        {
            try
            {
                return new PersonajeDA().ObtenerPersonaje(IdUsuario);
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

        public void ActualizarPersonaje(int IdUsuario, string Alias, string Avatar)
        {
            try
            {
                new PersonajeDA().ActualizarPersonaje(IdUsuario, Alias, Avatar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double ObtenerSumaPuntajeTrabajador(int IdTrabajador)
        {
            try
            {
                return new InscripcionDA().ObtenerSumaPuntajeTrabajador(IdTrabajador);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public CursoSlot ObtenerCursoSlotActivo(int IdCursoSlot)
        {
            try
            {
                return new CursoSlotDA().ObtenerCursoSlotActivo(IdCursoSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public CursoSlot ObtenerCursoSlotActivoPorIdCurso(int IdCurso)
        {
            try
            {
                 return new CursoSlotDA().ObtenerCursoSlotActivoPorIdCurso(IdCurso);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        //public IQueryable<Inscripcion> ListarInscripcionAprobadoConNotaSinTerminarSinCertificado()
        //{
        //    try
        //    {
        //        return new InscripcionDA().ListarInscripcionAprobadoConNotaSinTerminarSinCertificado();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public int GuardarSlot(Slot objSlot)
        {
            try
            {
                return new SlotDA().GuardarSlot(objSlot);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
