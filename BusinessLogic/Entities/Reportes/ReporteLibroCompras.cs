using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes
{
    public class ReporteLibroCompras
    {
        public System.DateTime FechaFactura { get; set; }
        public string Nit { get; set; }
        public string Nombre { get; set; }
        public int NumeroFactura { get; set; }
        public long NumeroAutorizacion { get; set; }
        public decimal Importe { get; set; }
        public Nullable<decimal> TotalNoCreditoFiscal { get; set; }
        public Nullable<decimal> ImporteTotal { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> CreditoFiscal_IVA { get; set; }
        public string CodigoControl { get; set; }
    }
}
