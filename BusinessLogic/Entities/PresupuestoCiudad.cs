using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class PresupuestoCiudad
    {
        public int Id { get; set; }
        public int PresupuestoId { get; set; }
        public int CiudadId { get; set; }
        public short EstadoPresupuestoId { get; set; }
        public decimal Monto { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual Ciudad Ciudad { get; set; }
        public virtual EstadoPresupuesto EstadoPresupuesto { get; set; }
        public virtual Presupuesto Presupuesto { get; set; }
    }
}
