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
    
    public partial class Pregunta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pregunta()
        {
            this.Respuesta = new HashSet<Respuesta>();
            this.Respuesta1 = new HashSet<Respuesta>();
        }
    
        public int IdPregunta { get; set; }
        public int IdCursoSlot { get; set; }
        public string Contenido { get; set; }
    
        public virtual CursoSlot CursoSlot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Respuesta> Respuesta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Respuesta> Respuesta1 { get; set; }
    }
}