﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class PlantillaCuenta
    {
        public int Id { get; set; }
        public int PlantillaAsientoId { get; set; }
        public int CuentaContableId { get; set; }
        public string Glosa { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public int TerritorioId { get; set; }
        public int PlanProgramaticoId { get; set; }
        public int ContraparteId { get; set; }
        public Nullable<int> AnexoTributarioId { get; set; }
        public Nullable<int> CodigoAuditoriaId { get; set; }
        public Nullable<int> AccionNacionalId { get; set; }
        public string NotasAdicionales { get; set; }
        public bool EsDebe { get; set; }
        public bool EsAjuste { get; set; }
        public Nullable<int> Orden { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual AccionesNacionale AccionesNacionale { get; set; }
        public virtual AnexosTributario AnexosTributario { get; set; }
        public virtual CodigosAuditoria CodigosAuditoria { get; set; }
        public virtual Contraparte Contraparte { get; set; }
        public virtual CuentaContable CuentaContable { get; set; }
        public virtual PlanProgramatico PlanProgramatico { get; set; }
        public virtual PlantillaAsiento PlantillaAsiento { get; set; }
        public virtual Territorio Territorio { get; set; }
    }
}
