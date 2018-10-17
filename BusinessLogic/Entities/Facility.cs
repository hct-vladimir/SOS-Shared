using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Facility
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public int CiudadId { get; set; }
        //[NotMapped] //PARA OBTENER EL NOMBRE DE LA CIUDAD EN EL LISTADO
        //public List<Ciudad> CiudadList { get; set; }

        [RegularExpression("^[A-ZÑÁÉÍÓÚ a-zñáéíóú 0-9]+$", ErrorMessage = "* .. solo letras y numeros")]
        [StringLength(50), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Codigo { get; set; }

        [RegularExpression("^[-/A-ZÑÁÉÍÓÚ a-zñáéíóú]+$", ErrorMessage = "* .. solo letras")]
        [StringLength(200), Required(ErrorMessage = "El {0} es obligatorio")]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string NombreDespliegue => string.Format("{0} - {1}", Codigo, Nombre);

        public decimal? TotalRecursos { get; set; }

        public string EstadoFacilityNombre { get; set; }
        public bool Compartido { get; set; }
        public int CiudadCompartidaActualId { get; set; }

        public int? TotalComprobantes { get; set; }
        public bool TieneCobertura { get; set; }
        public Nullable<short> TipoFacilityId { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }

        public virtual Ciudad Ciudad { get; set; }
    }
}
