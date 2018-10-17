using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes
{
    public class ReporteBalanceGeneral
    {
        public int Gestion { get; set; }
        public int Periodo { get; set; }
        public short TipoReporte { get; set; } // 1. Cierre Mensual 2. Cierre Anual 3. Consulta

        public DateTime FechaInicio { get; set; }
        public DateTime FechaCierre { get; set; }

        // Activos
        public decimal TotalActivos => TotalActivosCorrientes + TotalActivosNoCorrientes;

        public decimal TotalActivosNoCorrientes => Intangibles + BienesEquipamiento + PropiedadInversion + OtrosActivosLargoPlazo + InversionesEmpresas + Biologicos;
        public decimal Intangibles { get; set; }
        public decimal BienesEquipamiento { get; set; }
        public decimal PropiedadInversion { get; set; }
        public decimal OtrosActivosLargoPlazo { get; set; }
        public decimal InversionesEmpresas { get; set; }
        public decimal Biologicos { get; set; }

        public decimal TotalActivosCorrientes => Inventarios + CuentasPorCobrar + Efectivo + OtrosActivosCortoPlazo;
        public decimal Inventarios { get; set; }
        public decimal CuentasPorCobrar { get; set; }
        public decimal Efectivo { get; set; }
        public decimal OtrosActivosCortoPlazo { get; set; }

        //Fondos Acumulados y Pasivos
        public decimal TotalFondosYPasivos => TotalFondosAcumulados + TotalPasivosCorrientes + TotalPasivosNoCorrientes + TransferenciasInternas;

        public decimal TotalFondosAcumulados => FondosNoRestringidos + FondosRestringidos + ResultadoAnteriorGestion + ResultadoGestionActual;
        public decimal FondosNoRestringidos { get; set; }
        public decimal FondosRestringidos { get; set; }
        public decimal ResultadoAnteriorGestion { get; set; }
        public decimal ResultadoGestionActual { get; set; }

        public decimal TotalPasivosNoCorrientes => FondosAdministracionFiduciaria + ProvisionesLargoPlazo;
        public decimal FondosAdministracionFiduciaria { get; set; }
        public decimal ProvisionesLargoPlazo { get; set; }

        public decimal TotalPasivosCorrientes => OtrosPasivosCortoPlazo + IngresoDiferido + PasivoFondosRestringidos;
        public decimal OtrosPasivosCortoPlazo { get; set; }
        public decimal IngresoDiferido { get; set; }
        public decimal PasivoFondosRestringidos { get; set; }

        public decimal TransferenciasInternas { get; set; }
    }
}
