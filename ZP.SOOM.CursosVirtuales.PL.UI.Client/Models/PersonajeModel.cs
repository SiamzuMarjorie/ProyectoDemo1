using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.PL.UI.Client.Models
{
    public class PersonajeModel
    {
        public int IdPersonaje { get; set; }
        public int IdUsuario { get; set; }
        public string Alias { get; set; }
        public string Avatar { get; set; }
        public int PosicionX { get; set; }
        public int PosicionY { get; set; }

        public static PersonajeModel FromPersonaje(Personaje objPersonaje)
        {
            try
            {
                PersonajeModel objPersonajeModel = new PersonajeModel();
                objPersonajeModel.IdPersonaje = objPersonaje.IdPersonaje;
                objPersonajeModel.IdUsuario = objPersonaje.IdUsuario;
                objPersonajeModel.Alias = objPersonaje.Alias;
                objPersonajeModel.Avatar = objPersonaje.Avatar;
                objPersonajeModel.PosicionX = objPersonaje.PosicionX;
                objPersonajeModel.PosicionY = objPersonaje.PosicionY;

                return objPersonajeModel;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

    }
}