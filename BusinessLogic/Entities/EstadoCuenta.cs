using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class EstadoCuenta
    {
        public int ComprobanteId { get; set; }
        public int CuentaContableEstadoId { get; set; }
        public int CuentaContableId { get; set; }
        public short CiudadId { get; set; }
        public string CodigoFacility { get; set; }
        public string Glosa { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public CuentaAsiento CuentaAsiento { get; set; }


        public bool EstadoCuentaRelacionado { get; set; }
        public int EstadoCuentaRelacionadoId { get; set; }
        public int ComprobanteRelacionadoId { get; set; }
        public int CuentaAsientoRelacionadaId { get; set; }

        public int Id { get; set; }
        public int TipoEstadoCuentaId { get; set; }
        public Nullable<int> DebeCiudadId { get; set; }
        public Nullable<int> DebeFacilityId { get; set; }
        public Nullable<int> DebeCuentaAsientoId { get; set; }
        public Nullable<int> HaberCiudadId { get; set; }
        public Nullable<int> HaberFacilityId { get; set; }
        public Nullable<int> HaberCuentaAsientoId { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual Ciudad CiudadDebe { get; set; }
        public virtual Ciudad CiudadHaber { get; set; }
        public virtual CuentaAsiento CuentasAsientoDebe { get; set; }
        public virtual CuentaAsiento CuentasAsientoHaber { get; set; }
        public virtual Facility FacilityDebe { get; set; }
        public virtual Facility FacilityHaber { get; set; }
    }
}
