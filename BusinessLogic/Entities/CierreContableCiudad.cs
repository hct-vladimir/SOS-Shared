using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class CierreContableCiudad
    {
        public int CiudadId { get; set; }
        public string Ciudad { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public string Estado { get; set; }
        public int CierreContableId { get; set; }
        public short PeriodoCierreId { get; set; }
    }
}
