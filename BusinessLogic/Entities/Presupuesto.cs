using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Presupuesto
    {
        public int Id { get; set; }
        public short TipoPresupuestoId { get; set; }
        public short EstadoPresupuestoId { get; set; }
        public int Gestion { get; set; }
        public short Version { get; set; }
        public string NombreVersion { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual EstadoPresupuesto EstadoPresupuesto { get; set; }

        public virtual TiposPresupuesto TiposPresupuesto { get; set; }
    }
}
