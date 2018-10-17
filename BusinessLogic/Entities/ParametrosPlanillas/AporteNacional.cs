using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.ParametrosPlanillas
{
    public class AporteNacional
    {
        public short ID { get; set; }
        public decimal INTERVALO_INICIAL { get; set; }
        public decimal INTERVALO_FINAL { get; set; }
        public decimal PORCENTAJE { get; set; }
        public short TIPO { get; set; }
    }
}
