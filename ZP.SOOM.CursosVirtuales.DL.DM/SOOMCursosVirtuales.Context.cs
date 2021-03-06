﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SOOMCursosVirtualesEntities : DbContext
    {
        public SOOMCursosVirtualesEntities()
            : base("name=SOOMCursosVirtualesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ActividadUsuario> ActividadUsuario { get; set; }
        public virtual DbSet<Configuracion> Configuracion { get; set; }
        public virtual DbSet<ContenidoCurso> ContenidoCurso { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<CursoSlot> CursoSlot { get; set; }
        public virtual DbSet<GrupoOcupacional> GrupoOcupacional { get; set; }
        public virtual DbSet<Inscripcion> Inscripcion { get; set; }
        public virtual DbSet<Intento> Intento { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialInscripcion> MaterialInscripcion { get; set; }
        public virtual DbSet<Personaje> Personaje { get; set; }
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<Respuesta> Respuesta { get; set; }
        public virtual DbSet<Slot> Slot { get; set; }
        public virtual DbSet<SuscripcionNotificacion> SuscripcionNotificacion { get; set; }
        public virtual DbSet<Trabajador> Trabajador { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<ContenidoCursoInscripcion> ContenidoCursoInscripcion { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<GrupoCurso> GrupoCurso { get; set; }
        public virtual DbSet<GrupoTrabajador> GrupoTrabajador { get; set; }
    
        public virtual ObjectResult<DescargarBaseTrabajadores_Result> DescargarBaseTrabajadores()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DescargarBaseTrabajadores_Result>("DescargarBaseTrabajadores");
        }
    }
}
