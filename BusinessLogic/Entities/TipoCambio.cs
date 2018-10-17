using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class TipoCambio
    {
        public int Id { get; set; }
        public short TipoMonedaId { get; set; }
        public decimal Valor { get; set; }
        public System.DateTime FechaTipoCambio { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual TipoMoneda TipoMoneda { get; set; }
    }
}
