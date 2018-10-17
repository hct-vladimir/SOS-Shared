using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class TraspasosPrograma
    {
        public int Id { get; set; }
        public Nullable<int> ComprobanteProgramaId { get; set; }
        public Nullable<int> ComprobanteANId { get; set; }
        public string CuentaTransitoriaNumero { get; set; }
        public decimal MontoTraspaso { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual Comprobante ComprobanteAN { get; set; }
        public virtual Comprobante ComprobanteProg { get; set; }
    }
}
