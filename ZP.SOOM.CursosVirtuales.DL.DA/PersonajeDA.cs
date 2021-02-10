using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class PersonajeDA
    {

        public int RegistrarPosicionPersonaje(int IdUsuario, int PosicionX, int PosicionY)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Personaje objPersonaje = objModel.Personaje.FirstOrDefault(x => x.IdUsuario == IdUsuario);

                if (objPersonaje == null)
                {
                    objPersonaje = new Personaje();
                    objPersonaje.IdUsuario = IdUsuario;
                    objPersonaje.PosicionX = PosicionX;
                    objPersonaje.PosicionY = PosicionY;
                    objModel.Personaje.Add(objPersonaje);
                }
                objPersonaje.IdUsuario = IdUsuario;
                objPersonaje.PosicionX = PosicionX;
                objPersonaje.PosicionY = PosicionY;
                objModel.SaveChanges();

                return objPersonaje.IdUsuario;
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
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Personaje objPersonaje = objModel.Personaje.FirstOrDefault(x => x.IdUsuario == IdUsuario);

                return objPersonaje;
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
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Personaje objPersonaje = objModel.Personaje.FirstOrDefault(x => x.IdUsuario == IdUsuario);
                objPersonaje.Alias = Alias;
                objModel.SaveChanges();

                return objPersonaje;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Personaje CambiarAvatar(int IdUsuario, string Avatar)
        {
            try
            {
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Personaje objPersonaje = objModel.Personaje.FirstOrDefault(x => x.IdUsuario == IdUsuario);
                objPersonaje.Avatar = Avatar;
                objModel.SaveChanges();

                return objPersonaje;
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
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Personaje objPersonaje = objModel.Personaje.FirstOrDefault(x => x.IdUsuario == IdUsuario);

                return objPersonaje;
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
                SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();
                Personaje objPersonajeBD = objModel.Personaje.FirstOrDefault(x => x.IdUsuario == IdUsuario);
                if (objPersonajeBD == null)
                {
                    Personaje objpersonaje = new Personaje();
                    objpersonaje.IdUsuario = IdUsuario;
                    objpersonaje.Alias = Alias;
                    objpersonaje.Avatar = Avatar;
                    objpersonaje.PosicionX = 61;
                    objpersonaje.PosicionY = 43;
                    objModel.Personaje.Add(objpersonaje);
                }
                else
                {
                    objPersonajeBD.Alias = Alias;
                    objPersonajeBD.Avatar = Avatar;
                }
                objModel.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
