using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas
{
    public class ListaPrograma
    {
        public string codigoPrograma { get; set; }
        public string codigoTipoPrograma { get; set; }
        public string Descripcion { get; set; }
        public string codigoFilial { get; set; }
        public Nullable<int> idFacility { get; set; }
    }
}
