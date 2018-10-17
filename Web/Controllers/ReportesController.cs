using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class ReportesController : Controller
    {
        ReportesManager reportesManager = new ReportesManager();
        CuentasContablesManager cuentasContablesManager = new CuentasContablesManager();
        FacilitiesManager facilitiesManager = new FacilitiesManager();
        RetencionesManager retencionesManager = new RetencionesManager();

        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Mayores(int? numeroCuenta)
        {
            ViewBag.NumeroCuenta = new SelectList(cuentasContablesManager.GetAllCuentasContables(), "Numero", "NombreDespliegue");

            var reporte = numeroCuenta != null ? reportesManager.GetReporteMayores(numeroCuenta.Value) : new List<ReporteMayores>();
            return View(reporte);
        }

        public ActionResult EstadoCuentas(int? tipoEstadoCuentaId, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var reporte = new List<ReporteBalanceComprobacion>();
            var nombreEstadoCuenta = string.Empty;
            if (tipoEstadoCuentaId != null)
            {
                switch (tipoEstadoCuentaId.Value)
                {
                    case 1:
                        nombreEstadoCuenta = "Cuentas por Pagar";
                        reporte = reportesManager.GetReporteEstadoCuentas("92000", "92999",
                            DateTime.ParseExact("20180101", "yyyyMMdd", null),
                            DateTime.ParseExact("20181231", "yyyyMMdd", null));
                        break;
                    case 2:
                        nombreEstadoCuenta = "Cuentas por Cobrar";
                        reporte = reportesManager.GetReporteEstadoCuentas("06000", "06999",
                            DateTime.ParseExact("20180101", "yyyyMMdd", null),
                            DateTime.ParseExact("20181231", "yyyyMMdd", null));
                        break;
                    case 3:
                        nombreEstadoCuenta = "Padrinos";
                        reporte = reportesManager.GetReporteEstadoCuentas("92110", "92110",
                            DateTime.ParseExact("20180101", "yyyyMMdd", null),
                            DateTime.ParseExact("20181231", "yyyyMMdd", null));
                        break;

                }
            }
            @ViewBag.TipoEstadoCuentaNombre = nombreEstadoCuenta;
            return View(reporte);
        }

        public ActionResult LibroComprasVentas(int? mes, int? gestion)
        {

            ViewBag.LibroCompras = reportesManager.GetReporteLibroCompras(mes != null ? mes.Value : 7, gestion != null ? gestion.Value : 2018);
            ViewBag.LibroVentas = reportesManager.GetReporteLibroVentas(mes != null ? mes.Value : 7, gestion != null ? gestion.Value : 2018);
            return View();
        }

        public ActionResult Retenciones(int? mes, int? gestion)
        { 

            var retenciones = new List<Retencion>();
            var totalRetenciones = 0M;
            if (mes != null && gestion != null)
            {
                retenciones = retencionesManager.GetRetencionesByMesGestion(mes.Value, gestion.Value);
                totalRetenciones = retenciones.Sum(r => r.ImporteTotal);
            }

            ViewBag.TotalRetenciones = totalRetenciones;
            return View(retenciones);
        }

        public ActionResult BalanceComprobacion()
        {
            ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");

            var reporte =
                reportesManager.GetReporteBalanceComprobacion(DateTime.ParseExact("20180101", "yyyyMMdd", null), DateTime.ParseExact("20181231", "yyyyMMdd", null));
            return View(reporte);
        }

        public ActionResult BalanceGeneral()
        {
            ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");

            var reporte = reportesManager.GetReporteBalance();
            return View(reporte);
        }

        public ActionResult EstadoResultados(int? numeroCuenta)
        {
            ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");

            var reporte = reportesManager.GetReporteResultados();
            return View(reporte);
        }


        // GET: Reportes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reportes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reportes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reportes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reportes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reportes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
