using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Resultado
    {
        public Resultado(string mensaje)
        {
            Mensaje = mensaje;
        }

        public string Mensaje { get; set; }
    }
}
