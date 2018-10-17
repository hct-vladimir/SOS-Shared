using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes
{
    public class ReporteComprobante
    {
        public int IdCuentaAsiento { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public string Glosa_CuentasAsientos { get; set; }
        public int CuentaContableId { get; set; }
        public int IdCuentaContable { get; set; }
        public string Nombre { get; set; }
        public string Numero { get; set; }
        public string Descripcion_CuentaContable { get; set; }
        public int IdComprobante { get; set; }
        public string Beneficiario { get; set; }
        public int FacilityId { get; set; }
        public string Glosa_comprobante { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public string NumeroCheque { get; set; }
        public string Cod_Facility { get; set; }
        public string Nombre_Facility { get; set; }
        public int IdCodAuditoria { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
