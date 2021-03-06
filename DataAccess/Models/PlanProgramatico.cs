//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class PlanProgramatico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanProgramatico()
        {
            this.CuentasAsientos = new HashSet<CuentaAsiento>();
            this.Recursos = new HashSet<Recurso>();
            this.PlantillasCuentas = new HashSet<PlantillaCuenta>();
            this.FacilitiesPlanProgramaticoes = new HashSet<FacilityPlanProgramatico>();
            this.PlanProgramaticoCuentas = new HashSet<PlanProgramaticoCuenta>();
        }
    
        public int Id { get; set; }
        public Nullable<int> PlanProgramaticoId { get; set; }
        public int NivelProgramaticoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Comentario { get; set; }
        public int Gestion { get; set; }
        public bool Seleccionable { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        public virtual NivelProgramatico NivelProgramatico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuentaAsiento> CuentasAsientos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recurso> Recursos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlantillaCuenta> PlantillasCuentas { get; set; }
        public virtual PlanProgramatico PlanProgramaticoParent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FacilityPlanProgramatico> FacilitiesPlanProgramaticoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanProgramaticoCuenta> PlanProgramaticoCuentas { get; set; }
    }
}
