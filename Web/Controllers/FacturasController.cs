using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class FacturasController : Controller
    {
        private FacturasManager facturasManager = new FacturasManager();
        private CuentasAsientosManager cuentasAsientosManager = new CuentasAsientosManager();
        private ComprobantesManager comprobantesManager = new ComprobantesManager();
        private RetencionesManager retencionesManager = new RetencionesManager();

        // GET: Facturas
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Comprobantes");
        }

        // POST: Factura
        [HttpPost]
        public ActionResult Create(Factura factura)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var importeDefinitivo = factura.Descuento != null && factura.Descuento.Value > 0
                ? factura.Importe - factura.Descuento.Value
                : factura.Importe;

            var cuentaAsientoGastos = new CuentaAsiento
            {
                ComprobanteId = factura.ComprobanteId,
                CuentaContableId = factura.CuentaContableId,
                Glosa = factura.Glosa,
                Debe = factura.NoGravada ? importeDefinitivo : (importeDefinitivo * 0.87M),
                Haber = 0,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = true,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoGastos);

            if (!factura.NoGravada)
            {
                var cuentaAsientoIva = new CuentaAsiento
                {
                    ComprobanteId = factura.ComprobanteId,
                    CuentaContableId = 765,
                    Glosa = factura.Glosa,
                    Debe = (importeDefinitivo * 0.13M),
                    Haber = 0,
                    TerritorioId = 1,
                    PlanProgramaticoId = 6,
                    ContraparteId = 1,
                    EsDebe = true,
                    EsAjuste = false,
                    UsuarioCreacion = usuarioActual,
                    UsuarioModificacion = usuarioActual
                };

                cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoIva); 
            }

            var cuentaAsientoCaja = new CuentaAsiento
            {
                ComprobanteId = factura.ComprobanteId,
                CuentaContableId = 775,
                Glosa = factura.Glosa,
                Debe = 0,
                Haber = importeDefinitivo,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = false,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoCaja);

            factura.CuentaAsientoId = cuentaAsientoGastos.Id;
            factura.UsuarioCreacion = usuarioActual;
            factura.UsuarioModificacion = usuarioActual;
            facturasManager.InsertFactura(factura);

            return RedirectToAction("Index", "CuentasAsientos", new { id = factura.ComprobanteId});
        }

        [HttpPost]
        public ActionResult Retencion(RetencionVista retencion)
        {
            switch (retencion.TipoRetenidoId)
            {
                case 1:
                    GenerarRetenido(retencion);
                    break;
                case 2:
                    GenerarGrossingUp(retencion);
                    break;
            }

            return RedirectToAction("Index", "CuentasAsientos", new { id = retencion.ComprobanteId });
        }

        
        /// <summary>
        /// Genera asiento con retención IUE
        /// </summary>
        /// <param name="retencion"></param>
        private void GenerarRetenido(RetencionVista retencion)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var comprobanteActual = comprobantesManager.GetComprobante(retencion.ComprobanteId);

            var retencionIt = (retencion.Importe * 0.03M);
            var retencionIue = retencion.TipoRetencionId == 1 ? (retencion.Importe * 0.125M) : (retencion.Importe * 0.05M);
            var retencionRcIva = retencion.Importe * 0.13M;
            var retencionIueBe = retencion.Importe * 0.125M;

            var anexoTributarioId = 0;
            var importeRetencion = 0M;
            var importeRetencionIt = 0M;
            var importeCajaBancos = 0M;
            switch (retencion.TipoRetencionId)
            {
                case 1:
                case 2:
                    anexoTributarioId = 10;
                    importeRetencion = retencionIue;
                    importeRetencionIt = retencionIt;
                    importeCajaBancos = retencion.Importe - retencionIt - retencionIue;
                    break;
                case 3:
                    anexoTributarioId = 8;
                    importeRetencion = retencionRcIva;
                    importeRetencionIt = retencionIt;
                    importeCajaBancos = retencion.Importe - retencionIt - retencionRcIva;
                    break;
                case 4:
                    anexoTributarioId = 8;
                    importeRetencion = retencionRcIva;
                    importeCajaBancos = retencion.Importe - retencionRcIva;
                    break;
                case 5:
                    anexoTributarioId = 11;
                    importeRetencion = retencionIueBe;
                    importeCajaBancos = retencion.Importe - retencionIueBe;
                    break;
            }

            var cuentaAsientoPersonalizada = new CuentaAsiento
            {
                ComprobanteId = retencion.ComprobanteId,
                CuentaContableId = retencion.CuentaContableId,
                Glosa = retencion.Glosa,
                Debe = retencion.Importe,
                Haber = 0,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                AnexoTributarioId = anexoTributarioId,
                EsDebe = true,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoPersonalizada);

            if (retencion.TipoRetencionId == 1 || retencion.TipoRetencionId == 2 || retencion.TipoRetencionId == 3)
            {
                var cuentaAsientoIt = new CuentaAsiento
                {
                    ComprobanteId = retencion.ComprobanteId,
                    CuentaContableId = 1280,
                    Glosa = retencion.Glosa,
                    Debe = 0,
                    Haber = retencionIt,
                    TerritorioId = 1,
                    PlanProgramaticoId = 6,
                    ContraparteId = 1,
                    AnexoTributarioId = 9, //Retención IT
                    EsDebe = false,
                    EsAjuste = false,
                    UsuarioCreacion = usuarioActual,
                    UsuarioModificacion = usuarioActual
                };

                cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoIt); 
            }

            if (retencion.TipoRetencionId == 1 || retencion.TipoRetencionId == 2)
            {
                var cuentaAsientoIue = new CuentaAsiento
                {
                    ComprobanteId = retencion.ComprobanteId,
                    CuentaContableId = 1281,
                    Glosa = retencion.Glosa,
                    Debe = 0,
                    Haber = retencionIue,
                    TerritorioId = 1,
                    PlanProgramaticoId = 6,
                    ContraparteId = 1,
                    AnexoTributarioId = anexoTributarioId,
                    EsDebe = false,
                    EsAjuste = false,
                    UsuarioCreacion = usuarioActual,
                    UsuarioModificacion = usuarioActual
                };

                cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoIue);
            }


            if (retencion.TipoRetencionId == 3 || retencion.TipoRetencionId == 4)
            {
                var cuentaAsientoRcIva = new CuentaAsiento
                {
                    ComprobanteId = retencion.ComprobanteId,
                    CuentaContableId = 1283,
                    Glosa = retencion.Glosa,
                    Debe = 0,
                    Haber = retencionRcIva,
                    TerritorioId = 1,
                    PlanProgramaticoId = 6,
                    ContraparteId = 1,
                    AnexoTributarioId = anexoTributarioId,
                    EsDebe = false,
                    EsAjuste = false,
                    UsuarioCreacion = usuarioActual,
                    UsuarioModificacion = usuarioActual
                };

                cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoRcIva);
            }

            if (retencion.TipoRetencionId == 5)
            {
                var cuentaAsientoIueBe = new CuentaAsiento
                {
                    ComprobanteId = retencion.ComprobanteId,
                    CuentaContableId = 1282,
                    Glosa = retencion.Glosa,
                    Debe = 0,
                    Haber = retencionIueBe,
                    TerritorioId = 1,
                    PlanProgramaticoId = 6,
                    ContraparteId = 1,
                    AnexoTributarioId = anexoTributarioId,
                    EsDebe = false,
                    EsAjuste = false,
                    UsuarioCreacion = usuarioActual,
                    UsuarioModificacion = usuarioActual
                };

                cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoIueBe);
            }

            var cuentaAsientoCaja = new CuentaAsiento
            {
                ComprobanteId = retencion.ComprobanteId,
                CuentaContableId = 775,
                Glosa = retencion.Glosa,
                Debe = 0,
                Haber = importeCajaBancos,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                AnexoTributarioId = anexoTributarioId,
                EsDebe = false,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoCaja);

            // Insertar Objeto Retencion
            var retencionEntidad = new Retencion
            {
                ComprobanteId = retencion.ComprobanteId,
                FechaComprobante = comprobanteActual.FechaComprobante,
                TipoRetencionId = (short)retencion.TipoRetencionId,
                ImporteRetencion = importeRetencion,
                ImporteRetencionIT = importeRetencionIt,
                Retenido = true,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            retencionesManager.InsertRetencion(retencionEntidad);

        }


        /// <summary>
        /// Genera asiento con Grossing Up (Retención asumida por la institución)
        /// </summary>
        /// <param name="retencion"></param>
        private void GenerarGrossingUp(RetencionVista retencion)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var importeGrossingUp = retencion.TipoRetencionId == 1
                ? retencion.Importe + (retencion.Importe * 15.5M) / 84.5M
                : retencion.Importe + (retencion.Importe * 8.5M) / 91.5M;

            var retencionIt = (importeGrossingUp * 0.03M);
            var retencionIue = retencion.TipoRetencionId == 1
                ? (importeGrossingUp * 0.125M)
                : (importeGrossingUp * 0.05M);

            var cuentaAsientoPersonalizada = new CuentaAsiento
            {
                ComprobanteId = retencion.ComprobanteId,
                CuentaContableId = retencion.CuentaContableId,
                Glosa = retencion.Glosa,
                Debe = importeGrossingUp,
                Haber = 0,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = true,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoPersonalizada);

            if (retencion.TipoRetencionId == 1 || retencion.TipoRetencionId == 2 || retencion.TipoRetencionId == 3)
            {
                var cuentaAsientoIt = new CuentaAsiento
                {
                    ComprobanteId = retencion.ComprobanteId,
                    CuentaContableId = 1280,
                    Glosa = retencion.Glosa,
                    Debe = 0,
                    Haber = retencionIt,
                    TerritorioId = 1,
                    PlanProgramaticoId = 6,
                    ContraparteId = 1,
                    EsDebe = false,
                    EsAjuste = false,
                    UsuarioCreacion = usuarioActual,
                    UsuarioModificacion = usuarioActual
                };

                cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoIt);
            }

            if (retencion.TipoRetencionId == 1 || retencion.TipoRetencionId == 2)
            {
                var cuentaAsientoIue = new CuentaAsiento
                {
                    ComprobanteId = retencion.ComprobanteId,
                    CuentaContableId = 1281,
                    Glosa = retencion.Glosa,
                    Debe = 0,
                    Haber = retencionIue,
                    TerritorioId = 1,
                    PlanProgramaticoId = 6,
                    ContraparteId = 1,
                    EsDebe = false,
                    EsAjuste = false,
                    UsuarioCreacion = usuarioActual,
                    UsuarioModificacion = usuarioActual
                };

                cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoIue);
            }

            if (retencion.TipoRetencionId == 3 || retencion.TipoRetencionId == 4)
            {
                var cuentaAsientoIue = new CuentaAsiento
                {
                    ComprobanteId = retencion.ComprobanteId,
                    CuentaContableId = 1283,
                    Glosa = retencion.Glosa,
                    Debe = 0,
                    Haber = retencionIue,
                    TerritorioId = 1,
                    PlanProgramaticoId = 6,
                    ContraparteId = 1,
                    EsDebe = false,
                    EsAjuste = false,
                    UsuarioCreacion = usuarioActual,
                    UsuarioModificacion = usuarioActual
                };

                cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoIue);
            }

            var cuentaAsientoCaja = new CuentaAsiento
            {
                ComprobanteId = retencion.ComprobanteId,
                CuentaContableId = 775,
                Glosa = retencion.Glosa,
                Debe = 0,
                Haber = retencion.Importe,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = false,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaAsientoCaja);
        }
    }
}