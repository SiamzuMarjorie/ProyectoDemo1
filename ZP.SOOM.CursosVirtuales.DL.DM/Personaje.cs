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
    
    public partial class Personaje
    {
        public int IdPersonaje { get; set; }
        public int IdUsuario { get; set; }
        public string Alias { get; set; }
        public string Avatar { get; set; }
        public int PosicionX { get; set; }
        public int PosicionY { get; set; }
    
        public virtual Usuario Usuario { get; set; }
    }
}