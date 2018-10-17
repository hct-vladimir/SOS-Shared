using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class AccionesNacionale
    {
        public int Id { get; set; }
        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú .]+$", ErrorMessage = "* .. solo letras y .")]
        [StringLength(50), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Codigo { get; set; }
        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú .(),-]+$", ErrorMessage = "* .. solo letras y .(),")]
        [StringLength(200), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Descripcion { get; set; }

        public string NombreDespliegue => $"{Codigo} - {Descripcion}";
    }
}
