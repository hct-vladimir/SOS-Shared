using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class PlanillaSueldo
    {
        public string GESTION { get; set; }
        public int MES { get; set; }
        public int ITEM { get; set; }
        public Nullable<System.DateTime> FECHA_INGRESO { get; set; }
        public Nullable<int> DIAS_PAG { get; set; }
        public Nullable<decimal> HABER_BASICO { get; set; }
        public Nullable<decimal> BASICO_GANADO { get; set; }
        public Nullable<decimal> CAL_BONO_ANTIGUEDAD { get; set; }
        public Nullable<decimal> MOD_RC_IVA { get; set; }
        public Nullable<decimal> MOD_RC_IVA_2 { get; set; }
        public decimal MOD_COMISION { get; set; }
        public decimal MOD_AMIGOSOS { get; set; }
        public decimal MOD_PRESTAMOS { get; set; }
        public decimal MOD_OTRS_DESCUENTOS { get; set; }
        public Nullable<decimal> MOD_OTROS_INGRESOS { get; set; }
        public Nullable<decimal> MOD_MONTO_NETO { get; set; }
        public decimal MOD_ASIGSOS { get; set; }
        public Nullable<decimal> CAL_TOTAL_GANADO { get; set; }
        public Nullable<decimal> CAL_APONALSOL { get; set; }
        public Nullable<decimal> CAL_AFP { get; set; }
        public Nullable<decimal> CAL_APO_SOL_ASEG { get; set; }
        public Nullable<decimal> CAL_TOTAL_DESCUENTO { get; set; }
        public Nullable<decimal> CAL_LIQ_PAGABLE { get; set; }
        public Nullable<System.DateTime> FEC_CIERRE { get; set; }
        public string MES_CIERRE { get; set; }
        public Nullable<decimal> CAL_ANTIGUEDAD_REAL { get; set; }
        public Nullable<int> COTIZANTE { get; set; }
        public Nullable<System.DateTime> FECHA_NACIMIENTO { get; set; }
        public Nullable<decimal> CAL_APONACSOL_1 { get; set; }
        public Nullable<decimal> CAL_APONACSOL_2 { get; set; }
        public Nullable<decimal> CAL_APONACSOL_3 { get; set; }
        public Nullable<decimal> CAL_IMPUESTO_RETENIDO { get; set; }
        public string ESTADO { get; set; }
        public Nullable<decimal> SALARIO { get; set; }
        public Nullable<int> EDAD { get; set; }
    }
}
