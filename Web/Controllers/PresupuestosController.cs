using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes.Presupuesto;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    [Authorize]
    public class PresupuestosController : Controller
    {
        PresupuestosManager presupuestosManager = new PresupuestosManager();
        ObservacionesManager observacionesManager = new ObservacionesManager();
        UsuariosManager usuariosManager = new UsuariosManager();
        ReportesManager reportesManager = new ReportesManager();
        RecursosMesesManager recursosMesesManager = new RecursosMesesManager();
        FacilitiesManager facilitiesManager = new FacilitiesManager();
        PlanProgramaticoManager planProgramaticoManager = new PlanProgramaticoManager();
        CuentasContablesManager cuentasContablesManager = new CuentasContablesManager();
        CiudadesManager ciudadesManager = new CiudadesManager();

        public ActionResult Index()
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var presupuestos = new List<PresupuestoCiudad>();

            ViewBag.TotalPresupuesto = 0;

            return View(presupuestos);
        }

        // GET: Presupuestos
        public ActionResult Gestion()
        {
            var presupuestos = new List<Presupuesto>();
            presupuestos = presupuestosManager.GetAllPresupuestos();

            return View(presupuestos);
        }

        public ActionResult Ciudades(int presupuestoId)
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.PresupuestoActual = presupuestoActual;

            var presupuestosCiudades = new List<PresupuestoCiudad>();
            if (HttpContext.User.IsInRole("PPTO-CLR"))
            {
                presupuestosCiudades = presupuestosManager.GetPresupuestosCiudades(presupuestoId);
            }
            else
            {
                var usuarioActual = HttpContext.User.Identity.Name;
                var usuario = usuariosManager.GetUsuarioByLogin(usuarioActual);
                presupuestosCiudades = presupuestosManager.GetPresupuestosCiudadesByUsuario(presupuestoId, usuario.Id);
            }

            return View(presupuestosCiudades);
        }

        public ActionResult Resumen(int? ciudadId, int? facilityId, int? nivelId)
        {
            ViewBag.CiudadId = new SelectList(ciudadesManager.GetAllCiudades(), "Id", "Nombre");
            ViewBag.FacilityId = ciudadId != null
                ? new SelectList(facilitiesManager.GetFacilitiesPorCiudad(ciudadId.Value), "Id", "NombreDespliegue")
                : new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");

            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.CiudadOrigenId = ciudadId;

            var resumen = reportesManager.GetReportePresupuestoResumen(presupuestoActual.Id, ciudadId, facilityId);


            var nivelesIds = new List<int>();
            if (resumen.Count > 0 && nivelId != null && nivelId.Value > 0)
            {
                switch (nivelId.Value)
                {
                    case 1:
                        nivelesIds = new List<int> { 1 };
                        break;
                    case 2:
                        nivelesIds = new List<int> { 1, 2 };
                        break;
                    case 3:
                        nivelesIds = new List<int> { 1, 2, 3 };
                        break;
                }
                resumen = resumen.Where(r => nivelesIds.Contains(r.NivelProgramaticoId.Value)).ToList();
            }

            var codigosGastos = new List<string> { "10000", "20000", "30000", "40000" };
            var resumenGastos = resumen.Where(r => r.NivelProgramaticoId == 1 && codigosGastos.Contains(r.Codigo)).ToList();
            var resumenIngresos = resumen.Where(r => r.NivelProgramaticoId == 1 && r.Codigo.StartsWith("5")).ToList();

            var totalGastos = resumenGastos.Sum(r => r.Total);
            var totalIngresos = resumenIngresos.Sum(r => r.Total);
            var totalSubsidio = totalGastos + totalIngresos;

            ViewBag.TotalGasto = totalGastos;
            ViewBag.TotalIngreso = totalIngresos;
            ViewBag.TotalSubsidio = totalSubsidio;

            return View(resumen);
        }

        public ActionResult ExportarResumen(int? ciudadId, int? facilityId, int? nivelId)
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            var resumen = reportesManager.GetReportePresupuestoResumen(presupuestoActual.Id, ciudadId, facilityId);

            var nivelesIds = new List<int>();
            if (resumen.Count > 0 && nivelId != null && nivelId.Value > 0)
            {
                switch (nivelId.Value)
                {
                    case 1:
                        nivelesIds = new List<int> { 1 };
                        break;
                    case 2:
                        nivelesIds = new List<int> { 1, 2 };
                        break;
                    case 3:
                        nivelesIds = new List<int> { 1, 2, 3 };
                        break;
                }
                resumen = resumen.Where(r => nivelesIds.Contains(r.NivelProgramaticoId.Value)).ToList();
            }

            var codigosGastos = new List<string> { "10000", "20000", "30000", "40000" };
            var resumenGastos = resumen.Where(r => r.NivelProgramaticoId == 1 && codigosGastos.Contains(r.Codigo)).ToList();
            var resumenIngresos = resumen.Where(r => r.NivelProgramaticoId == 1 && r.Codigo.StartsWith("5")).ToList();

            var totalGastos = resumenGastos.Sum(r => r.Total);
            var totalIngresos = resumenIngresos.Sum(r => r.Total);
            var totalSubsidio = totalGastos + totalIngresos;

            // Exportar Excel
            string templateDocument =
                            System.Web.HttpContext.Current.Server.MapPath("~/Templates/I03-ReporteResumen.xlsx");

            var report = reportesManager.ExportReportePresupuestoResumen(templateDocument, resumen, totalGastos, totalIngresos, totalSubsidio);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteResumen-{0}{1}{2}.xlsx", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0')));
        }

        public ActionResult ReporteGeneral(int? ciudadId, int? facilityId,
            int? planProgramaticoId, int? cuentaContableId, int? contraparteId, int? codigoAuditoriaId, int? accionNacionalId, int? territorioId,
            string descripcion, string notasAdicionales)
        {
            var listaPlanProgramatico = planProgramaticoManager.GetAllPlan();

            if (facilityId != null)
            {
                listaPlanProgramatico = planProgramaticoManager.GetPlanByFacility(facilityId.Value);
            }

            // Parámetros
            ViewBag.CiudadId = new SelectList(ciudadesManager.GetAllCiudades(), "Id", "Nombre");
            ViewBag.FacilityId = ciudadId != null ?
                new SelectList(facilitiesManager.GetFacilitiesPorCiudad(ciudadId.Value), "Id", "NombreDespliegue") :
                new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");

            ViewBag.PlanProgramaticoId = new SelectList(listaPlanProgramatico, "Id", "NombreLista", new object(), planProgramaticoManager.GetParentsPlanIds());
            ViewBag.CuentaContableId = new SelectList(cuentasContablesManager.GetAllCuentasContables(), "Id", "NombreDespliegue");
            ViewBag.ContraparteId = new SelectList(new ContrapartesManager().GetAllContrapartes(), "Id", "NombreDespliegue");
            ViewBag.CodigoAuditoriaId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosAuditoria(), "Id", "Descripcion");
            ViewBag.AccionNacionalId = new SelectList(new AccionesNacionalesManager().GetAllAccionesNacionales(), "Id", "NombreDespliegue");
            ViewBag.TerritorioId = new SelectList(new TerritoriosManager().GetAllTerritorios(), "Id", "NombreDespliegue");

            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.CiudadOrigenId = ciudadId;

            var reporte = reportesManager.GetReportePresupuestoGeneral(presupuestoActual.Id, ciudadId, facilityId);

            //Filtros
            reporte = FiltrarReporteGeneral(reporte, planProgramaticoId, cuentaContableId, contraparteId, codigoAuditoriaId, accionNacionalId, territorioId, descripcion, notasAdicionales);

            ViewBag.TotalReporte = reporte.Sum(r => r.Monto);
            return View(reporte);
        }

        public ActionResult ExportarReporteGeneral(int? ciudadId, int? facilityId,
            int? planProgramaticoId, int? cuentaContableId, int? contraparteId, int? codigoAuditoriaId, int? accionNacionalId, int? territorioId,
            string descripcion, string notasAdicionales)
        {

            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.CiudadOrigenId = ciudadId;

            var reporte = reportesManager.GetReportePresupuestoGeneral(presupuestoActual.Id, ciudadId, facilityId);

            //Filtros
            reporte = FiltrarReporteGeneral(reporte, planProgramaticoId, cuentaContableId, contraparteId, codigoAuditoriaId, accionNacionalId, territorioId, descripcion, notasAdicionales);


            // Exportar Excel
            string templateDocument =
                            System.Web.HttpContext.Current.Server.MapPath("~/Templates/I01-ReporteGeneral.xlsx");

            var report = reportesManager.ExportReportePresupuestoGeneral(templateDocument, reporte);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteGeneral-{0}{1}{2}.xlsx", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0')));

        }

        public ActionResult ExportarReporteExtendido(int? ciudadId, int? facilityId,
            int? planProgramaticoId, int? cuentaContableId, int? contraparteId, int? codigoAuditoriaId, int? accionNacionalId, int? territorioId,
            string descripcion, string notasAdicionales)
        {

            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.CiudadOrigenId = ciudadId;

            var reporte = reportesManager.GetReportePresupuestoGeneral(presupuestoActual.Id, ciudadId, facilityId);

            //Filtros
            reporte = FiltrarReporteGeneral(reporte, planProgramaticoId, cuentaContableId, contraparteId, codigoAuditoriaId, accionNacionalId, territorioId, descripcion, notasAdicionales);


            // Exportar Excel
            string templateDocument =
                            System.Web.HttpContext.Current.Server.MapPath("~/Templates/I04-ReporteExtendido.xlsx");

            var report = reportesManager.ExportReporteGeneralExtendido(templateDocument, reporte);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteGeneralExtendido-{0}{1}{2}.xlsx", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0')));

        }

        private List<Recurso> FiltrarReporteGeneral(List<Recurso> reporte, int? planProgramaticoId, int? cuentaContableId, int? contraparteId, int? codigoAuditoriaId, int? accionNacionalId, int? territorioId,
                string descripcion, string notasAdicionales)
        {
            var resultado = reporte;

            if (planProgramaticoId != null)
            {
                resultado = resultado.Where(p => p.PlanProgramaticoId == planProgramaticoId).ToList();
            }

            if (cuentaContableId != null)
            {
                resultado = resultado.Where(p => p.CuentaContableId == cuentaContableId).ToList();
            }

            if (contraparteId != null)
            {
                resultado = resultado.Where(p => p.ContraparteId == contraparteId).ToList();
            }

            if (codigoAuditoriaId != null)
            {
                resultado = resultado.Where(p => p.CodigoAuditoriaId == codigoAuditoriaId).ToList();
            }

            if (accionNacionalId != null)
            {
                resultado = resultado.Where(p => p.AccionNacionalId == accionNacionalId).ToList();
            }

            if (territorioId != null)
            {
                resultado = resultado.Where(p => p.TerritorioId == territorioId).ToList();
            }

            if (!string.IsNullOrEmpty(descripcion))
            {
                resultado = resultado.Where(c => c.Descripcion.Contains(descripcion)).ToList();
            }

            if (!string.IsNullOrEmpty(notasAdicionales))
            {
                resultado = resultado.Where(c => c.NotasAdicionales.Contains(notasAdicionales)).ToList();
            }

            return resultado;
        }

        public ActionResult ReporteMensual(int? ciudadId, int? facilityId,
            int? planProgramaticoId, int? cuentaContableId, int? contraparteId, int? codigoAuditoriaId, int? accionNacionalId, int? territorioId,
            string descripcion, string notasAdicionales,
            bool tieneCobertura = false)
        {
            var listaPlanProgramatico = planProgramaticoManager.GetAllPlan();

            if (facilityId != null)
            {
                listaPlanProgramatico = planProgramaticoManager.GetPlanByFacility(facilityId.Value);
            }

            // Parámetros
            ViewBag.CiudadId = new SelectList(ciudadesManager.GetAllCiudades(), "Id", "Nombre");
            ViewBag.FacilityId = ciudadId != null ?
                new SelectList(facilitiesManager.GetFacilitiesPorCiudad(ciudadId.Value), "Id", "NombreDespliegue") :
                new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");

            ViewBag.PlanProgramaticoId = new SelectList(listaPlanProgramatico, "Id", "NombreLista", new object(), planProgramaticoManager.GetParentsPlanIds());
            ViewBag.CuentaContableId = new SelectList(cuentasContablesManager.GetAllCuentasContables(), "Id", "NombreDespliegue");
            ViewBag.ContraparteId = new SelectList(new ContrapartesManager().GetAllContrapartes(), "Id", "NombreDespliegue");
            ViewBag.CodigoAuditoriaId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosAuditoria(), "Id", "Descripcion");
            ViewBag.AccionNacionalId = new SelectList(new AccionesNacionalesManager().GetAllAccionesNacionales(), "Id", "NombreDespliegue");
            ViewBag.TerritorioId = new SelectList(new TerritoriosManager().GetAllTerritorios(), "Id", "NombreDespliegue");

            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.CiudadOrigenId = ciudadId;

            var reporte = reportesManager.GetReportePresupuestoMensual(presupuestoActual.Id, ciudadId, facilityId);

            //Filtros
            reporte = FiltrarReporteMensual(reporte, planProgramaticoId, cuentaContableId, contraparteId, codigoAuditoriaId, accionNacionalId, territorioId, descripcion, notasAdicionales);

            reporte = tieneCobertura ?
                reporte.Where(r => r.Recurso.Cobertura.HasValue && r.Recurso.Cobertura.Value > 0).ToList() :
                reporte.Where(r => !r.Recurso.Cobertura.HasValue || r.Recurso.Cobertura.Value == 0).ToList();

            ViewBag.TotalReporte = reporte.Sum(r => r.Recurso.Monto);
            ViewBag.TotalReporteCobertura = reporte.Sum(r => r.Recurso.Cobertura);
            ViewBag.TieneCobertura = tieneCobertura;
            return View(reporte);
        }

        public ActionResult ExportarReporteMensual(int? ciudadId, int? facilityId,
            int? planProgramaticoId, int? cuentaContableId, int? contraparteId, int? codigoAuditoriaId, int? accionNacionalId, int? territorioId,
            string descripcion, string notasAdicionales,
            bool tieneCobertura = false)
        {

            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.CiudadOrigenId = ciudadId;

            var reporte = reportesManager.GetReportePresupuestoMensual(presupuestoActual.Id, ciudadId, facilityId);

            //Filtros
            reporte = FiltrarReporteMensual(reporte, planProgramaticoId, cuentaContableId, contraparteId, codigoAuditoriaId, accionNacionalId, territorioId, descripcion, notasAdicionales);

            reporte = tieneCobertura ?
                reporte.Where(r => r.Recurso.Cobertura.HasValue && r.Recurso.Cobertura.Value > 0).ToList() :
                reporte.Where(r => !r.Recurso.Cobertura.HasValue || r.Recurso.Cobertura.Value == 0).ToList();

            // Exportar Excel
            string templateDocument = System.Web.HttpContext.Current.Server.MapPath("~/Templates/I02-ReporteMensual.xlsx");

            var report = reportesManager.ExportReportePresupuestoMensual(templateDocument, reporte);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteMensual-{0}{1}{2}.xlsx", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0')));
        }

        private List<RecursoMes> FiltrarReporteMensual(List<RecursoMes> reporte,
            int? planProgramaticoId, int? cuentaContableId, int? contraparteId, int? codigoAuditoriaId, int? accionNacionalId, int? territorioId,
            string descripcion, string notasAdicionales)
        {
            var resultado = reporte;

            if (planProgramaticoId != null)
            {
                resultado = resultado.Where(p => p.Recurso.PlanProgramaticoId == planProgramaticoId).ToList();
            }

            if (cuentaContableId != null)
            {
                resultado = resultado.Where(p => p.Recurso.CuentaContableId == cuentaContableId).ToList();
            }

            if (contraparteId != null)
            {
                resultado = resultado.Where(p => p.Recurso.ContraparteId == contraparteId).ToList();
            }

            if (codigoAuditoriaId != null)
            {
                resultado = resultado.Where(p => p.Recurso.CodigoAuditoriaId == codigoAuditoriaId).ToList();
            }

            if (accionNacionalId != null)
            {
                resultado = resultado.Where(p => p.Recurso.AccionNacionalId == accionNacionalId).ToList();
            }

            if (territorioId != null)
            {
                resultado = resultado.Where(p => p.Recurso.TerritorioId == territorioId).ToList();
            }

            if (!string.IsNullOrEmpty(descripcion))
            {
                resultado = resultado.Where(c => c.Recurso.Descripcion.Contains(descripcion)).ToList();
            }

            if (!string.IsNullOrEmpty(notasAdicionales))
            {
                resultado = resultado.Where(c => c.Recurso.NotasAdicionales.Contains(notasAdicionales)).ToList();
            }

            return resultado;
        }

        public ActionResult ReporteBet()
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();

            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/E01-ReporteBET.xlsx");

            var report = reportesManager.GetReporteBet(templateDocument, presupuestoActual.Id);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteBET-{0}{1}{2}.xlsx", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0')));
        }

        public ActionResult Reportes()
        {
            return View();
        }

        // POST: Presupuestos/Create
        public ActionResult Create(int gestion)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                if (gestion != -1)
                {
                    var presupuesto = new Presupuesto
                    {
                        Gestion = gestion,
                        EstadoPresupuestoId = 2,
                        TipoPresupuestoId = 1,
                        Version = 0,
                        Activo = true,
                        UsuarioCreacion = usuarioActual,
                        UsuarioModificacion = usuarioActual
                    };

                    var resultado = presupuestosManager.InsertPresupuesto(presupuesto);
                }

                return RedirectToAction("Gestion");
            }
            catch
            {
                return RedirectToAction("Gestion");
            }
        }



        // GET: Presupuestos/CambiarEstado/2018
        public ActionResult CambiarEstado(int gestion, int version, short estadoId)
        {

            var resultado = presupuestosManager.CambiarEstado(gestion, version, estadoId);

            return RedirectToAction("Gestion", "Presupuestos");
        }


        public ActionResult CambiarEstadoFacility(int presupuestoId, int ciudadId, int facilityId, short estadoId)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            // Verificar si se trata de un facility compartido
            var facilityActual = facilitiesManager.GetFacility(facilityId);

            if (facilityActual.Codigo == "R0031557")
            {
                //Si es compartido validar que existe un presupuesto compartido para la ciudad actual
                var presupuestoCompartidoActual = presupuestosManager.GetPresupuestoCompartidoActual(presupuestoId, facilityId, ciudadId);
                if (presupuestoCompartidoActual != null)
                {
                    //Si existe el presupuesto compartido cambiar de estado y devolver a la pantalla de Facilities.
                    var resultadoCompartido = presupuestosManager.CambiarEstadoFacilityCompartido(presupuestoCompartidoActual.Id, ciudadId, estadoId);
                    if (Session["ciudadId"] != null && Session["ciudadId"].ToString() == "12")
                    {
                        return RedirectToAction("Facilities", "Recursos", new { ciudadId = 12 });
                    }
                    else
                    {
                        return RedirectToAction("Facilities", "Recursos", new { ciudadId });
                    }
                }
            }

            var resultado = presupuestosManager.CambiarEstadoFacility(presupuestoId, facilityId, estadoId, usuarioActual);
            return RedirectToAction("Facilities", "Recursos", new { ciudadId });
        }


        // GET: Presupuestos/Habilitar/2018
        public ActionResult Habilitar(int gestion)
        {
            var resultado = presupuestosManager.HabilitarGestion(gestion);

            return RedirectToAction("Index");
        }

        // GET: Presupuestos/Cerrar/2018
        public ActionResult Cerrar(int gestion)
        {
            var resultado = presupuestosManager.CerrarGestion(gestion);
            return RedirectToAction("Index");
        }

        /* Acciones sobre Observaciones */

        // POST: CuentasAsientos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateObservacion(Observacion observacion)
        {
            try
            {
                var usuarioActual = HttpContext.User.Identity.Name;
                observacion.UsuarioCreacion = usuarioActual;
                observacion.UsuarioModificacion = usuarioActual;

                observacion.TipoObservacionId = 1; // Observación de Presupuesto

                observacion.EsNacional = HttpContext.User.IsInRole("PPTO-CLR");

                observacionesManager.InsertObservacion(observacion);

                return RedirectToAction("Revision", "Recursos", new { facilityId = observacion.FacilityId, ciudadId = observacion.CiudadId, esCompartido = observacion.EsCompartido, ciudadOrigenId = observacion.CiudadOrigenId });
            }
            catch
            {
                return RedirectToAction("Revision", "Recursos", new { facilityId = observacion.FacilityId, ciudadId = observacion.CiudadId, esCompartido = observacion.EsCompartido, ciudadOrigenId = observacion.CiudadOrigenId });
            }
        }

        // POST: CuentasAsientos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditObservacion(Observacion observacion)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                observacion.UsuarioModificacion = usuarioActual;
                var resultado = observacionesManager.UpdateObservacion(observacion);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult AprobarObservacion(int id, bool esCompartido)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var observacion = observacionesManager.GetObservacion(id);
            try
            {
                observacion.Aprobado = true;
                observacion.UsuarioModificacion = usuarioActual;
                var resultado = observacionesManager.UpdateObservacion(observacion);

                var presupuestoActual = presupuestosManager.GetPresupuestoActual();
                var facilityActual = facilitiesManager.GetFacility(observacion.FacilityId.Value);

                if (esCompartido)
                {
                    var presupuestoCompartidoActual = presupuestosManager.GetPresupuestoCompartidoActual(presupuestoActual.Id, observacion.FacilityId.Value, observacion.CiudadId.Value);
                    if (presupuestoCompartidoActual != null)
                    {
                        return RedirectToAction("Revision", "Recursos", new { facilityId = observacion.FacilityId, ciudadId = observacion.CiudadId, esCompartido = true, ciudadOrigenId = facilityActual.CiudadId });
                    }
                }

                return RedirectToAction("Revision", "Recursos", new { facilityId = observacion.FacilityId, ciudadId = observacion.CiudadId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Revision", "Recursos", new { facilityId = observacion.FacilityId, ciudadId = observacion.CiudadId });
            }
        }

        public ActionResult DeleteObservacion(int id, bool esCompartido)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var observacion = observacionesManager.GetObservacion(id);
            try
            {
                var resultado = observacionesManager.DeleteObservacion(id, usuarioActual);

                var presupuestoActual = presupuestosManager.GetPresupuestoActual();
                var facilityActual = facilitiesManager.GetFacility(observacion.FacilityId.Value);

                if (esCompartido)
                {
                    var presupuestoCompartidoActual = presupuestosManager.GetPresupuestoCompartidoActual(presupuestoActual.Id, observacion.FacilityId.Value, observacion.CiudadId.Value);
                    if (presupuestoCompartidoActual != null)
                    {
                        return RedirectToAction("Revision", "Recursos", new { facilityId = observacion.FacilityId, ciudadId = observacion.CiudadId, esCompartido = true, ciudadOrigenId = facilityActual.CiudadId });
                    }
                }
                return RedirectToAction("Revision", "Recursos", new { facilityId = observacion.FacilityId, ciudadId = observacion.CiudadId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Revision", "Recursos", new { facilityId = observacion.FacilityId, ciudadId = observacion.CiudadId });
            }
        }

        [HttpPost]
        public ActionResult Importar(HttpPostedFileBase archivo)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            // Obtener Periodo de Planilla Actual
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            if (archivo != null && archivo.ContentLength > 0)
            {

                var rutaArchivoImportado = Path.Combine(Server.MapPath("~/Imports"), Path.GetFileName(archivo.FileName));
                archivo.SaveAs(rutaArchivoImportado);

                var resultado = presupuestosManager.ImportarPresupuesto(presupuestoActual.Id, rutaArchivoImportado, archivo.FileName, archivo.ContentLength, usuarioActual);
            }


            return RedirectToAction("Ciudades", new { presupuestoId = presupuestoActual.Id });
        }

    }
}
