using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes
{
    public class ReporteBalanceComprobacion
    {
        public Nullable<int> GrupoCuentasNavId { get; set; }
        public Nullable<int> NivelCuentaId { get; set; }
        public string Numero { get; set; }
        public string Nombre { get; set; }
        public Nullable<decimal> DEBITO { get; set; }
        public Nullable<decimal> CREDITO { get; set; }
    }
}
