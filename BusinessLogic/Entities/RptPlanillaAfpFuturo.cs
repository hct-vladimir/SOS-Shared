﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class RptPlanillaAfpFuturo
    {
        public int ITEM { get; set; }
        public string CI { get; set; }
        public string EXT { get; set; }
        public string NOMBRE { get; set; }
        public string CARGO { get; set; }
        public Nullable<int> DIAS_TRAB { get; set; }
        public string FECHA_INGRESO { get; set; }
        public Nullable<decimal> TOTAL_GANADO { get; set; }
        public string NOVEDAD { get; set; }
        public string FECHANOVEDAD { get; set; }
        public Nullable<decimal> AFP_12_21 { get; set; }
        public Nullable<decimal> AFP_SOL_5 { get; set; }
        public Nullable<decimal> APO_SOL_1 { get; set; }
        public Nullable<decimal> APO_SOL_5 { get; set; }
        public Nullable<decimal> APO_SOL_10 { get; set; }
        public Nullable<decimal> APO_PAT { get; set; }
        public Nullable<decimal> APO_PRO_VIV { get; set; }
        public Nullable<decimal> APO_PAT_SOL { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string APELLIDO_CASADA { get; set; }
        public string NOMBRES { get; set; }
        public string NUA { get; set; }
        public string FILIAL { get; set; }
        public string TIPO_DOC { get; set; }
        public string PROGRAMA { get; set; }
        public string MES_CIERRE { get; set; }
        public int MES { get; set; }
        public string GESTION { get; set; }
    }
}
