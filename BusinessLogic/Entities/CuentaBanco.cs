using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class CuentaBanco
    {

        public int Id { get; set; }
        [Range(1, 9999)]
        public int BancoId { get; set; }
        [Range(1, 9999)]
        public int FacilityId { get; set; }

        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú .]+$", ErrorMessage = "* .. solo letras y .")]
        [StringLength(200), Required(ErrorMessage = "El {0} es obligatorio")]
        public string FichaBanco { get; set; }
        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú .]+$", ErrorMessage = "* .. solo letras y .")]
        [StringLength(100), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Contacto { get; set; }
       
        public decimal SaldoBs { get; set; }
        public decimal SaldoDl { get; set; }
        [RegularExpression("^[0-9 -]*$", ErrorMessage = "* .. solo numeros y -")]
        [StringLength(100), Required(ErrorMessage = "El {0} es obligatorio")]
        public string NumeroCuenta { get; set; }

        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú .]+$", ErrorMessage = "* .. solo letras y .")]
        [StringLength(200), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Alias { get; set; }

        public virtual Banco Banco { get; set; }
    }
}
