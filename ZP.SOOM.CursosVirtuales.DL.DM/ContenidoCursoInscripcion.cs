//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZP.SOOM.CursosVirtuales.DL.DM
{
    using System;
    using System.Collections.Generic;
    
    public partial class ContenidoCursoInscripcion
    {
        public int IdContenidoCursoInscripcion { get; set; }
        public int IdContenidoCurso { get; set; }
        public int IdInscripcion { get; set; }
        public Nullable<System.DateTime> FechaHoraRevision { get; set; }
    
        public virtual ContenidoCurso ContenidoCurso { get; set; }
        public virtual Inscripcion Inscripcion { get; set; }
    }
}