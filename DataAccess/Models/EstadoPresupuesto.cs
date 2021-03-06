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
    
    public partial class EstadoPresupuesto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EstadoPresupuesto()
        {
            this.PresupuestosCiudades = new HashSet<PresupuestoCiudad>();
            this.PresupuestosFacilities = new HashSet<PresupuestoFacility>();
            this.Presupuestos = new HashSet<Presupuesto>();
            this.PresupuestosFacilitiesCompartidos = new HashSet<PresupuestoFacilityCompartido>();
        }
    
        public short Id { get; set; }
        public string Nombre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PresupuestoCiudad> PresupuestosCiudades { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PresupuestoFacility> PresupuestosFacilities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Presupuesto> Presupuestos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PresupuestoFacilityCompartido> PresupuestosFacilitiesCompartidos { get; set; }
    }
}
