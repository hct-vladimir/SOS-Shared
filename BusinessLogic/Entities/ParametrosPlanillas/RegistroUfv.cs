using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.ParametrosPlanillas
{
    public class RegistroUfv
    {
        public int ID { get; set; }
        public decimal UFV_ACTUAL { get; set; }
        public decimal UFV_ANTERIOR { get; set; }
        public string FECHA_UFV { get; set; }
    }
}
