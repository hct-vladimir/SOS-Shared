using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;
using PagedList;
using Microsoft.Reporting.WebForms;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    [Authorize]
    public class PlanillasController : Controller
    {
        private PlanillasManager planillasManager = new PlanillasManager();
        private PeriodosPlanillasManager periodosPlanillasManager = new PeriodosPlanillasManager();
        private RetroactivosManager retroactivosManager = new RetroactivosManager();

        // GET: Planillas
        public ActionResult Index(int? pagina, int gestion, int mes, int? item, string nombres, string cinumero, int? cargoId)
        {
            var numeroPagina = pagina ?? 1;

            var planillas = BuscarPlanilla(mes, gestion, item, nombres, cinumero, cargoId);
            var model = new PagedList<RptPlanillaSueldos>(planillas, numeroPagina, 20);

            @ViewBag.MesPlanilla = mes;
            @ViewBag.GestionPlanilla = gestion;

            return View(model);
        }

        private List<RptPlanillaSueldos> BuscarPlanilla(int mes, int gestion, int? item, string nombres, string cinumero, int? cargoId)
        {
            var resultado = planillasManager.GetPlanillaSueldos(mes, gestion);

            if (item != null)
            {
                resultado = resultado.Where(c => c.ITEM == item).ToList();
            }

            if (!string.IsNullOrEmpty(nombres))
            {
                resultado = resultado.Where(c => c.NOMBRE.Contains(nombres)).ToList();
            }

            if (!string.IsNullOrEmpty(cinumero))
            {
                resultado = resultado.Where(c => c.NUMERO_DOC.Contains(cinumero)).ToList();
            }

            if (cargoId != null)
            {
                resultado = resultado.Where(c => c.CARGO == cargoId.Value.ToString()).ToList();
            }

            return resultado.OrderBy(c => c.ITEM).ToList();
        }


        public ActionResult Gestion()
        {
            var periodos = periodosPlanillasManager.GetAllPeriodosPlanillas();
            var ultimoPeriodo = periodos.Max(p => p.Mes);
            ViewBag.NuevoPeriodo = ultimoPeriodo + 1;
            ViewBag.GestionActual = DateTime.Now.Year;

            var retroactivos = retroactivosManager.GetAllRetroactivos();
            ViewBag.Retroactivos = retroactivos;

            return View(periodos);
        }

        public ActionResult GenerarRetroactivo(Retroactivo retroactivo)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            retroactivo.UsuarioCreacion = usuarioActual;
            retroactivo.UsuarioModificacion = usuarioActual;
            var resultado = retroactivosManager.InsertRetroactivo(retroactivo);


            return View("Retroactivos", new { id = retroactivo.Id });
        }

        public ActionResult Retroactivos(int id)
        {
            return View();
        }

        public ActionResult CerrarRetroactivo(int id)
        {
            var resultado = retroactivosManager.CerrarRetroactivo(id);

            return RedirectToAction("Gestion");
        }


        public ActionResult CerrarPeriodo(int gestion, int mes)
        {
            var resultado = periodosPlanillasManager.CerrarPeriodoPlanilla(gestion, mes);

            var fechaProceso = new DateTime(gestion, mes + 2, 1).AddDays(-1);
            var resultadoParametros = planillasManager.SetFechaProceso(fechaProceso);

            var resultadoNuevoPeriodo = periodosPlanillasManager.CrearPeriodoPlanilla(gestion, fechaProceso.Month);

            var resultadoSueldos = planillasManager.GenerarPlanillaSueldo();
            var resultadoRcIva = planillasManager.GenerarPlanillaRcIva();

            return RedirectToAction("Gestion");
        }

        public ActionResult Generar(int gestion, int mes)
        {
            var resultado = planillasManager.GenerarPlanillaSueldo();

            var resultadoRcIva = planillasManager.GenerarPlanillaRcIva();


            return RedirectToAction("Index", new { gestion, mes });
        }

        public ActionResult Actualizar(int gestion, int mes)
        {
            var resultado = planillasManager.ProcesarPlanillaSueldo();

            var resultadoRcIva = planillasManager.ProcesarPlanillaRcIva();


            return RedirectToAction("Index", new { gestion, mes });
        }

        [HttpPost]
        public ActionResult Importar(HttpPostedFileBase archivo)
        {
            // Obtener Periodo de Planilla Actual
            var fechaProceso = planillasManager.GetFechaProceso();
            if (archivo != null && archivo.ContentLength > 0)
            {

                var rutaArchivoImportado = Path.Combine(Server.MapPath("~/Imports"), Path.GetFileName(archivo.FileName));
                archivo.SaveAs(rutaArchivoImportado);

                var resultado = planillasManager.ImportarPlanillaSueldo(fechaProceso.Month, fechaProceso.Year, rutaArchivoImportado);
            }


            return RedirectToAction("Index", new { gestion = fechaProceso.Year, mes = fechaProceso.Month });
        }


        public ActionResult Sueldos(int gestion, int mes)
        {
            var model = planillasManager.GetPlanillaSueldos(mes, gestion);

            @ViewBag.MesPlanilla = mes.ToString().PadLeft(2, '0');
            @ViewBag.GestionPlanilla = gestion;

            return View(model);
        }

        /* Reportes */
        public ActionResult PlanillaSueldosGeneral()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P01-PlanillaSueldosGeneral.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaSueldosGeneral(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-PlanillaSueldosGeneral-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        public ActionResult PlanillaSueldos()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P02-PlanillaSueldos.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaSueldos(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-PlanillaSueldos-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de sueldos mensuales en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaSueldosMensuales()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaSueldosMensuales(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planilla sueldos mensuales
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaSueldosMensuales(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaSueldosMensuales.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillaSueldosmensuales = planillasManager.GetPlanillaSueldos(mes, gestion);

            ReportDataSource rdsPlanillaSueldomensual = new ReportDataSource("reporteplanilla", reportePlanillaSueldosmensuales);
            localReport.DataSources.Add(rdsPlanillaSueldomensual);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>21in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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

        public ActionResult PlanillaRcIva()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P03-PlanillaRcIva.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaRcIva(templateDocument, fechaProceso.ToString("yyyyMM"));

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-PlanillaRcIva-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de RcIva en pdf con respeto al mescierre
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaRcIva()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaRcIva(fechaProceso.ToString("yyyyMM"), out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planillaRcIva
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaRcIva(string mesCierre, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaRcIva.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillaRcIva = planillasManager.GetPlanillaRcIva(mesCierre);

            ReportDataSource rdsPlanillaRcIva = new ReportDataSource("ReporteIva", reportePlanillaRcIva);
            localReport.DataSources.Add(rdsPlanillaRcIva);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>18in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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

        public ActionResult PlanillaAfpFuturo()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P04-PlanillaAfpFuturo.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaAfpFuturo(templateDocument, fechaProceso.ToString("yyyyMM"));

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-PlanillaAfpFuturo-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de afpfuturo en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaAfpFuturo()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaAfpFuturo(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planillaafpfuturo
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaAfpFuturo(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaAfpFuturo.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillaAfpFuturo = planillasManager.GetPlanillaAfpFuturo(mes, gestion);

            ReportDataSource rdsPlanillaAfpFuturo = new ReportDataSource("datosfuturo", reportePlanillaAfpFuturo);
            localReport.DataSources.Add(rdsPlanillaAfpFuturo);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>17in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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

        public ActionResult PlanillaAfpPrevision()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P05-PlanillaAfpPrevision.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaAfpPrevision(templateDocument, fechaProceso.ToString("yyyyMM"));

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-PlanillaAfpPrevision-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de afpprevision en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaAfpPrevision()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaAfpPrevision(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planillaafpprevision
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaAfpPrevision(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaAfpPrevision.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillaAfpPrevision = planillasManager.GetPlanillaAfpPrevision(mes, gestion);

            ReportDataSource rdsPlanillaAfpPrevision = new ReportDataSource("datosprevision", reportePlanillaAfpPrevision);
            localReport.DataSources.Add(rdsPlanillaAfpPrevision);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>17in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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

        /// <summary>
        /// Planilla de Aportes Salud Cochabamba
        /// </summary>
        /// <returns></returns>
        public ActionResult PlanillaSalud()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P06-PlanillaSalud.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaSalud(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-AportesSaludCochabamba-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de salud oruro en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaSaludLp()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaSaludLp(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planillasalud
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaSaludLp(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaSaludLp.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillSalud = planillasManager.GetReportePlanillaSaludLaPaz(mes, gestion);

            ReportDataSource rdsPlanillaSalud = new ReportDataSource("reporteplanilla", reportePlanillSalud);
            localReport.DataSources.Add(rdsPlanillaSalud);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>18in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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
        public ActionResult PlanillaSaludLp()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P06-PlanillaSaludLP.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaSaludLp(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-AportesSaludLaPaz-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de salud oruro en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaSaludOr()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaSaludOr(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planillasalud
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaSaludOr(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaSaludOr.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillSalud = planillasManager.GetReportePlanillaSaludOruro(mes, gestion);

            ReportDataSource rdsPlanillaSalud = new ReportDataSource("reporteplanilla", reportePlanillSalud);
            localReport.DataSources.Add(rdsPlanillaSalud);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>18in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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
        public ActionResult PlanillaSaludOr()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P06-PlanillaSaludOR.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaSaludOr(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-AportesSaludOruro-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de salud oruro en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaSaludSc()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaSaludSc(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planillasalud
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaSaludSc(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaSaludSc.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillSalud = planillasManager.GetReportePlanillaSaludSantaCruz(mes, gestion);

            ReportDataSource rdsPlanillaSalud = new ReportDataSource("reporteplanilla", reportePlanillSalud);
            localReport.DataSources.Add(rdsPlanillaSalud);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>20in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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

        public ActionResult PlanillaSaludSc()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P06-PlanillaSaludSC.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaSaludSc(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-AportesSaludSantaCruz-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de salud oruro en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaSaludSr()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaSaludSr(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planillasalud
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaSaludSr(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaSaludSr.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillSalud = planillasManager.GetReportePlanillaSaludSucre(mes, gestion);

            ReportDataSource rdsPlanillaSalud = new ReportDataSource("reporteplanilla", reportePlanillSalud);
            localReport.DataSources.Add(rdsPlanillaSalud);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>18in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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

        public ActionResult PlanillaSaludSr()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P06-PlanillaSaludSR.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaSaludSr(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-AportesSaludSucre-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de salud oruro en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaSaludTj()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaSaludTj(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planillasalud
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaSaludTj(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaSaludTj.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillSalud = planillasManager.GetReportePlanillaSaludTarija(mes, gestion);

            ReportDataSource rdsPlanillaSalud = new ReportDataSource("reporteplanilla", reportePlanillSalud);
            localReport.DataSources.Add(rdsPlanillaSalud);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>26in</PageWidth>" +
            "  <PageHeight>15in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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

        public ActionResult PlanillaSaludTj()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P06-PlanillaSaludTJ.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaSaludTj(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-AportesSaludTarija-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        /// <summary>
        /// ECC 13/10/18 obtener planilla de aportes de salud en pdf con respeto al mes y gestion
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportePlanillaAportesMensualesSalud()
        {
            string mimeType;
            byte[] renderedBytes = null;
            var fechaProceso = planillasManager.GetFechaProceso();
            renderedBytes = ObtenerReportePlanillaAportesMensualesSalud(fechaProceso.Month, fechaProceso.Year, out mimeType);
            return File(renderedBytes, contentType: mimeType);
        }

        /// <summary>
        /// ECC 13/10/18 metodo de obtener el pdf de planilla de aportes de salud
        /// </summary>
        /// <returns></returns>
        public byte[] ObtenerReportePlanillaAportesMensualesSalud(int mes, int gestion, out string mimeType)
        {
            mimeType = string.Empty;

            LocalReport localReport = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports/Planillas"), "ReportePlanillaAportesMensualesSalud.rdlc");
            if (System.IO.File.Exists(path))
            {
                localReport.ReportPath = path;
            }
            else
            {
                throw new ApplicationException("La ruta al reporte no existe, favor verificar.");
            }
            var reportePlanillaAporteMensualSalud = planillasManager.GetPlanillaAporteMensualSalud(mes, gestion);

            ReportDataSource rdsPlanillaAporteMensualSalud = new ReportDataSource("reporteplanilla", reportePlanillaAporteMensualSalud);
            localReport.DataSources.Add(rdsPlanillaAporteMensualSalud);

            string reportType = "PDF";
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>16in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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
        public ActionResult PlanillaMinisterio()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/P07-PlanillaMinisterio.xlsx");

            var fechaProceso = planillasManager.GetFechaProceso();
            var report = planillasManager.GetReportePlanillaMinisterio(templateDocument, fechaProceso.Month, fechaProceso.Year);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-PlanillaMinisterio-{0}{1}.xlsx", fechaProceso.Year, fechaProceso.Month.ToString().PadLeft(2, '0')));
        }

        // GET: Planillas/Edit/5
        public ActionResult Edit(int item)
        {
            var fechaProceso = planillasManager.GetFechaProceso();
            var planilla = planillasManager.GetPlanillaSueldosByItem(item, fechaProceso.Month, fechaProceso.Year);

            var planillas = planillasManager.GetPlanillaSueldos(fechaProceso.Month, fechaProceso.Year);
            ViewBag.Planillas = planillas;
            ViewBag.MesPlanilla = fechaProceso.ToString("MM/yyyy");

            return View(planilla);
        }

        // POST: Planillas/Edit/5
        [HttpPost]
        public ActionResult Edit(PlanillaSueldo planilla)
        {
            try
            {
                var resultado = planillasManager.UpdatePlanillaSueldo(planilla);

                ////var resultadoProcesoRcIva = planillasManager.ProcesarPlanillaRcIva();
                ////var resultadoProcesoSueldos = planillasManager.ProcesarPlanillaSueldo();

                return Json(resultado);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index");
            }
        }



    }
}
