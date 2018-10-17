using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Comprobante
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public int TipoComprobanteId { get; set; }
        public Nullable<int> CuentaBancoId { get; set; }
        public string NumeroComprobante { get; set; }
        public System.DateTime FechaComprobante { get; set; }
        public string Beneficiario { get; set; }
        public string Glosa { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<decimal> TipoCambio { get; set; }
        public short TipoMonedaId { get; set; }
        public string NumeroCheque { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public int CiudadId { get; set; }
        public short EstadoComprobanteId { get; set; }
        public string Observaciones { get; set; }
        public int CierreContableId { get; set; }

        public virtual CierreContable CierreContable { get; set; }
        public virtual EstadosComprobante EstadosComprobante { get; set; }
        public virtual CuentaBanco CuentaBanco { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual TiposComprobante TiposComprobante { get; set; }
        public virtual TipoMoneda TiposMoneda { get; set; }
        public virtual Ciudad Ciudad { get; set; }
        public virtual List<CuentaAsiento> CuentasAsientos { get; set; }

    }
}
