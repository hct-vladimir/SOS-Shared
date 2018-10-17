using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class PeriodoPlanilla
    {
        public int Id { get; set; }
        public short Gestion { get; set; }
        public short Mes { get; set; }
        public short EstadoPlanillaId { get; set; }
        public bool EsRetroactivo { get; set; }
        public Nullable<int> RetroactivoId { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual EstadoPlanilla EstadoPlanilla { get; set; }
    }
}
