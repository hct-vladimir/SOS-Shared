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
    
    public partial class ListaCargo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ListaCargo()
        {
            this.Personals = new HashSet<Personal>();
        }
    
        public string codigoCargo { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> idfkArea { get; set; }
        public string estado { get; set; }
        public Nullable<decimal> salarioMinimo { get; set; }
        public Nullable<decimal> salarioMaximo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Personal> Personals { get; set; }
    }
}