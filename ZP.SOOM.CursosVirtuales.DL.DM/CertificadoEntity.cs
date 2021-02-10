using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZP.SOOM.CursosVirtuales.DL.DM
{
    public class CertificadoEntity
    {
        public string Codigo { get; set; }
        public string Certificado { get; set; }
        public string Compañia { get; set; }

        public CertificadoEntity()
        {

        }
        public CertificadoEntity(string codigo, string certificado, string compañia)
        {
            this.Codigo = codigo;
            this.Certificado = certificado;
            this.Compañia = compañia;

        }


    }
}
