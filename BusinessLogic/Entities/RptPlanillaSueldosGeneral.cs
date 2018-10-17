﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class RptPlanillaSueldosGeneral
    {
        public int ITEM { get; set; }
        public System.DateTime FECHA_NACIMIENTO { get; set; }
        public string SEXO { get; set; }
        public Nullable<int> MES { get; set; }
        public string GESTION { get; set; }
        public string NOMBRE { get; set; }
        public string CARGO { get; set; }
        public string NUMERO_DOC { get; set; }
        public string EXPEDIDO { get; set; }
        public Nullable<System.DateTime> FECHA_INGRESO { get; set; }
        public Nullable<int> DIAS_PAG { get; set; }
        public string NUMERO_CUENTA { get; set; }
        public Nullable<decimal> HABER_BASICO { get; set; }
        public Nullable<decimal> BASICO_GANADO { get; set; }
        public string COD_FILIAL { get; set; }
        public string FILIAL { get; set; }
        public string PROGRAMA { get; set; }
        public string COD_SECCION { get; set; }
        public string SECCION { get; set; }
        public Nullable<decimal> CAL_BONO_ANTIGUEDAD { get; set; }
        public Nullable<decimal> MOD_RC_IVA { get; set; }
        public decimal MOD_COMISION { get; set; }
        public decimal MOD_AMIGOSOS { get; set; }
        public decimal MOD_PRESTAMOS { get; set; }
        public decimal MOD_OTRS_DESCUENTOS { get; set; }
        public Nullable<decimal> CAL_TOTAL_GANADO { get; set; }
        public Nullable<decimal> CAL_APONALSOL { get; set; }
        public Nullable<decimal> CAL_AFP { get; set; }
        public Nullable<decimal> CAL_APO_SOL_ASEG { get; set; }
        public string COD_ESCALA_SAL { get; set; }
        public Nullable<decimal> CAL_IMPUESTO_RETENIDO { get; set; }
        public string CODIGO_FACILITY { get; set; }
        public string NOM_FACILITY { get; set; }
        public string NOM_FACPROYECTO { get; set; }
        public string AREA { get; set; }
        public Nullable<decimal> CAL_APONACSOL_1 { get; set; }
        public Nullable<decimal> CAL_APONACSOL_2 { get; set; }
        public Nullable<decimal> CAL_APONACSOL_3 { get; set; }
        public int ITEM_2 { get; set; }
        public string NACIONALIDAD { get; set; }
        public string TIPO_SEGURO { get; set; }
        public string COTIZANTE { get; set; }
        public string TIPO_AFP { get; set; }
    }
}
