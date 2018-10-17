using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Cierre
    {
        public int Id { get; set; }
        public short TipoCierreId { get; set; }
        public short EstadoCierreId { get; set; }
        public int Periodo { get; set; }
        public int Gestion { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual EstadosCierre EstadosCierre { get; set; }
        public virtual TiposCierre TiposCierre { get; set; }
    }
}
