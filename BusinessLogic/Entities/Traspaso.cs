using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Traspaso
    {
        public int FacilityId { get; set; }
        public int ComprobanteId { get; set; }
        public int CuentaContableId { get; set; }
        public string Glosa { get; set; }
        public decimal Monto { get; set; }
        public bool AnPrograma { get; set; }
        public int CuentaTransitoriaNro { get; set; }
        
        public CuentaTransitoria CuentaTransitoria { get; set; }
    }

    public enum CuentaTransitoria
    {
        Manutencion = 1237,
        Resto = 1238,
        Sueldos = 1239,
        Construccion = 1241
    }
}
