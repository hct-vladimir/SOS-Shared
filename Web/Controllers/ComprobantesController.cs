using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;
using Microsoft.Reporting.WebForms;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    [Authorize]
    public class ComprobantesController : Controller
    {
        private ComprobantesManager comprobantesManager = new ComprobantesManager();
        private FacilitiesManager facilitiesManager = new FacilitiesManager();
        private CierresManager cierresManager = new CierresManager();
        private CierresContablesManager cierresContablesManager = new CierresContablesManager();
        private ObservacionesManager observacionesManager = new ObservacionesManager();
        private ReportesManager reportesManager = new ReportesManager();

        // GET Facilities
        public ActionResult Facilities(int ciudadId)
        {
            //Obtener Periodo Contable Activo
            var periodoActivo = cierresContablesManager.GetPeriodoActivo();

            //Si no existe un periodo activo no mostrar nada en la pantalla
            if (periodoActivo == null)
            {
                ViewBag.PeriodoActivo = string.Empty;
                return View(new List<Facility>());
            }

            var cierreContableActual = cierresContablesManager.GetCierreContableByCiudadPeriodo(ciudadId, periodoActivo.Id);
            ViewBag.PeriodoActivoId = periodoActivo.Id;
            ViewBag.PeriodoActivo = string.Format("{0}/{1}",
                cierreContableActual.PeriodoCierre.Mes.ToString().PadLeft(2, '0'),
                cierreContableActual.PeriodoCierre.Gestion);
            ViewBag.EstadoCierre = cierreContableActual.EstadosCierre.Nombre;
            ViewBag.EstadoCierreId = cierreContableActual.EstadoCierreId;
            ViewBag.CiudadOrigenId = ciudadId;

            var facilities = facilitiesManager.GetFacilitiesComprobantesPorCiudad(ciudadId, cierreContableActual.Id);
            return View(facilities);
        }

        // GET: Index
        public ActionResult Index(int facilityId = 0, int ciudadId = 0)
        {
            if (facilityId == 0)
            {
                return RedirectToAction("Facilities");
            }

            //Obtener Periodo Contable Activo
            var periodoActivo = cierresContablesManager.GetPeriodoActivo();
            var cierreContableActual = cierresContablesManager.GetCierreContableByCiudadPeriodo(ciudadId, periodoActivo.Id);

            ViewBag.EstadoCierreContableId = cierreContableActual.EstadoCierreId;

            ViewBag.FacilityActual = facilitiesManager.GetFacility(facilityId);
            ViewBag.CiudadOrigenId = ciudadId;
            return View(comprobantesManager.GetComprobantesPorFacility(facilityId));
        }

        // GET: Comprobantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comprobante comprobante = comprobantesManager.GetComprobante(id.Value);
            if (comprobante == null)
            {
                return HttpNotFound();
            }
            return View(comprobante);
        }

        public ActionResult Cierres()
        {
            var cierres = cierresManager.GetCierresByGestion(2018);
            return View(cierres);
        }

        public ActionResult IniciarPeriodo(int gestion)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var periodo = 1;
            var cierres = cierresManager.GetCierresByGestion(2018).LastOrDefault();
            if (cierres != null)
            {
                periodo = cierres.Periodo == 12 ? 0 : cierres.Periodo + 1;
            }


            var tipoCierreId = (periodo == 0) ? 2 : 1;
            var cierre = new Cierre
            {
                Gestion = gestion,
                Periodo = periodo,
                TipoCierreId = (short)tipoCierreId,
                EstadoCierreId = 1,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cierresManager.InsertCierre(cierre);
            return RedirectToAction("Cierres");
        }

        public ActionResult CerrarPeriodo(int cierreId)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var cierre = new Cierre
            {
                Id = cierreId,
                EstadoCierreId = 2,
                UsuarioModificacion = usuarioActual
            };

            cierresManager.UpdateCierre(cierre);
            return RedirectToAction("Cierres");
        }

        public ActionResult HabilitarPeriodo(int cierreId)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var cierre = new Cierre
            {
                Id = cierreId,
                EstadoCierreId = 3,
                UsuarioModificacion = usuarioActual
            };

            cierresManager.UpdateCierre(cierre);
            return RedirectToAction("Cierres");
        }

        // GET: Comprobantes/Create
        public ActionResult Create(int facilityId, int ciudadId)
        {
            //Obtener Periodo Contable Activo
            var periodoActivo = cierresContablesManager.GetPeriodoActivo();
            var cierreContableActual = cierresContablesManager.GetCierreContableByCiudadPeriodo(ciudadId, periodoActivo.Id);
            ViewBag.CierreContableId = cierreContableActual.Id;
            ViewBag.CiudadOrigenId = ciudadId;

            ViewBag.FechaActual = DateTime.Now.ToString("dd'/'MM'/'yyyy");
            ViewBag.FacilityActual = facilitiesManager.GetFacility(facilityId);
            return View();
        }

        // POST: Comprobantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comprobante comprobante)
        {
            if (ModelState.IsValid)
            {
                comprobantesManager.InsertComprobante(comprobante);
                return RedirectToAction("Index", "CuentasAsientos", new { id = comprobante.Id });
            }

            ViewBag.FechaActual = DateTime.Now.ToString("dd'/'MM'/'yyyy");
            ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");
            return View(comprobante);
        }

        // GET: Comprobantes/Edit/5
        public ActionResult Edit(int? id, int facilityId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comprobante comprobante = comprobantesManager.GetComprobante(id.Value);
            if (comprobante == null)
            {
                return HttpNotFound();
            }

            ViewBag.FacilityActual = facilitiesManager.GetFacility(facilityId);
            ViewBag.FechaActual = DateTime.Now.ToString("dd'/'MM'/'yyyy");
            ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");
            return View(comprobante);
        }

        // POST: Comrprobantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comprobante comprobante)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            comprobante.UsuarioModificacion = usuarioActual;
            comprobantesManager.UpdateComprobante(comprobante);
            return RedirectToAction("Index", new { facilityId = comprobante.FacilityId});

            ////ViewBag.FacilityActual = facilitiesManager.GetFacility(facilityId);
            ////ViewBag.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");
            ////ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");
            ////return View(comprobante);
        }

        public ActionResult Delete(int id, int facilityId)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            comprobantesManager.DeleteComprobante(id, usuarioActual);
            return RedirectToAction("Index", new { facilityId });
        }

        public ActionResult Finalizar(int id)
        {
            comprobantesManager.CambiarEstadoComprobante(id, 2);

            //Si tiene Observaciones el comprobante, darlas por Aprobadas
            var comprobante = comprobantesManager.GetComprobante(id);

            var observaciones = observacionesManager.GetObservacionesPorEntidadFacility(2, comprobante.Id, comprobante.Facility.Id);
            var observacionesNoAprobadas = observaciones.Where(o => !o.Aprobado).ToList();
            if (observacionesNoAprobadas.Any())
            {
                foreach (var observacionAprobada in observacionesNoAprobadas)
                {
                    observacionAprobada.Aprobado = true;
                    observacionesManager.UpdateObservacion(observacionAprobada);
                }
            }

            return RedirectToAction("Index", "CuentasAsientos", new { id });
        }

        public ActionResult Anular(int id, int facilityId)
        {
            comprobantesManager.CambiarEstadoComprobante(id, 4);
            return RedirectToAction("Index", new { facilityId });
        }

        public ActionResult ReporteComprobante(int id)
        {
            string mimeType = string.Empty;
            byte[] renderedBytes = null;
            var comprobanteActual = comprobantesManager.GetComprobante(id);
            comprobanteActual.Importe = comprobanteActual.CuentasAsientos.Sum(c => c.Debe);
            switch (comprobanteActual.TipoComprobanteId)
            {
                case 2:
                    renderedBytes = ObtenerReporteEgreso(comprobanteActual, out mimeType);
                    break;
                case 3:
                    renderedBytes = ObtenerReporteDiario(comprobanteActual, out mimeType);
                    break;
            }

            return File(renderedBytes, contentType: mimeType);
        }

        public byte[] ObtenerReporteEgreso(Comprobante comprobante, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "ComprobanteEgreso.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }

            var cuentaAsiento = comprobante.CuentasAsientos.First();
            var cuentaBanco = comprobante.CuentaBanco;
            var banco = comprobante.CuentaBanco != null ? comprobante.CuentaBanco.Banco : null;
            var tipoComprobante = comprobante.TiposComprobante;
            var reportesComprobante = reportesManager.GetReporteComprobante(comprobante.Id);


            var dataSourceComprobantes = new List<Comprobante> { comprobante };
            var dataSourceTiposComprobantes = new List<TiposComprobante> { tipoComprobante };
            var dataSourceCuentasAsientos = new List<CuentaAsiento> { cuentaAsiento };
            var dataSourceBancos = new List<Banco> { banco };
            var dataSourceCuentasBancos = new List<CuentaBanco> { cuentaBanco };

            ReportDataSource rdsComprobantes = new ReportDataSource("Comprobante", dataSourceComprobantes);
            localReport.DataSources.Add(rdsComprobantes);
            ReportDataSource rdsTiposComprobantes = new ReportDataSource("TipoComprobante", dataSourceTiposComprobantes);
            localReport.DataSources.Add(rdsTiposComprobantes);
            ReportDataSource rdsCuentasAsientos = new ReportDataSource("CuentasAsientos", dataSourceCuentasAsientos);
            localReport.DataSources.Add(rdsCuentasAsientos);
            ReportDataSource rdsBancos = new ReportDataSource("Banco", dataSourceBancos);
            localReport.DataSources.Add(rdsBancos);
            ReportDataSource rdsDetalleComprobante = new ReportDataSource("Datos", reportesComprobante);
            localReport.DataSources.Add(rdsDetalleComprobante);
            ReportDataSource rdsCuentasBancos = new ReportDataSource("CuentasBancos", dataSourceCuentasBancos);
            localReport.DataSources.Add(rdsCuentasBancos);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>21,59in</PageWidth>" +
            "  <PageHeight>27,94in</PageHeight>" +
            "  <MarginTop>0,5in</MarginTop>" +
            "  <MarginLeft>0,5in</MarginLeft>" +
            "  <MarginRight>0,5in</MarginRight>" +
            "  <MarginBottom>0,5in</MarginBottom>" +
            "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return renderedBytes;
        }

        public byte[] ObtenerReporteDiario(Comprobante comprobante, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "ComprobanteDiario.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }

            var cuentaAsiento = comprobante.CuentasAsientos.First();
            var cuentaBanco = comprobante.CuentaBanco;
            var banco = comprobante.CuentaBanco != null ? comprobante.CuentaBanco.Banco : null;
            var tipoComprobante = comprobante.TiposComprobante;
            var reportesComprobante = reportesManager.GetReporteComprobante(comprobante.Id);


            var dataSourceComprobantes = new List<Comprobante> {comprobante};
            var dataSourceTiposComprobantes = new List<TiposComprobante> {tipoComprobante};
            var dataSourceCuentasAsientos = new List<CuentaAsiento> {cuentaAsiento};
            var dataSourceBancos = new List<Banco> {banco};
            var dataSourceCuentasBancos = new List<CuentaBanco> {cuentaBanco};

            ReportDataSource rdsComprobantes = new ReportDataSource("Comprobante", dataSourceComprobantes);
            localReport.DataSources.Add(rdsComprobantes);
            ReportDataSource rdsTiposComprobantes = new ReportDataSource("TipoComprobante", dataSourceTiposComprobantes);
            localReport.DataSources.Add(rdsTiposComprobantes);
            ReportDataSource rdsCuentasAsientos = new ReportDataSource("CuentasAsientos", dataSourceCuentasAsientos);
            localReport.DataSources.Add(rdsCuentasAsientos);
            ReportDataSource rdsBancos = new ReportDataSource("Banco", dataSourceBancos);
            localReport.DataSources.Add(rdsBancos);
            ReportDataSource rdsDetalleComprobante = new ReportDataSource("Datos", reportesComprobante);
            localReport.DataSources.Add(rdsDetalleComprobante);
            ReportDataSource rdsCuentasBancos = new ReportDataSource("CuentasBancos", dataSourceCuentasBancos);
            localReport.DataSources.Add(rdsCuentasBancos);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>21,59in</PageWidth>" +
            "  <PageHeight>27,94in</PageHeight>" +
            "  <MarginTop>0,5in</MarginTop>" +
            "  <MarginLeft>0,5in</MarginLeft>" +
            "  <MarginRight>0,5in</MarginRight>" +
            "  <MarginBottom>0,5in</MarginBottom>" +
            "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return renderedBytes;
        }
    }
}