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
    
    public partial class AccountingSosDBEntities : DbContext
    {
        public AccountingSosDBEntities()
            : base("name=AccountingSosDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Banco> Bancos { get; set; }
        public virtual DbSet<CodigosAuditoria> CodigosAuditorias { get; set; }
        public virtual DbSet<Contraparte> Contrapartes { get; set; }
        public virtual DbSet<Territorio> Territorios { get; set; }
        public virtual DbSet<TiposComprobante> TiposComprobantes { get; set; }
        public virtual DbSet<AccionesNacionale> AccionesNacionales { get; set; }
        public virtual DbSet<AnexosTributario> AnexosTributarios { get; set; }
        public virtual DbSet<Ciudad> Ciudades { get; set; }
        public virtual DbSet<CuentaBanco> CuentasBancos { get; set; }
        public virtual DbSet<NivelProgramatico> NivelProgramaticos { get; set; }
        public virtual DbSet<RecursoMes> RecursosMeses { get; set; }
        public virtual DbSet<CuentaContable> CuentasContables { get; set; }
        public virtual DbSet<CuentaNavision> CuentasNavision { get; set; }
        public virtual DbSet<FacturaComprador> FacturasCompradores { get; set; }
        public virtual DbSet<PlanProgramatico> PlanProgramaticos { get; set; }
        public virtual DbSet<TiposCuenta> TiposCuentas { get; set; }
        public virtual DbSet<Comprobante> Comprobantes { get; set; }
        public virtual DbSet<TipoMoneda> TiposMonedas { get; set; }
        public virtual DbSet<CuentaAsiento> CuentasAsientos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<EstadoPresupuesto> EstadosPresupuestos { get; set; }
        public virtual DbSet<TipoObservacion> TiposObservaciones { get; set; }
        public virtual DbSet<Observacion> Observaciones { get; set; }
        public virtual DbSet<Recurso> Recursos { get; set; }
        public virtual DbSet<TraspasosPrograma> TraspasosProgramas { get; set; }
        public virtual DbSet<Factura> Facturas { get; set; }
        public virtual DbSet<Retencion> Retenciones { get; set; }
        public virtual DbSet<TiposRetencion> TiposRetencion { get; set; }
        public virtual DbSet<Cierre> Cierres { get; set; }
        public virtual DbSet<EstadosCierre> EstadosCierres { get; set; }
        public virtual DbSet<EstadosComprobante> EstadosComprobantes { get; set; }
        public virtual DbSet<TiposCierre> TiposCierres { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<TipoCambio> TiposCambio { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<UsuarioRol> UsuariosRoles { get; set; }
        public virtual DbSet<PlantillaAsiento> PlantillasAsientos { get; set; }
        public virtual DbSet<PlantillaCuenta> PlantillasCuentas { get; set; }
        public virtual DbSet<TiposFacility> TiposFacilities { get; set; }
        public virtual DbSet<UsuarioCiudad> UsuariosCiudades { get; set; }
        public virtual DbSet<CierreContable> CierresContables { get; set; }
        public virtual DbSet<PeriodoCierre> PeriodosCierres { get; set; }
        public virtual DbSet<VCierreContable> VCierresContables { get; set; }
        public virtual DbSet<EstadoCuenta> EstadosCuentas { get; set; }
        public virtual DbSet<TiposEstadosCuenta> TiposEstadosCuentas { get; set; }
        public virtual DbSet<VComprobante> VComprobantes { get; set; }
        public virtual DbSet<PresupuestoCiudad> PresupuestosCiudades { get; set; }
        public virtual DbSet<PresupuestoFacility> PresupuestosFacilities { get; set; }
        public virtual DbSet<TiposPresupuesto> TiposPresupuestos { get; set; }
        public virtual DbSet<Presupuesto> Presupuestos { get; set; }
        public virtual DbSet<FacilityCuentaContable> FacilitiesCuentasContables { get; set; }
        public virtual DbSet<FacilityPlanProgramatico> FacilitiesPlanProgramaticos { get; set; }
        public virtual DbSet<PlanProgramaticoCuenta> PlanProgramaticoCuentas { get; set; }
        public virtual DbSet<PresupuestoFacilityCompartido> PresupuestosFacilitiesCompartidos { get; set; }
        public virtual DbSet<CodigoMarcoLogico> CodigosMarcoLogicos { get; set; }
        public virtual DbSet<RecursosExcel> RecursosExcels { get; set; }
        public virtual DbSet<RecursosExcelLote> RecursosExcelLotes { get; set; }
    
        public virtual ObjectResult<SumasSaldos_Result> SumasSaldos(string fechaInicio, string fechaFin)
        {
            var fechaInicioParameter = fechaInicio != null ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(string));
    
            var fechaFinParameter = fechaFin != null ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SumasSaldos_Result>("SumasSaldos", fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<BalanceGeneral_Result> BalanceGeneral(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, Nullable<int> ufvAnterior, Nullable<int> ufvActual)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var ufvAnteriorParameter = ufvAnterior.HasValue ?
                new ObjectParameter("UfvAnterior", ufvAnterior) :
                new ObjectParameter("UfvAnterior", typeof(int));
    
            var ufvActualParameter = ufvActual.HasValue ?
                new ObjectParameter("UfvActual", ufvActual) :
                new ObjectParameter("UfvActual", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BalanceGeneral_Result>("BalanceGeneral", fechaInicioParameter, fechaFinParameter, ufvAnteriorParameter, ufvActualParameter);
        }
    
        public virtual ObjectResult<EstadoResultado_Result> EstadoResultado(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, Nullable<int> ufvAnterior, Nullable<int> ufvActual)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var ufvAnteriorParameter = ufvAnterior.HasValue ?
                new ObjectParameter("UfvAnterior", ufvAnterior) :
                new ObjectParameter("UfvAnterior", typeof(int));
    
            var ufvActualParameter = ufvActual.HasValue ?
                new ObjectParameter("UfvActual", ufvActual) :
                new ObjectParameter("UfvActual", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<EstadoResultado_Result>("EstadoResultado", fechaInicioParameter, fechaFinParameter, ufvAnteriorParameter, ufvActualParameter);
        }
    
        public virtual ObjectResult<LibroVentas_Result> LibroVentas(Nullable<int> mes, Nullable<int> año)
        {
            var mesParameter = mes.HasValue ?
                new ObjectParameter("mes", mes) :
                new ObjectParameter("mes", typeof(int));
    
            var añoParameter = año.HasValue ?
                new ObjectParameter("año", año) :
                new ObjectParameter("año", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibroVentas_Result>("LibroVentas", mesParameter, añoParameter);
        }
    
        public virtual ObjectResult<LibroCompras_Result> LibroCompras(Nullable<int> mes, Nullable<int> año)
        {
            var mesParameter = mes.HasValue ?
                new ObjectParameter("mes", mes) :
                new ObjectParameter("mes", typeof(int));
    
            var añoParameter = año.HasValue ?
                new ObjectParameter("año", año) :
                new ObjectParameter("año", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibroCompras_Result>("LibroCompras", mesParameter, añoParameter);
        }
    
        public virtual ObjectResult<Mayores_Result> Mayores(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string numeroCuenta)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var numeroCuentaParameter = numeroCuenta != null ?
                new ObjectParameter("NumeroCuenta", numeroCuenta) :
                new ObjectParameter("NumeroCuenta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Mayores_Result>("Mayores", fechaInicioParameter, fechaFinParameter, numeroCuentaParameter);
        }
    
        public virtual ObjectResult<BalanceComprobacion_Result> BalanceComprobacion(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BalanceComprobacion_Result>("BalanceComprobacion", fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<BalanceCuentasUnitarias_Result> BalanceCuentasUnitarias(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string ci, string cf)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var ciParameter = ci != null ?
                new ObjectParameter("ci", ci) :
                new ObjectParameter("ci", typeof(string));
    
            var cfParameter = cf != null ?
                new ObjectParameter("cf", cf) :
                new ObjectParameter("cf", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BalanceCuentasUnitarias_Result>("BalanceCuentasUnitarias", fechaInicioParameter, fechaFinParameter, ciParameter, cfParameter);
        }
    
        public virtual ObjectResult<BalanceRangoCuentas_Result> BalanceRangoCuentas(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string ci, string cf)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var ciParameter = ci != null ?
                new ObjectParameter("ci", ci) :
                new ObjectParameter("ci", typeof(string));
    
            var cfParameter = cf != null ?
                new ObjectParameter("cf", cf) :
                new ObjectParameter("cf", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BalanceRangoCuentas_Result>("BalanceRangoCuentas", fechaInicioParameter, fechaFinParameter, ciParameter, cfParameter);
        }
    
        public virtual ObjectResult<BalanceTotalRangos_Result> BalanceTotalRangos(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string ci, string cf)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var ciParameter = ci != null ?
                new ObjectParameter("ci", ci) :
                new ObjectParameter("ci", typeof(string));
    
            var cfParameter = cf != null ?
                new ObjectParameter("cf", cf) :
                new ObjectParameter("cf", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BalanceTotalRangos_Result>("BalanceTotalRangos", fechaInicioParameter, fechaFinParameter, ciParameter, cfParameter);
        }
    
        public virtual ObjectResult<RptPresupuestoResumen_Result> RptPresupuestoResumen(Nullable<int> presupuestoId, Nullable<int> ciudadId, Nullable<int> facilityId)
        {
            var presupuestoIdParameter = presupuestoId.HasValue ?
                new ObjectParameter("presupuestoId", presupuestoId) :
                new ObjectParameter("presupuestoId", typeof(int));
    
            var ciudadIdParameter = ciudadId.HasValue ?
                new ObjectParameter("ciudadId", ciudadId) :
                new ObjectParameter("ciudadId", typeof(int));
    
            var facilityIdParameter = facilityId.HasValue ?
                new ObjectParameter("facilityId", facilityId) :
                new ObjectParameter("facilityId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RptPresupuestoResumen_Result>("RptPresupuestoResumen", presupuestoIdParameter, ciudadIdParameter, facilityIdParameter);
        }
    }
}
