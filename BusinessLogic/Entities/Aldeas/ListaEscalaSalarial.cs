using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas
{
    public class ListaEscalaSalarial
    {
        public string codigoEscalaSalarial { get; set; }
        public string descripcion { get; set; }
        public Nullable<decimal> haberBasico { get; set; }

        public string EscalaDescripcion => string.Format("{1} - {0} ", descripcion, haberBasico.Value.ToString("N2"));
    }
}
