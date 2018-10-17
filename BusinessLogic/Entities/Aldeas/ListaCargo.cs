using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas
{
    public class ListaCargo
    {
        public string codigoCargo { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> idfkArea { get; set; }
        public string estado { get; set; }
        public Nullable<decimal> salarioMinimo { get; set; }
        public Nullable<decimal> salarioMaximo { get; set; }
    }
}
