using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class PlanProgramatico : IComparable<PlanProgramatico>
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Comentario { get; set; }
        public int NivelProgramaticoId { get; set; }
        public Nullable<int> PlanProgramaticoId { get; set; }
        public bool Seleccionable { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public string NombreDespliegue => string.Format("{0} - {1}", Codigo, Descripcion);

        public string NombreLista => NivelProgramaticoId == 4 ? string.Format("{1} {0} -  {2}", PlanProgramaticoParent.PlanProgramaticoParent.Descripcion , Codigo, Descripcion) : string.Format("{0} {1}", Codigo, Descripcion);

        public virtual NivelProgramatico NivelProgramatico { get; set; }
        public virtual PlanProgramatico PlanProgramaticoParent { get; set; }


        public int CompareTo(PlanProgramatico other)
        {
            return Codigo.CompareTo(other.Codigo);
        }
    }
}
