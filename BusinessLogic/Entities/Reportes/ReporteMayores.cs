using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes
{
    public class ReporteMayores
    {
        public string Numero { get; set; }
        public string Glosa { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public decimal debe { get; set; }
        public decimal haber { get; set; }
        public Nullable<decimal> Saldo { get; set; }
    }
}
