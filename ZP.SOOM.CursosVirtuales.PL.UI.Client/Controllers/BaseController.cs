using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GemBox.Presentation;

using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Client.Models;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Controllers
{
    public class BaseController : Controller
    {
        public UsuarioModel UsuarioLogueado
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    Usuario objUsuario = new UsuarioBC().ObtenerUsuario(UsuarioModel.FromString(User.Identity.Name).IdUsuario);

                    if (objUsuario != null)
                        return UsuarioModel.FromUsuario(objUsuario);
                }

                return null;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UsuarioLogueado == null)
            {
                FormsAuthentication.SignOut();
                filterContext.Result = RedirectToAction("Index", "Login");
                base.OnActionExecuting(filterContext);
            }
        }

        public List<CursoModel> ListarCursos()
        {
            try
            {
                IQueryable<Curso> lstCursos = new CursoBC().ListarCurso().Where(c => !c.Eliminado);
                List<CursoModel> lstCursosModel = new List<CursoModel>();
                foreach (Curso objCurso in lstCursos)
                {
                    lstCursosModel.Add(CursoModel.FromCurso(objCurso));
                }
                return lstCursosModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<CursoModel> ListarCursoSlotIngresados()
        {
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                IQueryable<CursoSlot> lstCursosIngresados = new CursoBC().ListarCursoSlotIngresados(objUsuarioLogueado.IdTrabajador).Where(cs => cs.Curso != null);
                List<CursoModel> lstCursosIngresadosModel = new List<CursoModel>();
                if (lstCursosIngresados.Count() > 0)
                {
                    foreach (CursoSlot objCursosIngresados in lstCursosIngresados)
                    {
                        CursoModel objCursosIngresadosModel = CursoModel.FromCursoSlot(objCursosIngresados);
                        lstCursosIngresadosModel.Add(objCursosIngresadosModel);
                    }
                }

                return lstCursosIngresadosModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<SlotModel> ListarSlotAsignados()
        {
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                IQueryable<Slot> lstSlot = new MapaBC().ListarSlot();
                List<SlotModel> lstSlotModel = new List<SlotModel>();
                MapaBC objMapaBC = new MapaBC();
                List<int> IdsCurso = objMapaBC.ListarIdsCursoXTrabajador(objUsuarioLogueado.IdTrabajador).ToList();

                foreach (Slot objSlot in lstSlot)
                {
                    SlotModel objSlotModel = SlotModel.FromSlot(objSlot);
                    if (objSlot.CursoSlot.Where(s => s.Estado == Constants.Cursos.Estado.ACTIVO && s.FechaHoraFin > DateTimeHelper.PeruDateTime && IdsCurso.Contains(s.IdCurso ?? 0)).Count() > 0)
                    {
                        lstSlotModel.Add(objSlotModel);
                    }
                }

                return lstSlotModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public PersonajeModel ObtenerPersonaje()
        {
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                Personaje objPersonaje = new MapaBC().ObtenerPersonaje(objUsuarioLogueado.IdUsuario);
                PersonajeModel objPersonajeModel = PersonajeModel.FromPersonaje(objPersonaje);

                return objPersonajeModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public double ObtenerSumaPuntajeTrabajador()
        {
            try
            {
                UsuarioModel objUsuarioLogueado = UsuarioModel.FromString(User.Identity.Name);
                double puntajeTrabajador = new MapaBC().ObtenerSumaPuntajeTrabajador(objUsuarioLogueado.IdTrabajador);
                return puntajeTrabajador;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GenerarCertificado(string Certificado, CursoSlot objCursoSlot, int? IdTrabajador, double? Nota, double? NuevoPuntaje)
        {
            try
            {

                UsuarioBC objUsuarioBC = new UsuarioBC();

                Trabajador objTrabajador = objUsuarioBC.ObtenerTrabajador(IdTrabajador.Value);

                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                var rutaCertificado= CursoModel.PLANTILLA_CERTIFICADO.Replace(CursoModel.CODIGO_CURSO, objCursoSlot.Codigo).Replace(CursoModel.ARCHIVO, objCursoSlot.Curso.Certificado);
                var powerpoint = PresentationDocument.Load(Server.MapPath(rutaCertificado));

                var slide = powerpoint.Slides[0];

                slide.TextContent.Replace("[NOMBRE_COLABORADOR]", objTrabajador.Nombres + " " + objTrabajador.PrimerApellido + " " + objTrabajador.SegundoApellido);
                slide.TextContent.Replace("[NOMBRE_SLOT]", objCursoSlot.Slot.NombreSlot);
                slide.TextContent.Replace("[NOMBRE_CURSO]", objCursoSlot.Curso.Titulo);
                slide.TextContent.Replace("[FECHA]", DateTimeHelper.PeruDateTime.ToString("dd/MM/yyyy"));
                slide.TextContent.Replace("[HORAS]", objCursoSlot.Curso.Horas != null ? objCursoSlot.Curso.Horas.Value.ToString() : String.Empty);
                if (Nota != null)
                    slide.TextContent.Replace("[NOTA]", Nota.ToString());
                else
                    slide.TextContent.Replace("[NOTA]", "-");
                if (NuevoPuntaje != null)
                    slide.TextContent.Replace("[PUNTAJE]", NuevoPuntaje != null ? NuevoPuntaje.Value.ToString() : String.Empty);

                Certificado = CursoModel.NOMBRE_ARCHIVO.Replace(CursoModel.PERSONA, objTrabajador.Nombres + " " + objTrabajador.PrimerApellido + " " + objTrabajador.SegundoApellido).Replace(CursoModel.CURSO, objCursoSlot.Curso.Titulo).Replace(CursoModel.FECHA, DateTimeHelper.PeruDateTime.ToString("dd-MM-yyyy"));
                string pathPDF = CursoModel.URL_CERTIFICADO.Replace(CursoModel.CODIGO_CURSO, objCursoSlot.Codigo).Replace(CursoModel.ARCHIVO, Certificado);

                powerpoint.Save(Server.MapPath(pathPDF));
                return Certificado;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
