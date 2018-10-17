using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class VPresupuestoCiudad
    {
        public string Ciudad { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string Estado { get; set; }
        public int PresupuestoId { get; set; }
        public int CiudadId { get; set; }
    }
}
