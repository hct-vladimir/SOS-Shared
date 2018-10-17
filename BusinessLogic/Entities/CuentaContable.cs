using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class CuentaContable
    {
        public int Id { get; set; }
        public int CuentaNavisionId { get; set; }
        public byte TipoCuentaId { get; set; }
        public int ContraparteId { get; set; }
        public string Numero { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public string NombreDespliegue => string.Format("{0} - {1}", Numero, Nombre);

        public virtual Contraparte Contraparte { get; set; }
        public virtual CuentaNavision CuentaNavision { get; set; }
        public virtual TiposCuenta TiposCuenta { get; set; }
    }
}
