using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class RptPlanillaSueldos
    {
        public int ITEM { get; set; }
        public string NOMBRE { get; set; }
        public string CARGO { get; set; }
        public Nullable<System.DateTime> FECHA_INGRESO { get; set; }
        public Nullable<int> DIAS_PAG { get; set; }
        public Nullable<decimal> HABER_BASICO { get; set; }
        public Nullable<decimal> BASICO_GANADO { get; set; }
        public Nullable<decimal> CAL_BONO_ANTIGUEDAD { get; set; }
        public decimal MOD_ASIGSOS { get; set; }
        public decimal MOD_COMISION { get; set; }
        public Nullable<decimal> CAL_TOTAL_GANADO { get; set; }
        public Nullable<decimal> MOD_RC_IVA { get; set; }
        public Nullable<decimal> RC_IVA { get; set; }
        public Nullable<decimal> CAL_AFP { get; set; }
        public Nullable<decimal> CAL_APO_SOL_ASEG { get; set; }
        public Nullable<decimal> CAL_APONALSOL { get; set; }
        public decimal MOD_PRESTAMOS { get; set; }
        public decimal MOD_OTRS_DESCUENTOS { get; set; }
        public Nullable<decimal> MOD_OTROS_INGRESOS { get; set; }
        public decimal MOD_AMIGOSOS { get; set; }
        public Nullable<decimal> CAL_TOTAL_DESCUENTO { get; set; }
        public Nullable<decimal> CAL_LIQ_PAGABLE { get; set; }
        public string FILIAL { get; set; }
        public string PROGRAMA { get; set; }
        public string NUMERO_DOC { get; set; }
        public string EXPEDIDO { get; set; }
        public string COD_ESCALA_SAL { get; set; }
        public string NUMERO_CUENTA { get; set; }
        public int MES { get; set; }
        public string GESTION { get; set; }
        public string FACI_CODIGO { get; set; }
    }
}
