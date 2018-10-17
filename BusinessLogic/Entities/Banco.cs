using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Banco
    {
        public int Id { get; set; }

        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú .]+$", ErrorMessage = "* .. solo letras y .")]
        [StringLength(200), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Nombre { get; set; }

        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú 0-9]+$", ErrorMessage = "* .. solo letras y numeros")]
        [StringLength(10), Required(ErrorMessage = "El {0} es obligatorio")]
        public string CodigoPais { get; set; }

        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú 0-9]+$", ErrorMessage = "* .. solo letras y numeros")]
        [StringLength(50), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Iban { get; set; }

        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú 0-9]+$", ErrorMessage = "* .. solo letras y numeros")]
        [StringLength(50), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Swift { get; set; }

        [RegularExpression("^[0-9 -]*$", ErrorMessage = "* .. solo numeros y -")]
        [StringLength(30), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Telefono { get; set; }
    }
}
