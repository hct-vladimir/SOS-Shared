using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Observacion
    {
        public int Id { get; set; }
        public short TipoObservacionId { get; set; }
        public Nullable<int> EntidadId { get; set; }
        public Nullable<int> CiudadId { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string Descripcion { get; set; }
        public string FilasObservadas { get; set; }
        public bool EsNacional { get; set; }
        public bool Aprobado { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public bool EsCompartido { get; set; }
        public int CiudadOrigenId { get; set; }

        public virtual TipoObservacion TipoObservacion { get; set; }
    }
}
