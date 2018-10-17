using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Retencion
    {
        public int Id { get; set; }
        public Nullable<int> ComprobanteId { get; set; }
        public Nullable<System.DateTime> FechaComprobante { get; set; }
        public Nullable<short> TipoRetencionId { get; set; }
        public Nullable<decimal> ImporteRetencion { get; set; }
        public Nullable<decimal> ImporteRetencionIT { get; set; }
        public Nullable<bool> Retenido { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual TiposRetencion TiposRetencion { get; set; }

        [NotMapped]
        public decimal ImporteTotal => ImporteRetencion.Value + ImporteRetencionIT.Value;

    }
}
