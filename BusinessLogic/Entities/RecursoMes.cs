using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class RecursoMes
    {
        public int Id { get; set; }
        public int RecursoId { get; set; }
        public Nullable<decimal> Enero { get; set; }
        public Nullable<decimal> Febrero { get; set; }
        public Nullable<decimal> Marzo { get; set; }
        public Nullable<decimal> Abril { get; set; }
        public Nullable<decimal> Mayo { get; set; }
        public Nullable<decimal> Junio { get; set; }
        public Nullable<decimal> Julio { get; set; }
        public Nullable<decimal> Agosto { get; set; }
        public Nullable<decimal> Septiembre { get; set; }
        public Nullable<decimal> Octubre { get; set; }
        public Nullable<decimal> Noviembre { get; set; }
        public Nullable<decimal> Diciembre { get; set; }
        public Nullable<decimal> CoberturaEnero { get; set; }
        public Nullable<decimal> CoberturaFebrero { get; set; }
        public Nullable<decimal> CoberturaMarzo { get; set; }
        public Nullable<decimal> CoberturaAbril { get; set; }
        public Nullable<decimal> CoberturaMayo { get; set; }
        public Nullable<decimal> CoberturaJunio { get; set; }
        public Nullable<decimal> CoberturaJulio { get; set; }
        public Nullable<decimal> CoberturaAgosto { get; set; }
        public Nullable<decimal> CoberturaSeptiembre { get; set; }
        public Nullable<decimal> CoberturaOctubre { get; set; }
        public Nullable<decimal> CoberturaNoviembre { get; set; }
        public Nullable<decimal> CoberturaDiciembre { get; set; }
        public string TipoPresupuesto { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }

        public virtual Recurso Recurso { get; set; }
    }
}
