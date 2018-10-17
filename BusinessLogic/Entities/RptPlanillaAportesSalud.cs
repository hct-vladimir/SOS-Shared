using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class RptPlanillaAportesSalud
    {
        public string ITEM { get; set; }
        public string COD_ESCALA_SAL { get; set; }
        public string PROGRAMA { get; set; }

        public string NUMERO_DOC { get; set; }
        public string EXT { get; set; }
        public string NOMBRE { get; set; }
        public string FECHA_NAC { get; set; }
        public string SEXO { get; set; }
        public string CARGO { get; set; }
        public string FECHA_INGRESO { get; set; }
        public Nullable<int> DIAS_TRAB { get; set; }
        public int HORA_PAG { get; set; }
        public Nullable<decimal> SUELDO_BASICO { get; set; }
        public Nullable<decimal> BASICO_GANADO { get; set; }
        public Nullable<decimal> BONO_ANTIGUEDAD { get; set; }
        public decimal ASIGNACION_SOS { get; set; }
        public int HORAS_EXTRAS { get; set; }
        public Nullable<decimal> MONTO_PAGADO { get; set; }
        public Nullable<decimal> BONO_PRODUCCION { get; set; }
        public decimal OTROS_BONOS { get; set; }
        public Nullable<decimal> NRO_DIAS_DOMINICALES { get; set; }
        public Nullable<decimal> MONTO_DOMINICALES { get; set; }
        public Nullable<decimal> TOTAL_GANADO { get; set; }
        public Nullable<decimal> AFP { get; set; }
        public Nullable<decimal> AFP_1221 { get; set; }
        public Nullable<decimal> AFP_050 { get; set; }
        public Nullable<decimal> SOLIDARIO { get; set; }
        public Nullable<decimal> RC_IVA { get; set; }
        public Nullable<decimal> DESCUENTOS { get; set; }
        public decimal AMIGO_SOS { get; set; }
        public Nullable<decimal> TOTAL_DESCUENTO { get; set; }
        public Nullable<decimal> LIQUIDO_PAGABLE { get; set; }
        public string numeroCuenta { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string APELLIDO_CASADA { get; set; }
        public string NOMBRES { get; set; }
        public decimal COMISION { get; set; }
        public decimal OTROS_DESCUENTOS { get; set; }
        public string FILIAL { get; set; }
        public string COD_TIPO_SEGURO { get; set; }
        public string TIPO_SEGURO { get; set; }
        public Nullable<decimal> TOTAL_APORTE { get; set; }
        public Nullable<int> CARGA_HORARIA { get; set; }
        public string NUMERO_CUENTA { get; set; }
        public int MES { get; set; }
        public string GESTION { get; set; }
        public string MES_CIERRE { get; set; }
    }
}
