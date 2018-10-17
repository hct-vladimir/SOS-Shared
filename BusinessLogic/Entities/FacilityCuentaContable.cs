using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class FacilityCuentaContable
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public int CuentaContableId { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual CuentaContable CuentaContable { get; set; }
        public virtual Facility Facility { get; set; }
    }
}
