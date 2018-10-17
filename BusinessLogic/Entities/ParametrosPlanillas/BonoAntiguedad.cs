using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.ParametrosPlanillas
{
    public class BonoAntiguedad
    {
        public short ID { get; set; }
        public int ANIOS_INICIAL { get; set; }
        public int ANIOS_FINAL { get; set; }
        public decimal PORCENTAJE { get; set; }
    }
}
