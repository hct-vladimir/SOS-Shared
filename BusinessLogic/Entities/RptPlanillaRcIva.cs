﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class RptPlanillaRcIva
    {
        public Nullable<int> ITEM { get; set; }
        public string NOMBRE { get; set; }
        public Nullable<decimal> CAL_SUELDO_NETO { get; set; }
        public Nullable<decimal> CAL_MIN_NO_IMPONIBLE { get; set; }
        public Nullable<decimal> CAL_SUJETO_IMP { get; set; }
        public Nullable<decimal> CAL_IMPUESTO { get; set; }
        public Nullable<decimal> MOD_COMPUTO_IVA { get; set; }
        public Nullable<decimal> CAL_POR_NRO_SMN { get; set; }
        public Nullable<decimal> CAL_SALDO_FISCO { get; set; }
        public Nullable<decimal> CAL_A_FAVOR_DEP { get; set; }
        public decimal CAL_SLD_MES_ANT { get; set; }
        public Nullable<decimal> CAL_ACT_MES_ANT { get; set; }
        public Nullable<decimal> CAL_VALOR_ACT { get; set; }
        public Nullable<decimal> CAL_SLD_TOTAL_DEP { get; set; }
        public Nullable<decimal> CAL_SLD_UTILIZADO { get; set; }
        public Nullable<decimal> CAL_IMP_RETENIDO { get; set; }
        public Nullable<decimal> CAL_SLD_FAVOR_PROX_MES { get; set; }
        public decimal TOTAL_GANADO { get; set; }
        public string CI { get; set; }
        public string EXTENDIDO { get; set; }
        public string NROCUENTA { get; set; }
        public string FILIAL { get; set; }
        public string PROGRAMA { get; set; }
        public string SECCION { get; set; }
        public string MES_CIERRE { get; set; }
    }
}
