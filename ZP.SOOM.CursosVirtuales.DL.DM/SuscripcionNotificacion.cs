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
    
    public partial class SuscripcionNotificacion
    {
        public int IdSuscripcionNotificacion { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public string Endpoint { get; set; }
        public string PS56DH { get; set; }
        public string Auth { get; set; }
    
        public virtual Usuario Usuario { get; set; }
    }
}
