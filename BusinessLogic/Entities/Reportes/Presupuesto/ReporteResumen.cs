using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes.Presupuesto
{
    public class ReporteResumen
    {
        public Nullable<int> NivelProgramaticoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public Nullable<decimal> SOS { get; set; }
        public Nullable<decimal> RRFF { get; set; }
        public Nullable<decimal> GOB { get; set; }
        public Nullable<decimal> MUN { get; set; }
        public Nullable<decimal> COM { get; set; }
        public Nullable<decimal> IPD { get; set; }
        public Nullable<decimal> Total { get; set; }
    }
}
