using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class PersonalBaja
    {
        public string codigoBaja { get; set; }
        public string item { get; set; }
        public string numeroDocumento { get; set; }
        public string itemAutorizado { get; set; }
        public System.DateTime fechaIngreso { get; set; }
        public System.DateTime fechaRetiro { get; set; }
        public string codigoMotivoRetiro { get; set; }
        public decimal ultimaRemuneracion { get; set; }
        public decimal promedioUltimosTresSueldos { get; set; }
        public string bajaPlanillas { get; set; }
        public string transferencia { get; set; }
        public string certificadoTrabajo { get; set; }
        public string beneficiosSociales { get; set; }
        public decimal saldoVacaciones { get; set; }
        public string observaciones { get; set; }
        public System.DateTime fechaSistema { get; set; }
        public string login { get; set; }

        public virtual Personal Personal { get; set; }
    }
}
