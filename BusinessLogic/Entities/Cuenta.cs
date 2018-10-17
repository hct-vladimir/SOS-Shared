using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Cuenta
    {
        public int Id { get; set; }
        public Nullable<int> CuentaNavId { get; set; }
        public string Numero { get; set; }
        public string Aporte { get; set; }
        public string Nombre { get; set; }
    }
}
