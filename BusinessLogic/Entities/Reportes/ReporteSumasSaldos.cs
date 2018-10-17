using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes
{
    public class ReporteSumasSaldos
    {
        public string Numero_Cuenta { get; set; }
        public string Nombre { get; set; }
        public Nullable<decimal> DEBE_SUMA { get; set; }
        public Nullable<decimal> HABER_SUMA { get; set; }
        public Nullable<decimal> DEBE_SALDO { get; set; }
        public Nullable<decimal> HABER_SALDO { get; set; }
    }
}
