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
    
    public partial class ESTADOS_PLANILLAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ESTADOS_PLANILLAS()
        {
            this.PERIODOS_PLANILLAS = new HashSet<PERIODOS_PLANILLAS>();
            this.RETROACTIVOS = new HashSet<RETROACTIVO>();
        }
    
        public short Id { get; set; }
        public string Nombre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERIODOS_PLANILLAS> PERIODOS_PLANILLAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RETROACTIVO> RETROACTIVOS { get; set; }
    }
}
