using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Planilla
    {
        public int Id { get; set; }
        public int Item { get; set; }
        public string NombreColaborador { get; set; }
        public string NombreCargo { get; set; }
        public string DocumentoIdentidad { get; set; }
        public int DiasTrabajados { get; set; }
        public int Faltas { get; set; }
        public decimal RcIva { get; set; }
        public decimal Comision { get; set; }
        public decimal AmigosSos { get; set; }
        public decimal Prestamos { get; set; }
        public decimal OtrosDescuentos { get; set; }
        public decimal OtrosIngresos { get; set; }
        public decimal MontoNeto { get; set; }
        public decimal AsignacionSos { get; set; }
    }
}
