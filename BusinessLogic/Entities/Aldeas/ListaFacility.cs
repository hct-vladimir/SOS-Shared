using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas
{
    public class ListaFacility
    {
        public int id { get; set; }
        public string codigoFacility { get; set; }
        public string ciudad { get; set; }
        public string proyecto { get; set; }
        public string nombre { get; set; }
        public string Descripcion => string.Format("{0} - {1}", codigoFacility, nombre);
    }
}
