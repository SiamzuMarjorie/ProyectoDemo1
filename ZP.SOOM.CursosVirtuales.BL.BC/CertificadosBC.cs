using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DA;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.BL.BC
{
    public class CertificadosBC
    {
        public List<CertificadoEntity> ListarCertificadosbyNombreCampo(string NombreCampo, int? Ano, string[] OpcionSeleccionada)
        {
            try
            {
                CertificadoDA certificadoDA = new CertificadoDA();

                List<CertificadoEntity> lista = new List<CertificadoEntity>();

                lista = certificadoDA.ListarCertificadosbyNombreCampo(NombreCampo, Ano, OpcionSeleccionada);

                return lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<CertificadoEntity> ListarCertificadosbyCursoSlot(int? ano, int[] idCursoSlot)
        {
            try
            {
                CertificadoDA certificadoDA = new CertificadoDA();

                List<CertificadoEntity> lista = new List<CertificadoEntity>();

                lista = certificadoDA.ListarCertificadosbyCursoSlot(ano, idCursoSlot);

                return lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
