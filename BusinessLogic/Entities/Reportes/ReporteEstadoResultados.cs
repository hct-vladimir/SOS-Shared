using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes
{
    public class ReporteEstadoResultados
    {
        public int Gestion { get; set; }
        public int Periodo { get; set; }
        public short TipoReporte { get; set; } // 1. Cierre Mensual 2. Cierre Anual 3. Consulta

        public DateTime FechaInicio { get; set; }
        public DateTime FechaCierre { get; set; }

        // Exedente/déficit neto
        public decimal TotalNetoConImpuesto => TotalNetoSinImpuesto + GastoPorImpuesto;

        public decimal TotalNetoSinImpuesto => TotalRecursosEntrantes + TotalGastos + GananciaPerdidaTipoCambio;

        // Ingresos
        public decimal TotalRecursosEntrantes => IngresoGastosManutencion + IngresoGc + IngresoRecaudacionFondos + IngresoSubsidios + IngresoOperativo + OtroIngreso;
        public decimal IngresoGastosManutencion { get; set; }
        public decimal IngresoGc { get; set; }
        public decimal IngresoRecaudacionFondos { get; set; }
        public decimal IngresoSubsidios { get; set; }
        public decimal IngresoOperativo { get; set; }
        public decimal OtroIngreso { get; set; }

        // Gastos
        public decimal TotalGastos => GastosPrograma + GastosAdministrativos + GastosRecaudacionFondos + OtrosGastos;
        public decimal GastosPrograma { get; set; }
        public decimal GastosAdministrativos { get; set; }
        public decimal GastosRecaudacionFondos { get; set; }
        public decimal OtrosGastos { get; set; }

        // Excedentes/déficit
        public decimal GananciaPerdidaTipoCambio => IngresoIntereses + GastoIntereses + OtroIngresoFinanciero;
        public decimal IngresoIntereses { get; set; }
        public decimal GastoIntereses { get; set; }
        public decimal OtroIngresoFinanciero { get; set; }

        public decimal GastoPorImpuesto { get; set; }
    }
}
