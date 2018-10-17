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
    
    public partial class CuentaBanco
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CuentaBanco()
        {
            this.Comprobantes = new HashSet<Comprobante>();
        }
    
        public int Id { get; set; }
        public int BancoId { get; set; }
        public int FacilityId { get; set; }
        public string FichaBanco { get; set; }
        public string Contacto { get; set; }
        public decimal SaldoBs { get; set; }
        public decimal SaldoDl { get; set; }
        public string NumeroCuenta { get; set; }
        public string Alias { get; set; }
    
        public virtual Banco Banco { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comprobante> Comprobantes { get; set; }
        public virtual Facility Facility { get; set; }
    }
}