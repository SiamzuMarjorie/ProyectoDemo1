using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

namespace ZP.SOOM.CursosVirtuales.BL.BE
{
    public class ReporteStatus
    {
        public bool Ingresado { get; set; }
        public Trabajador Trabajador { get; set; }
        public string TituloCurso { get; set; }
        public DateTime? FechaPrimeraVez { get; set; }
        public DateTime? FechaUltimaVez { get; set; }
        public bool TerminoCurso { get; set; }
        public int TotalMateriales { get; set; }
        public int MaterialesVisto { get; set; }
        public List<Respuesta> Respuestas { get; set; }
        public int RespuestasCorrectas { get; set; }
        public int RespuestasIncorrectas { get; set; }
        public double Porcentaje { get; set; }
        public double? Nota { get; set; }
        public bool? Aprobado { get; set; }
        public bool CertificadoDescargado { get; set; }
        public List<Pregunta> Preguntas { get; set; }
    }
}
