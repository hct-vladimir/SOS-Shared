using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Contraparte
    {
        public int Id { get; set; }

        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú 0-9]+$", ErrorMessage = "* .. solo letras y numeros")]
        [StringLength(50), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Codigo { get; set; }

        [RegularExpression("^[-/A-ZÑÁÉÍÓÚ a-zñáéíóú ]+$", ErrorMessage = "* .. solo letras")]
        [StringLength(200), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Nombre { get; set; }

        public string NombreDespliegue => $"{Codigo} - {Nombre}";
    }
}
