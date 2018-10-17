using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.ParametrosPlanillas
{
    public class SalarioMinimo
    {
        public short ID { get; set; }
        public decimal SALARIO_MINIMO { get; set; }
        public string FECHA_SALARIO_MINIMO { get; set; }
    }
}
