using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class RetencionVista
    {
        public int ComprobanteId { get; set; }
        public int CuentaContableId { get; set; }
        public string Glosa { get; set; }
        public int TipoRetenidoId { get; set; }
        public int TipoRetencionId { get; set; }
        public decimal Importe { get; set; }
    }
}
