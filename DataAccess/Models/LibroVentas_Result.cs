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
    
    public partial class LibroVentas_Result
    {
        public System.DateTime FechaFactura { get; set; }
        public int NumeroFactura { get; set; }
        public long NumeroAutorizacion { get; set; }
        public bool Activo { get; set; }
        public string Nit { get; set; }
        public string Nombre { get; set; }
        public decimal Importe { get; set; }
        public Nullable<decimal> ImporteImpuestoEsp { get; set; }
        public Nullable<decimal> ImporteExento { get; set; }
        public Nullable<decimal> ImporteTasaCero { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> DebitoFiscal_IVA { get; set; }
        public string CodigoControl { get; set; }
    }
}