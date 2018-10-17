﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cerberus.Sos.Accounting.DataAccess.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PlanillasSosDBEntities : DbContext
    {
        public PlanillasSosDBEntities()
            : base("name=PlanillasSosDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PARAMETRO> PARAMETROS { get; set; }
        public virtual DbSet<PLANILLA_SUELDOS> PLANILLA_SUELDOS { get; set; }
        public virtual DbSet<ESTADOS_PLANILLAS> ESTADOS_PLANILLAS { get; set; }
        public virtual DbSet<VPERSONAL> VPERSONALs { get; set; }
        public virtual DbSet<VPLANILLA_SUELDOS_GENERAL> VPLANILLA_SUELDOS_GENERAL { get; set; }
        public virtual DbSet<VPLANILLA_AFP_FUTURO> VPLANILLA_AFP_FUTURO { get; set; }
        public virtual DbSet<VPLANILLA_AFP_PREVISION> VPLANILLA_AFP_PREVISION { get; set; }
        public virtual DbSet<VPLANILLA_APORTES_SALUD> VPLANILLA_APORTES_SALUD { get; set; }
        public virtual DbSet<VPLANILLA_MINISTERIO> VPLANILLA_MINISTERIO { get; set; }
        public virtual DbSet<VPLANILLA_SUELDOS> VPLANILLA_SUELDOS { get; set; }
        public virtual DbSet<PLANILLA_RCIVA> PLANILLA_RCIVA { get; set; }
        public virtual DbSet<VPLANILLA_RCIVA> VPLANILLA_RCIVA { get; set; }
        public virtual DbSet<PERIODOS_PLANILLAS> PERIODOS_PLANILLAS { get; set; }
        public virtual DbSet<RETROACTIVO> RETROACTIVOS { get; set; }
        public virtual DbSet<PARAMETROS_PLANILLAS> PARAMETROS_PLANILLAS { get; set; }
        public virtual DbSet<PORCENTAJE_APORTE_NACIONAL_SOL> PORCENTAJE_APORTE_NACIONAL_SOL { get; set; }
        public virtual DbSet<PORCENTAJE_BONO_ANTIGUEDAD> PORCENTAJE_BONO_ANTIGUEDAD { get; set; }
        public virtual DbSet<SALARIOS_MINIMOS> SALARIOS_MINIMOS { get; set; }
        public virtual DbSet<REGISTRO_UFVS> REGISTRO_UFVS { get; set; }
    
        public virtual int GENERA_PLANILLA(Nullable<int> tIPO_CIERRE)
        {
            var tIPO_CIERREParameter = tIPO_CIERRE.HasValue ?
                new ObjectParameter("TIPO_CIERRE", tIPO_CIERRE) :
                new ObjectParameter("TIPO_CIERRE", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GENERA_PLANILLA", tIPO_CIERREParameter);
        }
    
        public virtual int GENERA_PLANILLA_RCIVA(Nullable<int> tIPO_CIERRE)
        {
            var tIPO_CIERREParameter = tIPO_CIERRE.HasValue ?
                new ObjectParameter("TIPO_CIERRE", tIPO_CIERRE) :
                new ObjectParameter("TIPO_CIERRE", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GENERA_PLANILLA_RCIVA", tIPO_CIERREParameter);
        }
    }
}