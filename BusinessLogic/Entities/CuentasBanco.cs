using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class CuentasBanco
    {
        public int Id { get; set; }

        [Range(1, 9999)]
        public int BancoId { get; set; }
        

        [Range(1, 9999)]
        public int FacilityId { get; set; }
       

        [StringLength(200), Required]
        public string FichaBanco { get; set; }
        [StringLength(100), Required]
        public string Contacto { get; set; }

        public double SaldoBs { get; set; }

        public double SaldoDl { get; set; }

        [StringLength(100), Required]
        public string NumeroCuenta { get; set; }

        [StringLength(200), Required]
        public string Alias { get; set; }

        [NotMapped] //PARA OBTENER EL NOMBRE DEl BANCO EN EL LISTADO
        public List<Banco> BancoList { get; set; }

        [NotMapped] //PARA OBTENER EL NOMBRE DEl BANCO EN EL LISTADO
        public List<Facility> FacilityList { get; set; }
    }
}
