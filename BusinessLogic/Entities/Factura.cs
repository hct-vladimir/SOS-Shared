using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Factura
    {
        public int Id { get; set; }
        public int CuentaAsientoId { get; set; }
        public int FacturaCompradorId { get; set; }
        public long NumeroAutorizacion { get; set; }
        public int NumeroFactura { get; set; }
        public System.DateTime FechaFactura { get; set; }
        public decimal Importe { get; set; }
        public string CodigoControl { get; set; }
        public Nullable<decimal> ImporteImpuestoEsp { get; set; }
        public Nullable<decimal> ImporteExento { get; set; }
        public Nullable<decimal> ImporteTasaCero { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public string Placa { get; set; }
        public string LibroCV { get; set; }
        public Nullable<decimal> TipoCambio { get; set; }
        public short TipoMonedaId { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual FacturaComprador FacturaComprador { get; set; }
        public virtual TipoMoneda TipoMoneda { get; set; }

        [NotMapped]
        public int ComprobanteId { get; set; }

        [NotMapped]
        public string Glosa { get; set; }

        [NotMapped]
        public int CuentaContableId { get; set; }

        public bool NoGravada { get; set; }
    }
}
